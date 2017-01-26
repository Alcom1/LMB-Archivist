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

    enum ArchiveOptions
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

        //Counters
        private int postCount = 0;
        private int postCounter = 0;
        private int pageCount = 0;
        private int pageCounter = 0;

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
        }

        //
        private void archive_topic_radio_CheckedChanged(object sender, EventArgs e)
        {
            changeArchiveMode();
        }

        //
        private void archive_radio_posts_CheckedChanged(object sender, EventArgs e)
        {
            archive_radio_pages.Checked = false;
            archive_radio_topics.Checked = false;
        }

        //
        private void archive_radio_pages_CheckedChanged(object sender, EventArgs e)
        {
            archive_radio_posts.Checked = false;
            archive_radio_topics.Checked = false;
        }

        //
        private void archive_radio_topics_CheckedChanged(object sender, EventArgs e)
        {
            archive_radio_posts.Checked = false;
            archive_radio_pages.Checked = false;
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

                    int userId;

                    if (int.TryParse(textBoxUserId.Text, out userId))
                    {
                        string completeUrl = MESSAGE_URL + userId + '/';

                        //Start Task
                        Print(textBoxTop, "GETTING POST PAGE NUMBER 1");
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
                        Print(textBoxTop, "INVALID USER ID: " + textBoxUserId.Text + "");
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
                    var kek = docNode.QuerySelectorAll("span.lia-message-unread").Count();
                    postCount += docNode.QuerySelectorAll("span.lia-message-unread").Count() - 20;
                    Print(textBoxTop, "FINAL POST COUNT : " + postCount + "");

                    return;
                }

                //There is a next page, so if this is the first page
                else if(!Regex.Match(url, @"\d+$").Success)
                {
                    //HtmlAgilityPack seems to treat whitespace as children(?), so the logic here is a lot more silly than it should be.
                    var pagingList = docNode.QuerySelector("ul.lia-paging-full-pages").ChildNodes;
                    int.TryParse(pagingList[pagingList.Count - 2].ChildNodes[1].InnerHtml, out postCount);
                    postCount *= 20;
                    Print(textBoxTop, "POST ESTIMATE : ~" + postCount + "");
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

                    Print(textBoxTop, "GETTING POST PAGE NUMBER " + pageNumber + "");
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

            Print(textBoxBottom, "ACQUIRED POST OF ID: " + postId + "");

            //Node of post
            var post = docNode.QuerySelector("div.message-uid-" + postId);

            if (post == null)
            {
                Print(textBoxBottom, "POST OF ID: " + postId + " REQUIRES LOGIN. CANNOT GET.");
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

                string imageFileLocation = SAVE_IMAGE_LOCATION + System.IO.Path.GetFileName(src);

                image.SetAttributeValue("src", "../" + imageFileLocation);

                if (!File.Exists("output/" + imageFileLocation))
                {
                    #pragma warning disable 4014
                    Task.Run(() => HandleImageAsset(src)).ConfigureAwait(false);
                    #pragma warning restore 4014
                }
            }

            //Date of post
            var date = GetDateFromNode(post);

            //Title of post
            var title = docNode.QuerySelector("h3.custom-title").InnerHtml.Trim('"');

            //Subject of post (unused)
            //var subject = post.QuerySelector("div.lia-message-subject").ChildNodes[1].InnerHtml;

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

            postCounter++;

            if(postCounter >= postCount && postCount != 0)
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
                    using (FileStream lxFS = new FileStream("output/" + SAVE_IMAGE_LOCATION + System.IO.Path.GetFileName(url), FileMode.Create))
                    {
                        lxFS.Write(lnByte, 0, lnByte.Length);
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
    }
}
