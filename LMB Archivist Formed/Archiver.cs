using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LMB_Archivist_Formed
{
    enum TopicReadingState
    {
        MustGoToFirst,
        IsFirst,
        IsPastFirst
    }

    class Archiver
    {
        private Form1 form;

        //Base LMB URL
        private const string BASE_URL = "https://community.lego.com/";

        //URL For LMB user's posts
        private const string MESSAGE_URL = "https://community.lego.com/t5/forums/recentpostspage/post-type/message/user-id/";

        //Asset location
        private const string ASSET_LOCATION = "assets/";

        //Save location for image assets
        private const string SAVE_IMAGE_LOCATION = "images/";

        //Save location for image assets
        private const string SAVE_TOPIC_LOCATION = "topics/";

        //Save location for posts
        private const string SAVE_LOCATION = "output/";

        //Username
        private string username;

        //Task Factory
        private TaskFactory taskFactory = new TaskFactory();

        //Locks
        private Object lockerPost = new Object();
        private Object lockerImage = new Object();

        //Counters and finish states
        private int postCount = 0;
        private int postCounter = 0;
        private int imgCount = 0;
        private bool topicDone = true;

        //Constructor
        public Archiver(Form1 form)
        {
            this.form = form;

            Directory.CreateDirectory(SAVE_LOCATION + SAVE_IMAGE_LOCATION);
        }

        //Start User Post Archiving
        public void StartUserPostArchiving(int userId)
        {
            string completeUrl = MESSAGE_URL + userId + '/';

            //Start Task
            form.Print(TextBoxChoice.TextBoxBottom, "Getting post page number : 1");
            HandlePostListDocument(completeUrl);
        }

        //Start Topic Archiving
        public void StartTopicArchiving(string url)
        {
            HandleTopicDocument(url, TopicReadingState.MustGoToFirst);
        }

        //Check if good to finish archiving, and set finished if so.
        private void CheckSetFinished()
        {
            if (
                imgCount <= 0 &&
                postCounter >= postCount &&
                topicDone)
            {
                postCount = 0;
                postCounter = 0;
                imgCount = 0;

                form.SetFinished();
                form.Print(TextBoxChoice.TextBoxTop, "Archive Task Completed!");
            }
        }

        //TAP Asynchronous Web Request for LMB Document
        private async Task<WebResponse> GetRequestStreamAsync(string url)
        {
            var request = (System.Net.HttpWebRequest)WebRequest.Create(url);
            request.CookieContainer = new CookieContainer();

            Task<WebResponse> task = taskFactory.FromAsync(request.BeginGetResponse, request.EndGetResponse, null);
            return await task;
        }

        //Handle post-list page.
        private async void HandlePostListDocument(string url)
        {
            var doc = new HtmlAgilityPack.HtmlDocument();   //Document
            var webResponse = await GetRequestStreamAsync(url);
            doc.Load(webResponse.GetResponseStream());
            webResponse.Close();

            var docNode = doc.DocumentNode; //Document Node

            //Set Save Location based on username, but only for the first page
            if (!Regex.Match(url, @"\d+$").Success)
            {
                var usernameLink = docNode.QuerySelector("a.lia-user-name-link");

                if (usernameLink == null)
                {
                    form.ResetButtonToStopped();
                    form.Print(TextBoxChoice.TextBoxTop, "User has no posts.");

                    form.SetFinished();

                    return;
                }

                username = docNode.QuerySelector("a.lia-user-name-link").FirstChild.InnerHtml;
                form.Print(TextBoxChoice.TextBoxTop, "User name : " + username);
                Directory.CreateDirectory(SAVE_LOCATION + username + "/");
                var overview = new HtmlAgilityPack.HtmlDocument();
                overview.Load(ASSET_LOCATION + "overview.html");
                overview.DocumentNode.QuerySelector("h1").InnerHtml += "Posts For " + username;
                overview.Save(SAVE_LOCATION + "overview-" + username + ".html");
            }

            //Start web requests for each post on the list
            foreach (var item in docNode.QuerySelectorAll("span.lia-message-unread"))
            {
                //Deleted topics don't have an anchor so there might not be one. Skip it if it has none.
                var itemAnchor = item.QuerySelector("a");

                if (itemAnchor != null)
                {
                    #pragma warning disable 4014
                    Task.Run(() => HandlePostDocument(BASE_URL + itemAnchor.GetAttributeValue("href", ""))).ConfigureAwait(false);
                    #pragma warning restore 4014
                }
            }

            //Node containing link for the next post-list
            var next = docNode.QuerySelector("li.lia-paging-page-next");

            //Return if only one list page
            if (next == null)
            {
                postCount = docNode.QuerySelectorAll("span.lia-message-unread").Count();
                form.Print(TextBoxChoice.TextBoxTop, "Post count : " + postCount + "");

                return;
            }
            else
            {
                next = next.QuerySelector("a.lia-link-navigation");

                //Return if no list page after this one
                if (next == null)
                {
                    //Calculate final post count based on the final page.
                    postCount += docNode.QuerySelectorAll("span.lia-message-unread").Count() - 20;
                    form.Print(TextBoxChoice.TextBoxTop, "Final post count : " + postCount);

                    return;
                }

                //There is a next page, so if this is the first page
                else if (!Regex.Match(url, @"\d+$").Success)
                {
                    int.TryParse(docNode.QuerySelectorAll("[class^=lia-js-data-pageNum-]").Last().InnerHtml, out postCount);
                    postCount *= 20;
                    form.Print(TextBoxChoice.TextBoxTop, "Post count estimate : ~" + postCount);
                }
            }

            //Page number stuff
            var cssClasses = next.GetAttributeValue("class", "").Split(' ');
            foreach (var cssClass in cssClasses)
            {
                if (cssClass.StartsWith("lia-js-data-pageNum-"))
                {
                    int pageNumber = 0;
                    Int32.TryParse(Regex.Match(cssClass, @"\d+$").Value, out pageNumber);

                    form.Print(TextBoxChoice.TextBoxBottom, "Getting post page number : " + pageNumber);
                }
            }

            //Get the next list page
            #pragma warning disable 4014
            Task.Run(() => HandlePostListDocument(next.GetAttributeValue("href", ""))).ConfigureAwait(false);
            #pragma warning restore 4014
        }

        //Handle post page.
        private async void HandlePostDocument(string url)
        {
            //Read the document from the request result
            var doc = new HtmlAgilityPack.HtmlDocument();   //Document
            var webResponse = await GetRequestStreamAsync(url);
            doc.Load(webResponse.GetResponseStream());
            webResponse.Close();

            var docNode = doc.DocumentNode; //Document Node

            var postId = Regex.Match(url, @"m-p\/\d+").Value.Replace("m-p/", "");

            form.Print(TextBoxChoice.TextBoxBottom, "Acquired post of ID : " + postId);

            //Node of post
            var post = docNode.QuerySelector("div.message-uid-" + postId);

            if (post == null)
            {
                form.Print(TextBoxChoice.TextBoxBottom, "Post of ID : " + postId + " requires login. Cannot get.");
                incrementPostCounter();
                return; //Post requires login. Ignore it.
            }

            //Modify post document for exporting
            post.QuerySelector("div.lia-quilt-column-main-right")
                .QuerySelector("div.lia-quilt-column-alley")
                .Attributes["class"].Value = "lia-quilt-column-alley lia-quilt-column-alley-left";

            var images = post.QuerySelectorAll("img");

            //Get images and set the src for them, but not in that order.
            foreach (HtmlNode image in images)
            {
                var src = image.GetAttributeValue("src", "");

                string imageFileLocation = SAVE_IMAGE_LOCATION + Path.GetFileName(src);

                image.SetAttributeValue("src", "../" + imageFileLocation);

                if (!File.Exists("output/" + imageFileLocation))
                {
                    imgCount++;

                    #pragma warning disable 4014
                    Task.Run(() => HandleImageAsset(src)).ConfigureAwait(false);
                    #pragma warning restore 4014
                }
            }

            //Date of post
            var date = GetDateFromNode(post);

            //Title of post
            var title = docNode.QuerySelector("h3.custom-title").InnerHtml.Trim('"');

            //Post File Name
            string postFileName = legalizeFilePath(date + "_" + postId + "_" + title) + ".html";

            //Prevent interuption when writing post overview
            lock (lockerPost)
            {
                //Save overview document
                var overview = new HtmlAgilityPack.HtmlDocument();
                overview.Load(SAVE_LOCATION + "overview-" + username + ".html", Encoding.UTF8);
                overview.DocumentNode.QuerySelector("ul").InnerHtml +=
                    "<li><a href=\"" + username + "/" + postFileName + "\">" + postFileName + "</a></li>\n";
                overview.Save(SAVE_LOCATION + "overview-" + username + ".html", Encoding.UTF8);
            }

            //Save post document
            var output = new HtmlAgilityPack.HtmlDocument();
            output.Load(ASSET_LOCATION + "default.html");
            output.DocumentNode.QuerySelector("div.even-row").AppendChild(post);
            output.Save(SAVE_LOCATION + username + "/" + postFileName);

            incrementPostCounter();
        }

        //Handle topic page.
        private async void HandleTopicDocument(string url, TopicReadingState state)
        {
            topicDone = false;

            int pageNum = 1;

            //Logic for cleaning extra fluff in URI and getting a page number
            var segments = new Uri(url).Segments;

            //Walk through URI segments
            for (int i = 0; i < segments.Length; i++)
            {
                if (segments[i] == "page/")
                {
                    if (segments.Length > i)
                    {
                        if (int.TryParse(segments[i + 1], out pageNum))
                        {
                            break;
                        }
                    }
                }
            }

            //Read the document from the request result
            var doc = new HtmlAgilityPack.HtmlDocument();   //Document
            var webResponse = await GetRequestStreamAsync(url);
            doc.Load(webResponse.GetResponseStream());
            webResponse.Close();

            var docNode = doc.DocumentNode; //Document Node
            
            //Number of pages
            int pageCount = 0;
            var pageNumNodes = docNode.QuerySelectorAll("[class^=lia-js-data-pageNum-]");
            if (pageNumNodes.Count() > 0)
            {
                int.TryParse(pageNumNodes.Last().InnerHtml, out pageCount);

                if(state == TopicReadingState.MustGoToFirst)
                {
                    await Task.Run(() => HandleTopicDocument(
                        docNode.QuerySelector("a.lia-js-data-pageNum-1").GetAttributeValue("href", ""),
                        TopicReadingState.IsFirst));
                    return;
                }
            }
            else
            {
                pageCount = 1;
            }

            var messageList = docNode.QuerySelector(".message-list");

            //Modify post document for exporting
            var quilts = messageList.QuerySelectorAll("div.lia-quilt-column-main-right");
            foreach (var quilt in quilts)
            {
                quilt.QuerySelector("div.lia-quilt-column-alley")
                    .Attributes["class"].Value = "lia-quilt-column-alley lia-quilt-column-alley-left";
            }

            var displays = messageList.QuerySelectorAll(".lia-linear-display-message-view");
            foreach (var display in displays)
            {
                display.Attributes["class"].Value = "lia-linear-display-message-view even-row";
            }

            var images = messageList.QuerySelectorAll("img");

            //Get images and set the src for them, but not in that order.
            foreach (HtmlNode image in images)
            {
                var src = image.GetAttributeValue("src", "");

                string imageFileLocation = SAVE_IMAGE_LOCATION + Path.GetFileName(src);

                image.SetAttributeValue("src", "../" + imageFileLocation);

                if (!File.Exists("output/" + imageFileLocation))
                {
                    imgCount++;

                    #pragma warning disable 4014
                    Task.Run(() => HandleImageAsset(src)).ConfigureAwait(false);
                    #pragma warning restore 4014
                }
            }

            //Title of topic
            var title = docNode.QuerySelector(".custom-title").InnerHtml;

            //Logs
            if (state == TopicReadingState.IsFirst)
            {
                form.Print(TextBoxChoice.TextBoxTop, "Topic : " + title);
                form.Print(TextBoxChoice.TextBoxTop, "Page Count : " + pageCount);
            }

            form.Print(TextBoxChoice.TextBoxBottom, "Saving page : " + pageNum);

            //Node containing link for the next post-list
            var next = docNode.QuerySelector(".lia-paging-full-left-position");

            Directory.CreateDirectory(SAVE_LOCATION + SAVE_TOPIC_LOCATION);

            //Post File Name
            string topicFileName = legalizeFilePath(title + "_" + pageNum) + ".html";

            //Save page document
            var output = new HtmlAgilityPack.HtmlDocument();
            output.Load(ASSET_LOCATION + "topic.html");
            SetTopicPageHeader(output.DocumentNode, pageCount, pageNum, legalizeFilePath(title + "_"));
            output.DocumentNode.QuerySelector("div.lia-content").AppendChild(messageList);
            output.Save(SAVE_LOCATION + SAVE_TOPIC_LOCATION + "/" + topicFileName);

            //Return if only one list page
            if (next == null)
            {
                topicDone = true;
                CheckSetFinished();
                return;
            }
            else
            {
                next = next.QuerySelector("li.lia-paging-page-next").QuerySelector("a.lia-link-navigation");

                //Return if no list page after this one
                if (next == null)
                {
                    topicDone = true;
                    CheckSetFinished();
                    return;
                }
            }

            await Task.Run(() => HandleTopicDocument(
                next.GetAttributeValue("href", ""),
                TopicReadingState.IsPastFirst));
        }

        //Increment the post counter. Should be called when a post task is done.
        private void incrementPostCounter()
        {
            postCounter++;

            //Just to be save, in the bizarre scenario where posts are recieved before the post count is made.
            if (postCount > 0)
            {
                CheckSetFinished();
            }
        }

        //Write Image from Web Response
        private async void HandleImageAsset(string url)
        {
            //Compatibility for older LMB emotes that still hang around for SOME reason.
            if (url.StartsWith("/html/"))
            {
                url = BASE_URL + url;
            }
            try
            {
                var webResponse = await GetRequestStreamAsync(url);

                //Prevent interuption when writing post image
                lock (lockerImage)
                {
                    //I don't know, I just copied this off Stack Overflow and it works.
                    using (BinaryReader reader = new BinaryReader(webResponse.GetResponseStream()))
                    {
                        Byte[] lnByte = reader.ReadBytes(1 * 1024 * 1024 * 10);
                        using (FileStream lxFS = new FileStream("output/" + SAVE_IMAGE_LOCATION + Path.GetFileName(url), FileMode.Create))
                        {
                            lxFS.Write(lnByte, 0, lnByte.Length);
                            form.Print(TextBoxChoice.TextBoxBottom, "Saving Image : " + Path.GetFileName(url));
                        }
                    }
                }
            }
            catch
            {
                form.Print(TextBoxChoice.TextBoxBottom, "Failed to save Image : " + Path.GetFileName(url));
            }

            //Decrement image count back to zero for each image that is finally archived.
            imgCount--;
            CheckSetFinished();
        }

        //Replace illegal characters in given file path
        private string legalizeFilePath(string filePath)
        {
            string regexSearch = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
            Regex r = new Regex(string.Format("[{0}]", Regex.Escape(regexSearch)));
            return r.Replace(filePath, "");
        }

        //Get the date from an LMB HtmlNode
        private string GetDateFromNode(HtmlNode docNode)
        {
            var date = "";

            var dateContainer = docNode.QuerySelector("span.lia-message-posted-on");

            //But for me, it was Tuesday
            if (dateContainer.QuerySelector("span.local-friendly-date") != null)
            {
                date = docNode.QuerySelector("span.local-friendly-date").GetAttributeValue("title", "").Remove(0, 3);
            }

            //Non-friendly date, IE: "‎10-28-2013 09:03 PM"
            else if (dateContainer.QuerySelector("span.local-date") != null)
            {
                date = (
                    dateContainer.QuerySelector("span.local-date").InnerHtml +
                    " " +
                    dateContainer.QuerySelector("span.local-time").InnerHtml).Remove(0, 3);
            }

            //Standardize Date
            if (date != String.Empty)
            {
                date = DateTime.Parse(date).ToString("yyyy-MM-dd HH\uA789mm");  //It's a colon, if you're wondering.
            }
            else
            {
                date = "no-date";
            }

            return date;
        }

        //Sets the paging header of a topic page, the one with the numbers and anchors to other pages.
        private void SetTopicPageHeader(HtmlNode topicNode, int count, int current, string name)
        {
            topicNode.QuerySelector("ul.lia-paging-full").Attributes["class"].Value = "lia-paging-full " + "page-count-type-" + Math.Min(count, 5);

            topicNode.QuerySelector("a.page-first").Attributes["href"].Value = name + 1 + ".html";
            topicNode.QuerySelector("a.page-prev").Attributes["href"].Value = name + (current - 1) + ".html";
            topicNode.QuerySelector("a.page-next").Attributes["href"].Value = name + (current + 1) + ".html";
            topicNode.QuerySelector("a.page-last").Attributes["href"].Value = name + count + ".html";

            topicNode.QuerySelector("a.page-first").InnerHtml = "1";
            topicNode.QuerySelector("a.page-prev").InnerHtml = (current - 1).ToString();
            topicNode.QuerySelector("a.page-current").InnerHtml = current.ToString();
            topicNode.QuerySelector("a.page-next").InnerHtml = (current + 1).ToString();
            topicNode.QuerySelector("a.page-last").InnerHtml = count.ToString();

            topicNode.QuerySelector("a.page-prev-large").Attributes["href"].Value = current <= 1 ? "#" : name + (current - 1) + ".html";
            topicNode.QuerySelector("a.page-next-large").Attributes["href"].Value = current == count ? "#" : name + (current + 1) + ".html";

            switch (current)
            {
                case 1:
                    topicNode.QuerySelector("li.page-prev").Attributes.Add("style", "display:none;");
                    topicNode.QuerySelector("li.page-current").Attributes.Add("style", "display:none;");
                    break;
                case 2:
                    topicNode.QuerySelector("li.page-prev").Attributes.Add("style", "display:none;");
                    break;
            }

            switch (count - current)
            {
                case 1:
                    topicNode.QuerySelector("li.page-next").Attributes.Add("style", "display:none;");
                    break;
                case 0:
                    topicNode.QuerySelector("li.page-current").Attributes.Add("style", "display:none;");
                    topicNode.QuerySelector("li.page-next").Attributes.Add("style", "display:none;");
                    break;
            }
        }
    }
}
