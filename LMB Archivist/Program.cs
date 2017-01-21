﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using HtmlAgilityPack;
using Fizzler.Systems.HtmlAgilityPack;
using System.Text.RegularExpressions;
using System.IO;

namespace LMB_Archivist
{
    public class LMBArchiver
    {

    }

    public static class Program
    {
        //Base LMB URL
        private const string baseUrl = "https://community.lego.com/";

        //URL For LMB user's posts
        private const string messageUrl = "https://community.lego.com/t5/forums/recentpostspage/post-type/message/user-id/";

        //Save location
        private static string saveLocation = "output/";

        //Task Factory
        private static TaskFactory taskFactory;

        //Main
        public static void Main(string[] args)
        {
            //Task Factory
            taskFactory = new TaskFactory();

            //User Id and complete URL
            int userId = 349888;
            string completeUrl = messageUrl + userId + '/';

            //First Task
            Task task = LMBTaskAsync(completeUrl, HandlePostListDocument);
            task.Wait();

            //Press key to stop
            Console.ReadKey();
        }

        //TAP Asynchronous Web Request for LMB Document
        private static Task<WebResponse> GetRequestStreamAsync(this WebRequest request)
        {
            return taskFactory.FromAsync(request.BeginGetResponse, request.EndGetResponse, null);
        }

        //General LMB Request Task
        private static async Task LMBTaskAsync(string url, Action<HtmlNode, string> action)
        {

            var request = (System.Net.HttpWebRequest)WebRequest.Create(url);
            request.CookieContainer = new CookieContainer();

            Task<WebResponse> task = GetRequestStreamAsync(request);

            //Read the document from the request result
            var doc = new HtmlDocument();   //Document
            WebResponse webResponse = await task;
            doc.Load(webResponse.GetResponseStream());
            webResponse.Close();

            var docNode = doc.DocumentNode; //Document Node

            action(docNode, url);
        }

        //Handle post-list page from web request.
        private static async void HandlePostListDocument(HtmlNode docNode, string url)
        {
            //Set Save Location based on username
            saveLocation = "output/" + docNode.QuerySelector("a.lia-user-name-link").FirstChild.InnerHtml + "/";
            Directory.CreateDirectory(saveLocation);

            //Start web requests for each post on the list
            foreach (var item in docNode.QuerySelectorAll("span.lia-message-unread"))
            {
                var itemAnchor = item.QuerySelectorAll("a").ToList()[0];

                Task taskPost = LMBTaskAsync(baseUrl + itemAnchor.GetAttributeValue("href", ""), HandlePostDocument);
            }

            //Node containing link for the next post-list
            var next = docNode.QuerySelector("li.lia-paging-page-next").QuerySelector("a.lia-link-navigation");

            //Return if no list page after this one
            if (next == null)
            {
                return;
            }

            //Page number stuff
            var cssClasses = next.GetAttributeValue("class", "").Split(' ');
            foreach (var cssClass in cssClasses)
            {
                if (cssClass.StartsWith("lia-js-data-pageNum-"))
                {
                    int pageNumber = 0;
                    Int32.TryParse(cssClass[cssClass.Length - 1].ToString(), out pageNumber);

                    Console.WriteLine("---GETTING POST PAGE NUMBER " + pageNumber + "---\n");
                }
            }

            //Get the next list page
            Task taskNextPage = LMBTaskAsync(next.GetAttributeValue("href", ""), HandlePostListDocument);
        }

        //Handle post page from web request
        private static void HandlePostDocument(HtmlNode docNode, string url)
        {
            var postId = Regex.Match(url, @"m-p\/\d+").Value.Replace("m-p/", "");

            Console.WriteLine("---GETTING POST OF ID: " + postId + "---\n");

            //Node of post
            var post = docNode.QuerySelector("div.message-uid-" + postId);

            //Modify post document for exporting
            post.QuerySelector("div.lia-quilt-column-main-right")
                .QuerySelector("div.lia-quilt-column-alley")
                .Attributes["class"].Value = "lia-quilt-column-alley lia-quilt-column-alley-left";

            //Date of post
            var date = GetDateFromNode(post);

            //Title of post
            var title = docNode.QuerySelector("h3.custom-title").InnerHtml.Trim('"');

            //Subject of post
            var subject = post.QuerySelector("div.lia-message-subject").FirstChild.InnerHtml;

            //Save post document
            var output = new HtmlDocument();
            output.Load("assets/default.html");
            output.DocumentNode.QuerySelector("div.even-row").AppendChild(post);
            output.Save(saveLocation + legalizeFilePath(date + "_" + postId + "_" + title + "_" + subject) + ".html");
        }

        //Replace illegal characters in given file path
        private static string legalizeFilePath(string filePath)
        {
            string regexSearch = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
            Regex r = new Regex(string.Format("[{0}]", Regex.Escape(regexSearch)));
            return r.Replace(filePath, "");
        }

        //Get the date from an LMB HtmlNode
        private static string GetDateFromNode(HtmlNode docNode)
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
            if(date != String.Empty)
            {
                date = DateTime.Parse(date).ToString("yyyy-mm-dd hh\uA789mm");
            }
            else
            {
                date = "no-date";
            }

            return date;
        }
    }
}
