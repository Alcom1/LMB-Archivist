using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LMB_Archivist_Formed
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void archive_post_radio_CheckedChanged(object sender, EventArgs e)
        {
            changeArchiveMode();
        }

        private void archive_topic_radio_CheckedChanged(object sender, EventArgs e)
        {
            changeArchiveMode();
        }

        private void changeArchiveMode()
        {
            this.archive_post_panel.Enabled = this.archive_post_radio.Checked;
            this.archive_topic_panel.Enabled = this.archive_topic_radio.Checked;

            this.button_archive.Enabled = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
