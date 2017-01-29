using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Web;
using HtmlAgilityPack;
using Fizzler.Systems.HtmlAgilityPack;
using System.Text.RegularExpressions;
using System.IO;
using System.Reflection;

namespace LMB_Archivist_Formed
{
    enum ArchiveButtonState
    {
        Stopped,
        AreSure,
        Running,
    }

    enum ArchiveOptionState
    {
        None,
        SaveUserPosts,
        SaveUserPages,
        SaveUserTopics,
        SaveTopics
    }

    public partial class Form1 : Form
    {
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

        //Enums
        private ArchiveButtonState buttonState;
        private ArchiveOptionState optionState;

        //Counters
        private int postCount = 0;
        private int postCounter = 0;
        private int pageCount = 0;
        private int pageCounter = 0;
        private int imgCount = 0;

        //
        public Form1()
        {
            InitializeComponent();
            //Start(349888);

            buttonState = ArchiveButtonState.Stopped;
        }

        //
        private void archive_post_radio_CheckedChanged(object sender, EventArgs e)
        {
            changeArchiveMode();
            optionState = ArchiveOptionState.SaveUserPosts;
        }

        //
        private void archive_topic_radio_CheckedChanged(object sender, EventArgs e)
        {
            changeArchiveMode();
            optionState = ArchiveOptionState.SaveTopics;
        }

        //
        private void archive_radio_posts_CheckedChanged(object sender, EventArgs e)  
        {
            if(archive_radio_posts.Checked == false) { return; }
            archive_radio_pages.Checked = false;
            archive_radio_topics.Checked = false;
            optionState = ArchiveOptionState.SaveUserPosts;
        }

        //
        private void archive_radio_pages_CheckedChanged(object sender, EventArgs e)
        {
            if (archive_radio_pages.Checked == false) { return; }
            archive_radio_posts.Checked = false;
            archive_radio_topics.Checked = false;
            optionState = ArchiveOptionState.SaveUserPages;
        }

        //
        private void archive_radio_topics_CheckedChanged(object sender, EventArgs e)
        {
            if (archive_radio_topics.Checked == false) { return; }
            archive_radio_posts.Checked = false;
            archive_radio_pages.Checked = false;
            optionState = ArchiveOptionState.SaveUserTopics;
        }

        //
        private void changeArchiveMode()
        {
            this.archive_post_panel.Enabled = this.archive_post_radio.Checked;
            this.archive_topic_panel.Enabled = this.archive_topic_radio.Checked;

            this.button_archive.Enabled = true;
        }
        
        //Start button
        private void button_archive_Click(object sender, EventArgs e)
        {
            switch(buttonState)
            {
                case ArchiveButtonState.Stopped:
                    textBoxTop.Text = "";
                    textBoxBottom.Text = "";
                    switch(optionState)
                    {
                        case ArchiveOptionState.SaveUserPosts:
                            int userId;

                            if (int.TryParse(textBoxUserId.Text, out userId))
                            {
                                string completeUrl = MESSAGE_URL + userId + '/';

                                //Start Task
                                Print(textBoxBottom, "GETTING POST PAGE NUMBER 1");
                                HandlePostListDocument(completeUrl);

                                buttonState = ArchiveButtonState.Running;
                                button_archive.Text = "ARCHIVING!";

                                //Disable everything
                                this.archive_post_panel.Enabled = false;
                                this.archive_topic_panel.Enabled = false;
                                this.archive_post_radio.Enabled = false;
                                this.archive_topic_radio.Enabled = false;
                            }
                            else
                            {
                                Print(textBoxTop, "INVALID USER ID: " + textBoxUserId.Text);
                            }
                            break;

                        case ArchiveOptionState.SaveUserPages:
                            Print(textBoxTop, "UNIMPLEMENTED FEATURE : ARCHIVE USER PAGES");
                            break;

                        case ArchiveOptionState.SaveUserTopics:
                            Print(textBoxTop, "UNIMPLEMENTED FEATURE : ARCHIVE USER TOPICS");
                            break;

                        case ArchiveOptionState.SaveTopics:

                            string url = textBoxUrl.Text;
                            if(url.StartsWith("community.lego.com/t5/"))
                            {
                                url = "https://" + url;
                            }
                            if (url.StartsWith("https://community.lego.com/t5/"))
                            {
                                HandleTopicDocument(url, true);

                                buttonState = ArchiveButtonState.Running;
                                button_archive.Text = "ARCHIVING!";

                                //Disable everything
                                this.archive_post_panel.Enabled = false;
                                this.archive_topic_panel.Enabled = false;
                                this.archive_post_radio.Enabled = false;
                                this.archive_topic_radio.Enabled = false;
                            }
                            else
                            {
                                Print(textBoxTop, "INVALID URL: " + url);
                            }
                            break;
                    }
                    break;

                case ArchiveButtonState.Running:
                    buttonState = ArchiveButtonState.AreSure;
                    button_archive.Text = "Cancel and Quit?";
                    Task.Delay(3000).ContinueWith(t => ResetButtonToRunning());
                    break;

                case ArchiveButtonState.AreSure:
                    Application.Exit();
                    break;
            }
        }

