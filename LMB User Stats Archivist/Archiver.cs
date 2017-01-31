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

namespace LMB_User_Stats_Archivist
{
    class Archiver
    {
        private const string PROFILEs_URL = "https://community.lego.com/t5/user/viewprofilepage/user-id/";

        private string saveLocation;

        private int start;

        private int end;

        private Form1 form;                                     //Form
        
        private TaskFactory taskFactory = new TaskFactory();    //Task Factory

        public Archiver(Form1 form, string saveLocation, int start, int end)
        {
            this.start = start;
            this.end = end;
            this.form = form;
        }

        //TAP Asynchronous Web Request for LMB Document
        private async Task<WebResponse> GetRequestStreamAsync(string url)
        {
            var request = (System.Net.HttpWebRequest)WebRequest.Create(url);
            request.CookieContainer = new CookieContainer();

            Task<WebResponse> task = taskFactory.FromAsync(request.BeginGetResponse, request.EndGetResponse, null);
            return await task;
        }

        private void tossRequests()
        {
            for(int i = start; i <= end; i++)
            {
                #pragma warning disable 4014
                Task.Run(() => userRequest(i)).ConfigureAwait(false);
                #pragma warning restore 4014
            }
        }

        private async void userRequest(int id)
        {
            var webResponse = await GetRequestStreamAsync(PROFILEs_URL + id);
            var doc = new HtmlAgilityPack.HtmlDocument();
        }
    }
}
