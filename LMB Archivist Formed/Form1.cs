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
    public enum ArchiveButtonState
    {
        Stopped,
        AreSure,
        Running,
    }

    public enum ArchiveOptionState
    {
        None,
        SaveUserPosts,
        SaveUserPages,
        SaveUserTopics,
        SaveTopics
    }

    public enum TextBoxChoice
    {
        TextBoxTop,
        TextBoxBottom
    }

    public partial class Form1 : Form
    {

        //Enums
        private ArchiveButtonState buttonState;
        private ArchiveOptionState optionState;

        //Archiver
        Archiver archiver;

        //
        public Form1()
        {
            InitializeComponent(); //Alcom ID = 349888

            ExtractEmbeddedResources();

            archiver = new Archiver(this);

            buttonState = ArchiveButtonState.Stopped;
        }

        //Extract all embedded resources to the exe directory.
        public void ExtractEmbeddedResources()
        {
            var executingAssembly = Assembly.GetExecutingAssembly();
            var resourceNames = executingAssembly.GetManifestResourceNames().ToList();

            resourceNames.RemoveRange(0, 2);

            foreach(var resourceName in resourceNames)
            {
                using (Stream stream = executingAssembly.GetManifestResourceStream(resourceName))
                {
                    var pseduoName = resourceName.Replace('.', '\\');
                    var pseduoNameArray = pseduoName.ToCharArray();
                    pseduoNameArray[pseduoName.LastIndexOf('\\')] = '.';
                    pseduoName = new string(pseduoNameArray);
                    pseduoName = pseduoName.Substring(pseduoName.IndexOf('\\') + 1);

                    new FileInfo(pseduoName).Directory.Create();
                    try
                    {
                        stream.CopyTo(new FileStream(pseduoName, FileMode.CreateNew));
                    }
                    catch
                    {
                        //File probably alread exists
                    }
                }
            }
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
                                archiver.StartUserPostArchiving(userId);

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
                                this.Print(TextBoxChoice.TextBoxTop, "INVALID USER ID: " + textBoxUserId.Text);
                            }
                            break;

                        case ArchiveOptionState.SaveUserPages:
                            this.Print(TextBoxChoice.TextBoxTop, "UNIMPLEMENTED FEATURE : ARCHIVE USER PAGES");
                            break;

                        case ArchiveOptionState.SaveUserTopics:
                            this.Print(TextBoxChoice.TextBoxTop, "UNIMPLEMENTED FEATURE : ARCHIVE USER TOPICS");
                            break;

                        case ArchiveOptionState.SaveTopics:

                            string url = textBoxUrl.Text;
                            if(url.StartsWith("community.lego.com/t5/"))
                            {
                                url = "https://" + url;
                            }
                            if (url.StartsWith("https://community.lego.com/t5/"))
                            {
                                archiver.StartTopicArchiving(url);

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
                                this.Print(TextBoxChoice.TextBoxTop, "INVALID URL: " + url);
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
        public void ResetButtonToRunning()
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
        public void ResetButtonToStopped()
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
        public void SetFinished()
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="choice"></param>
        /// <param name="message"></param>
        public void Print(TextBoxChoice choice, string message)
        {
            //Thread safety
            if (this.InvokeRequired)
            {
                Invoke(new MethodInvoker(delegate () {
                    Print(choice, message);
                }));
            }
            else
            {
                switch(choice)
                {
                    case TextBoxChoice.TextBoxTop:
                        textBoxTop.AppendText(message);
                        textBoxTop.AppendText(Environment.NewLine);
                        break;
                    case TextBoxChoice.TextBoxBottom:
                        textBoxBottom.AppendText(message);
                        textBoxBottom.AppendText(Environment.NewLine);
                        break;
                }
            }
        }
    }
}