        //Return button from an "Are you sure" state to a running state.
        private void ResetButtonToRunning()
        {
            if(buttonState == ArchiveButtonState.AreSure)
            {
                //Thread safety
                if (this.InvokeRequired)
                {
                    Invoke(new MethodInvoker(delegate () {
                        ResetButtonToRunning();
                    }));
                }
                else
                {
                    buttonState = ArchiveButtonState.Running;
                    button_archive.Text = "ARCHIVING!";
                }
            }
        }

        //Return button to a stopped state
        private void ResetButtonToStopped()
        {
            //Thread safety
            if (this.InvokeRequired)
            {
                Invoke(new MethodInvoker(delegate () {
                    ResetButtonToStopped();
                }));
            }
            else
            {
                buttonState = ArchiveButtonState.Stopped;
                button_archive.Text = "START ARCHIVE!";
            }
        }

        //Set finished state for when finished
        private void SetFinished()
        {
            //Thread safety
            if (this.InvokeRequired)
            {
                Invoke(new MethodInvoker(delegate () {
                    SetFinished();
                }));
            }
            else
            {
                buttonState = ArchiveButtonState.Stopped;
                button_archive.Text = "START ARCHIVE!";
                this.archive_post_radio.Enabled = true;
                this.archive_topic_radio.Enabled = true;
                this.archive_post_panel.Enabled = this.archive_post_radio.Checked;
                this.archive_topic_panel.Enabled = this.archive_topic_radio.Checked;
            }
        }

        //Print text in a provided text box.
        private void Print(TextBox textBox, string message)
        {
            //Thread safety
            if (this.InvokeRequired)
            {
                Invoke(new MethodInvoker(delegate () {
                    Print(textBox, message);
                }));
            }
            else
            {
                textBox.AppendText(message);
                textBox.AppendText(Environment.NewLine);
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

        //Handle post-list page from web request.
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
                
                if(usernameLink == null)
                {
                    ResetButtonToStopped();
                    Print(textBoxTop, "USER HAS NO POSTS");

                    //Should still happen on UI thread, don't bother with invokes.
                    this.archive_post_radio.Enabled = true;
                    this.archive_topic_radio.Enabled = true;
                    this.archive_post_radio.Checked = false;
                    this.archive_topic_radio.Checked = false;

                    return;
                }

                username = docNode.QuerySelector("a.lia-user-name-link").FirstChild.InnerHtml;
                Directory.CreateDirectory(SAVE_LOCATION + username + "/");
                Directory.CreateDirectory(SAVE_LOCATION + SAVE_IMAGE_LOCATION);
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
                Print(textBoxTop, "POST COUNT : " + postCount + "");

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
                    Print(textBoxTop, "FINAL POST COUNT : " + postCount);

                    return;
                }

                //There is a next page, so if this is the first page
                else if(!Regex.Match(url, @"\d+$").Success)
                {
                    int.TryParse(docNode.QuerySelectorAll("[class^=lia-js-data-pageNum-]").Last().InnerHtml, out postCount);
                    postCount *= 20;
                    Print(textBoxTop, "POST ESTIMATE : ~" + postCount);
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

                    Print(textBoxBottom, "GETTING POST PAGE NUMBER " + pageNumber);
                }
            }

