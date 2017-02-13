using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LMB_User_Stats_Archivist
{
    class Archiver
    {
        private const string BASE_URL = "https://community.lego.com/";

        private const string PROFILEs_URL = "https://community.lego.com/t5/user/viewprofilepage/user-id/";

        private const string ASSET_LOCATION = "assets/";

        private const string SAVE_IMAGE_LOCATION = "/images/";

        private const int simuLimit = 20;

        private int finalSimuLimit;

        private string saveLocation;

        private string saveSubLocation;

        private int start;

        private int counter;

        private int counterFinished;

        private int end;

        private Form1 form;                                     //Form

        private TaskFactory taskFactory = new TaskFactory();    //Task Factory

        private Object lockerImage = new Object();

        public Archiver(Form1 form, string saveLocation, int start, int end)
        {
            this.form = form;
            this.saveLocation = saveLocation;
            this.start = start;
            this.counter = start;
            this.counterFinished = start;
            this.end = end;

            if(this.end - this.start < simuLimit)
            {
                finalSimuLimit = this.end - this.start;
            }
            else
            {
                finalSimuLimit = simuLimit;
            }

            saveSubLocation = "/Users " + start + " to " + end + "/";
            Directory.CreateDirectory(saveLocation + saveSubLocation);
            Directory.CreateDirectory(saveLocation + SAVE_IMAGE_LOCATION);

            form.ExtractEmbeddedResources(saveLocation);
        }

        //TAP Asynchronous Web Request for LMB Document
        private async Task<WebResponse> GetRequestStreamAsync(string url)
        {
            var request = (System.Net.HttpWebRequest)WebRequest.Create(url);
            request.CookieContainer = new CookieContainer();

            Task<WebResponse> task = taskFactory.FromAsync(request.BeginGetResponse, request.EndGetResponse, null);
            return await task;
        }

        public void tossRequests()
        {
            form.Print("Starting task!");

            for (; counter <= start + finalSimuLimit; counter++)
            {
                var copyId = counter;
                #pragma warning disable 4014
                Task.Run(() => UserRequest(copyId)).ConfigureAwait(false);
                #pragma warning restore 4014
            }

            counter--;
        }

        private async void UserRequest(int id)
        {
            var doc = new HtmlAgilityPack.HtmlDocument();
            try
            {
                var webResponse = await GetRequestStreamAsync(PROFILEs_URL + id);
                doc.Load(webResponse.GetResponseStream());
                webResponse.Close();
            }
            catch
            {
                form.Print("Failed to get page for user : " + id + "; Retrying.");
                Thread.Sleep(5000);
                #pragma warning disable 4014
                Task.Run(() => UserRequest(id)).ConfigureAwait(false);
                #pragma warning restore 4014
                return;
            }

            var docNode = doc.DocumentNode; //Document Node

            if (docNode.QuerySelector(".lia-text error-description") == null)
            {
                var images = docNode.QuerySelectorAll("img");

                var username = docNode.QuerySelector(".UserName").QuerySelector("span").InnerHtml;

                //Get images and set the src for them, but not in that order.
                foreach (HtmlNode image in images)
                {
                    var src = image.GetAttributeValue("src", "");

                    string imageFileLocation = SAVE_IMAGE_LOCATION + Path.GetFileName(src);

                    image.SetAttributeValue("src", "../" + imageFileLocation);

                    if (!File.Exists(saveLocation + imageFileLocation))
                    {
                        #pragma warning disable 4014
                        Task.Run(() => HandleImageAsset(src)).ConfigureAwait(false);
                        #pragma warning restore 4014
                    }
                }

                var output = new HtmlAgilityPack.HtmlDocument();
                output.Load(Assembly.GetExecutingAssembly().GetManifestResourceStream("LMB_User_Stats_Archivist.assets.default.html"));
                AppendToNode(output.DocumentNode, docNode, ".container", "div.custom-profile");
                AppendToNode(output.DocumentNode, docNode, ".container", "h3.role-badges-header");
                AppendToNode(output.DocumentNode, docNode, ".container", "div.role-badges");
                output.Save(saveLocation + saveSubLocation + legalizeFilePath(id + "_" + username) + ".html");

                form.Print("User ID " + id + " saved!");
            }
            else
            {
                form.Print("User ID " + id + " does not exist!");
            }

            counterFinished++;
            counter++;

            if (counter <= end)
            {
                #pragma warning disable 4014
                Task.Run(() => UserRequest(counter)).ConfigureAwait(false);
                #pragma warning restore 4014
            }

            if (counterFinished > end)
            {
                form.Print("Task complete!");
                form.Enable();
            }
        }

        private void AppendToNode(HtmlNode target, HtmlNode apendee, string destinationSelector, string segmentSelector)
        {
            if(apendee.QuerySelector(segmentSelector) != null)
                target.QuerySelector(destinationSelector).AppendChild(apendee.QuerySelector(segmentSelector));
        }

        //Replace illegal characters in given file path
        private string legalizeFilePath(string filePath)
        {
            string regexSearch = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
            Regex r = new Regex(string.Format("[{0}]", Regex.Escape(regexSearch)));
            return r.Replace(filePath, "");
        }

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
                        using (FileStream lxFS = new FileStream(saveLocation + SAVE_IMAGE_LOCATION + Path.GetFileName(url), FileMode.Create))
                        {
                            lxFS.Write(lnByte, 0, lnByte.Length);
                            form.Print("Saving Image : " + Path.GetFileName(url));
                        }
                    }
                }
            }
            catch
            {
                form.Print("Failed to save Image : " + Path.GetFileName(url));
            }
        }
    }
}
