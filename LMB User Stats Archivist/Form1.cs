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

            ExtractEmbeddedResources();
        }

        //Extract all embedded resources to the exe directory.
        public void ExtractEmbeddedResources()
        {
            var executingAssembly = Assembly.GetExecutingAssembly();
            var resourceNames = executingAssembly.GetManifestResourceNames().ToList();

            resourceNames.RemoveRange(0, 2);

            foreach (var resourceName in resourceNames)
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

        //Start button
        private void button_archive_Click(object sender, EventArgs e)
        {
            archiver = new Archiver(this, textSaveFolder.Text, (int)numStart.Value, (int)numEnd.Value);
            archiver.tossRequests();
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
    }
}