            //Get the next list page
            #pragma warning disable 4014
            Task.Run(() => HandlePostListDocument(next.GetAttributeValue("href", ""))).ConfigureAwait(false);
            #pragma warning restore 4014
        }

        //Handle post page from web request
        private async void HandlePostDocument(string url)
        {
            //Read the document from the request result
            var doc = new HtmlAgilityPack.HtmlDocument();   //Document
            var webResponse = await GetRequestStreamAsync(url);
            doc.Load(webResponse.GetResponseStream());
            webResponse.Close();

            var docNode = doc.DocumentNode; //Document Node

            var postId = Regex.Match(url, @"m-p\/\d+").Value.Replace("m-p/", "");

            Print(textBoxBottom, "ACQUIRED POST OF ID: " + postId);

            //Node of post
            var post = docNode.QuerySelector("div.message-uid-" + postId);

            if (post == null)
            {
                Print(textBoxBottom, "POST OF ID: " + postId + " REQUIRES LOGIN. CANNOT GET.");
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

        private async void HandleTopicDocument(string url, bool cleanExtras = false)
        {
            int pageNum = 1;

            //Logic for cleaning extra fluff in URI and getting a page number
            var uri = new Uri(url);
            var segments = uri.Segments;
            var killSegments = false;       //True when fluff starts in URI

            //Walk through URI segments
            for (int i = 0; i < segments.Length; i++)
            {
                if(segments[i] == "highlight/")
                {
                    killSegments = true;
                }
                else if(segments[i] == "page/")
                {
                    killSegments = true;
                    if (segments.Length > i && !cleanExtras)
                    {
                        if(int.TryParse(segments[i + 1], out pageNum))
                        {
                            break;
                        }
                    }
                }

                //Kill uneeded segments only if we are cleaning the extra stuff.
                if(killSegments && cleanExtras)
                {
                    segments[i] = "";
                }
            }

            url = uri.Scheme + "://" + uri.Host + String.Join("", segments);

            //Read the document from the request result
            var doc = new HtmlAgilityPack.HtmlDocument();   //Document
            var webResponse = await GetRequestStreamAsync(url);
            doc.Load(webResponse.GetResponseStream());
            webResponse.Close();

            var docNode = doc.DocumentNode; //Document Node

            var messageList = docNode.QuerySelector(".message-list");
            
            //Modify post document for exporting
            var quilts = messageList.QuerySelectorAll("div.lia-quilt-column-main-right");
            foreach(var quilt in quilts)
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
                    #pragma warning disable 4014
                    Task.Run(() => HandleImageAsset(src)).ConfigureAwait(false);
                    #pragma warning restore 4014
                }
            }

            //Number of pages
            int pageCount = 0;
            int.TryParse(docNode.QuerySelectorAll("[class^=lia-js-data-pageNum-]").Last().InnerHtml, out pageCount);

            //Title of topic
            var title = docNode.QuerySelector(".custom-title").InnerHtml;

            //Logs
            if (cleanExtras)
            {
                Print(textBoxTop, "Topic : " + title);
                Print(textBoxTop, "Page Count : " + pageCount);
            }

            Print(textBoxBottom, "Saving page : " + pageNum);

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
                SetFinished();
                return;
            }
            else
            {
                next = next.QuerySelector("li.lia-paging-page-next").QuerySelector("a.lia-link-navigation");

                //Return if no list page after this one
                if (next == null)
                {
                    SetFinished();
                    return;
                }
            }

            await Task.Run(() => HandleTopicDocument(next.GetAttributeValue("href", "")));
        }

        //Increment the post counter. Should be called when a post task is done.
        private void incrementPostCounter()
        {
            postCounter++;

            if (postCounter >= postCount && postCount != 0)
            {
                SetFinished();
                Print(textBoxTop, "ARCHIVE TASK COMPLETED");
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
                        imgCount--;
                    }
                }
            }
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
