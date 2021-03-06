﻿using System;
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
using System.Text.RegularExpressions;
using System.IO;
using System.Reflection;

namespace LMB_User_Stats_Archivist
{
    public partial class Form1 : Form
    {
        //Archiver
        Archiver archiver;

        //
        public Form1()
        {
            InitializeComponent(); //Alcom ID = 349888
        }

        //Extract all embedded resources to the exe directory.
        public void ExtractEmbeddedResources(string location)
        {
            var executingAssembly = Assembly.GetExecutingAssembly();
            var resourceNames = executingAssembly.GetManifestResourceNames().ToList();

            resourceNames.RemoveRange(0, 2);

            foreach (var resourceName in resourceNames)
            {
                using (Stream stream = executingAssembly.GetManifestResourceStream(resourceName))
                {
                    if (resourceName.Contains("css"))
                    {
                        var pseduoName = resourceName.Replace('.', '\\');
                        var pseduoNameArray = pseduoName.ToCharArray();
                        pseduoNameArray[pseduoName.LastIndexOf('\\')] = '.';
                        pseduoName = new string(pseduoNameArray);
                        pseduoName = location + "\\" + pseduoName.Substring(pseduoName.IndexOf('\\') + 1);

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
        }

        //Start button
        private void button_archive_Click(object sender, EventArgs e)
        {
            archiver = new Archiver(this, textSaveFolder.Text, (int)numStart.Value, (int)numEnd.Value);
            archiver.tossRequests();
            this.Disable();
        }

        public void Print(string message)
        {
            //Thread safety
            if (this.InvokeRequired)
            {
                Invoke(new MethodInvoker(delegate () {
                    Print(message);
                }));
            }
            else
            {
                textBox.AppendText(message);
                textBox.AppendText(Environment.NewLine);
            }
        }

        private void numStart_ValueChanged(object sender, EventArgs e)
        {
            if (numStart.Value > numEnd.Value)
            {
                numStart.Value = numEnd.Value;
            }
        }

        private void numEnd_ValueChanged(object sender, EventArgs e)
        {
            if (numStart.Value > numEnd.Value)
            {
                numEnd.Value = numStart.Value;
            }
        }

        public void Disable()
        {
            //Thread safety
            if (this.InvokeRequired)
            {
                Invoke(new MethodInvoker(delegate () {
                    Disable();
                }));
            }
            else
            {
                this.panel_top.Enabled = false;
                this.button_archive.Enabled = false;
            }
        }

        public void Enable()
        {
            //Thread safety
            if (this.InvokeRequired)
            {
                Invoke(new MethodInvoker(delegate ()
                {
                    Enable();
                }));
            }
            else
            {
                this.panel_top.Enabled = true;
                this.button_archive.Enabled = true;
            }
        }
    }
}
