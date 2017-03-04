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
        TaskManager manager;

        //
        public Form1()
        {
            InitializeComponent(); //Alcom ID = 349888

            ExtractEmbeddedResources();

            manager = new TaskManager(this);

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
                        var fileStream = new FileStream(pseduoName, FileMode.CreateNew);
                        stream.CopyTo(fileStream);
                        fileStream.Flush();
                        stream.Close();
                        fileStream.Close();
                    }
                    catch
                    {
                        //File probably alread exists
                    }
                }
            }
        }

        //
        void Form1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }

        //
        void Form1_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

            if (files.Count() > 0)
            {
                string[] lines = File.ReadAllLines(files[0]);

                foreach (string line in lines)
                {
                    if (Regex.IsMatch(line, @"^\d+$"))
                    {
                        this.NewLine(TextBoxChoice.TextBoxTop);
                        this.Print(TextBoxChoice.TextBoxTop, "TASK ADDED : Archive User ID : " + line);
                        manager.AddTask(ArchiveOptionState.SaveUserPosts, line);
                        this.button_archive.Enabled = true;
                    }
                    else
                    {
                        var lineUrl = line;

                        if (lineUrl.StartsWith("community.lego.com/t5/"))
                        {
                            lineUrl = "https://" + line;
                        }
                        if (lineUrl.StartsWith("https://community.lego.com/t5/"))
                        {
                            this.NewLine(TextBoxChoice.TextBoxTop);
                            this.Print(TextBoxChoice.TextBoxTop, "TASK ADDED : Archive Topic : " + lineUrl);
                            manager.AddTask(ArchiveOptionState.SaveTopics, lineUrl);
                            this.button_archive.Enabled = true;
                        }
                        else
                        {
                            this.Print(TextBoxChoice.TextBoxTop, "INVALID TEXT FILE LINE: " + lineUrl);
                        }
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
            this.button_add.Enabled = true;
        }
        
        //Add Task button
        private void button_add_Click(object sender, EventArgs e)
        {
            switch (optionState)
            {
                case ArchiveOptionState.SaveUserPosts:
                    if (Regex.IsMatch(textBoxUserId.Text, @"^\d+$"))
                    {
                        this.NewLine(TextBoxChoice.TextBoxTop);
                        this.Print(TextBoxChoice.TextBoxTop, "TASK ADDED : Archive User ID : " + textBoxUserId.Text);
                        manager.AddTask(optionState, textBoxUserId.Text);
                        this.button_archive.Enabled = true;
                    }
                    else
                    {
                        this.Print(TextBoxChoice.TextBoxTop, "Invalid User ID for task: " + textBoxUserId.Text);
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
                    if (url.StartsWith("community.lego.com/t5/"))
                    {
                        url = "https://" + url;
                    }
                    if (url.StartsWith("https://community.lego.com/t5/"))
                    {
                        this.NewLine(TextBoxChoice.TextBoxTop);
                        this.Print(TextBoxChoice.TextBoxTop, "TASK ADDED : Archive Topic : " + url);
                        manager.AddTask(optionState, url);
                        this.button_archive.Enabled = true;
                    }
                    else
                    {
                        this.Print(TextBoxChoice.TextBoxTop, "INVALID URL: " + url);
                    }
                    break;
            }
        }

        //Start button
        private void button_archive_Click(object sender, EventArgs e)
        {
            this.button_archive.Enabled = false;
            manager.Start();
        }

        //Enable the start button
        public void EnableStartButton()
        {
            //Thread safety
            if (this.InvokeRequired)
            {
                Invoke(new MethodInvoker(delegate () {
                    EnableStartButton();
                }));
            }
            else
            {
                this.button_archive.Enabled = true;
            }
        }

        //Create a new link in a text box
        public void NewLine(TextBoxChoice choice)
        {
            switch(choice)
            {
                case TextBoxChoice.TextBoxTop:
                    if(!String.IsNullOrEmpty(this.textBoxTop.Text))
                    {
                        Print(choice, "");
                    }
                    break;
                case TextBoxChoice.TextBoxBottom:
                    if (!String.IsNullOrEmpty(this.textBoxBottom.Text))
                    {
                        Print(choice, "");
                    }
                    break;
            }
        }

        //Print text in a provided text box.
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
