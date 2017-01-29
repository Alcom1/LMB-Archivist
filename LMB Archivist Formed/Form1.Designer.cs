namespace LMB_Archivist_Formed
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.button_archive = new System.Windows.Forms.Button();
            this.textBoxBottom = new System.Windows.Forms.TextBox();
            this.textBoxTop = new System.Windows.Forms.TextBox();
            this.archive_post_panel = new System.Windows.Forms.Panel();
            this.archive_radio_topics = new System.Windows.Forms.RadioButton();
            this.archive_radio_pages = new System.Windows.Forms.RadioButton();
            this.archive_radio_posts = new System.Windows.Forms.RadioButton();
            this.tooltip = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxUserId = new System.Windows.Forms.TextBox();
            this.archive_topic_panel = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxUrl = new System.Windows.Forms.TextBox();
            this.archive_post_radio = new System.Windows.Forms.RadioButton();
            this.archive_topic_radio = new System.Windows.Forms.RadioButton();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.archive_post_panel.SuspendLayout();
            this.archive_topic_panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_archive
            // 
            this.button_archive.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.button_archive.Enabled = false;
            this.button_archive.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_archive.Location = new System.Drawing.Point(130, 187);
            this.button_archive.Name = "button_archive";
            this.button_archive.Size = new System.Drawing.Size(243, 48);
            this.button_archive.TabIndex = 0;
            this.button_archive.Text = "START ARCHIVE!";
            this.button_archive.UseVisualStyleBackColor = true;
            this.button_archive.Click += new System.EventHandler(this.button_archive_Click);
            // 
            // textBoxBottom
            // 
            this.textBoxBottom.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxBottom.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxBottom.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.textBoxBottom.Location = new System.Drawing.Point(3, 3);
            this.textBoxBottom.Multiline = true;
            this.textBoxBottom.Name = "textBoxBottom";
            this.textBoxBottom.ReadOnly = true;
            this.textBoxBottom.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxBottom.Size = new System.Drawing.Size(477, 104);
            this.textBoxBottom.TabIndex = 5;
            // 
            // textBoxTop
            // 
            this.textBoxTop.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxTop.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxTop.Location = new System.Drawing.Point(3, 3);
            this.textBoxTop.Multiline = true;
            this.textBoxTop.Name = "textBoxTop";
            this.textBoxTop.ReadOnly = true;
            this.textBoxTop.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxTop.Size = new System.Drawing.Size(477, 110);
            this.textBoxTop.TabIndex = 8;
            // 
            // archive_post_panel
            // 
            this.archive_post_panel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.archive_post_panel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.archive_post_panel.Controls.Add(this.archive_radio_topics);
            this.archive_post_panel.Controls.Add(this.archive_radio_pages);
            this.archive_post_panel.Controls.Add(this.archive_radio_posts);
            this.archive_post_panel.Controls.Add(this.tooltip);
            this.archive_post_panel.Controls.Add(this.label1);
            this.archive_post_panel.Controls.Add(this.textBoxUserId);
            this.archive_post_panel.Enabled = false;
            this.archive_post_panel.Location = new System.Drawing.Point(13, 38);
            this.archive_post_panel.Name = "archive_post_panel";
            this.archive_post_panel.Size = new System.Drawing.Size(485, 55);
            this.archive_post_panel.TabIndex = 10;
            // 
            // archive_radio_topics
            // 
            this.archive_radio_topics.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.archive_radio_topics.AutoSize = true;
            this.archive_radio_topics.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.archive_radio_topics.Location = new System.Drawing.Point(327, 31);
            this.archive_radio_topics.Margin = new System.Windows.Forms.Padding(6);
            this.archive_radio_topics.Name = "archive_radio_topics";
            this.archive_radio_topics.Size = new System.Drawing.Size(147, 17);
            this.archive_radio_topics.TabIndex = 19;
            this.archive_radio_topics.Text = "Archive Entire Topics";
            this.archive_radio_topics.UseVisualStyleBackColor = true;
            this.archive_radio_topics.CheckedChanged += new System.EventHandler(this.archive_radio_topics_CheckedChanged);
            // 
            // archive_radio_pages
            // 
            this.archive_radio_pages.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.archive_radio_pages.AutoSize = true;
            this.archive_radio_pages.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.archive_radio_pages.Location = new System.Drawing.Point(170, 31);
            this.archive_radio_pages.Margin = new System.Windows.Forms.Padding(6);
            this.archive_radio_pages.Name = "archive_radio_pages";
            this.archive_radio_pages.Size = new System.Drawing.Size(147, 17);
            this.archive_radio_pages.TabIndex = 18;
            this.archive_radio_pages.Text = "Archive Whole Pages";
            this.archive_radio_pages.UseVisualStyleBackColor = true;
            this.archive_radio_pages.CheckedChanged += new System.EventHandler(this.archive_radio_pages_CheckedChanged);
            // 
            // archive_radio_posts
            // 
            this.archive_radio_posts.AutoSize = true;
            this.archive_radio_posts.Checked = true;
            this.archive_radio_posts.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.archive_radio_posts.Location = new System.Drawing.Point(6, 31);
            this.archive_radio_posts.Margin = new System.Windows.Forms.Padding(6);
            this.archive_radio_posts.Name = "archive_radio_posts";
            this.archive_radio_posts.Size = new System.Drawing.Size(155, 17);
            this.archive_radio_posts.TabIndex = 15;
            this.archive_radio_posts.TabStop = true;
            this.archive_radio_posts.Text = "Archive Individal Posts";
            this.archive_radio_posts.UseVisualStyleBackColor = true;
            this.archive_radio_posts.CheckedChanged += new System.EventHandler(this.archive_radio_posts_CheckedChanged);
            // 
            // tooltip
            // 
            this.tooltip.AutoSize = true;
            this.tooltip.BackColor = System.Drawing.SystemColors.Control;
            this.tooltip.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tooltip.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tooltip.Location = new System.Drawing.Point(144, 7);
            this.tooltip.Name = "tooltip";
            this.tooltip.Size = new System.Drawing.Size(16, 15);
            this.tooltip.TabIndex = 17;
            this.tooltip.Text = "?";
            this.toolTip1.SetToolTip(this.tooltip, "This is the 6-7 digit number associated with a user, found in the url of their pr" +
        "ofile.");
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 16;
            this.label1.Text = "User ID :";
            // 
            // textBoxUserId
            // 
            this.textBoxUserId.Location = new System.Drawing.Point(64, 5);
            this.textBoxUserId.Name = "textBoxUserId";
            this.textBoxUserId.Size = new System.Drawing.Size(74, 20);
            this.textBoxUserId.TabIndex = 15;
            this.textBoxUserId.Text = "0";
            // 
            // archive_topic_panel
            // 
            this.archive_topic_panel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.archive_topic_panel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.archive_topic_panel.Controls.Add(this.label2);
            this.archive_topic_panel.Controls.Add(this.label3);
            this.archive_topic_panel.Controls.Add(this.textBoxUrl);
            this.archive_topic_panel.Enabled = false;
            this.archive_topic_panel.Location = new System.Drawing.Point(12, 138);
            this.archive_topic_panel.Name = "archive_topic_panel";
            this.archive_topic_panel.Size = new System.Drawing.Size(486, 30);
            this.archive_topic_panel.TabIndex = 11;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.SystemColors.Control;
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(463, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(16, 15);
            this.label2.TabIndex = 20;
            this.label2.Text = "?";
            this.toolTip1.SetToolTip(this.label2, "The full url of the topic or a post on it.");
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(3, 6);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 13);
            this.label3.TabIndex = 19;
            this.label3.Text = "Topic URL : ";
            // 
            // textBoxUrl
            // 
            this.textBoxUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxUrl.Location = new System.Drawing.Point(84, 4);
            this.textBoxUrl.Name = "textBoxUrl";
            this.textBoxUrl.Size = new System.Drawing.Size(373, 20);
            this.textBoxUrl.TabIndex = 18;
            // 
            // archive_post_radio
            // 
            this.archive_post_radio.AutoSize = true;
            this.archive_post_radio.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.archive_post_radio.Location = new System.Drawing.Point(16, 12);
            this.archive_post_radio.Name = "archive_post_radio";
            this.archive_post_radio.Size = new System.Drawing.Size(158, 20);
            this.archive_post_radio.TabIndex = 13;
            this.archive_post_radio.Text = "Archive User Posts";
            this.archive_post_radio.UseVisualStyleBackColor = true;
            this.archive_post_radio.CheckedChanged += new System.EventHandler(this.archive_post_radio_CheckedChanged);
            // 
            // archive_topic_radio
            // 
            this.archive_topic_radio.AutoSize = true;
            this.archive_topic_radio.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.archive_topic_radio.Location = new System.Drawing.Point(16, 111);
            this.archive_topic_radio.Name = "archive_topic_radio";
            this.archive_topic_radio.Size = new System.Drawing.Size(122, 20);
            this.archive_topic_radio.TabIndex = 14;
            this.archive_topic_radio.Text = "Archive Topic";
            this.archive_topic_radio.UseVisualStyleBackColor = true;
            this.archive_topic_radio.CheckedChanged += new System.EventHandler(this.archive_topic_radio_CheckedChanged);
            // 
            // toolTip1
            // 
            this.toolTip1.AutomaticDelay = 50;
            this.toolTip1.AutoPopDelay = 5000;
            this.toolTip1.InitialDelay = 50;
            this.toolTip1.ReshowDelay = 10;
            this.toolTip1.ShowAlways = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer1.Location = new System.Drawing.Point(13, 254);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.textBoxTop);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.textBoxBottom);
            this.splitContainer1.Size = new System.Drawing.Size(485, 244);
            this.splitContainer1.SplitterDistance = 118;
            this.splitContainer1.SplitterWidth = 9;
            this.splitContainer1.TabIndex = 15;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(509, 512);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.archive_topic_radio);
            this.Controls.Add(this.archive_post_radio);
            this.Controls.Add(this.archive_topic_panel);
            this.Controls.Add(this.archive_post_panel);
            this.Controls.Add(this.button_archive);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(525, 475);
            this.Name = "Form1";
            this.Text = "LEGO Message Boards Archiver";
            this.archive_post_panel.ResumeLayout(false);
            this.archive_post_panel.PerformLayout();
            this.archive_topic_panel.ResumeLayout(false);
            this.archive_topic_panel.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        internal System.Windows.Forms.Button button_archive;
        internal System.Windows.Forms.TextBox textBoxBottom;
        internal System.Windows.Forms.TextBox textBoxTop;
        internal System.Windows.Forms.Panel archive_post_panel;
        internal System.Windows.Forms.Panel archive_topic_panel;
        internal System.Windows.Forms.RadioButton archive_post_radio;
        internal System.Windows.Forms.RadioButton archive_topic_radio;
        internal System.Windows.Forms.Label label1;
        internal System.Windows.Forms.TextBox textBoxUserId;
        internal System.Windows.Forms.ToolTip toolTip1;
        internal System.Windows.Forms.Label tooltip;
        internal System.Windows.Forms.RadioButton archive_radio_posts;
        internal System.Windows.Forms.RadioButton archive_radio_topics;
        internal System.Windows.Forms.RadioButton archive_radio_pages;
        internal System.Windows.Forms.Label label2;
        internal System.Windows.Forms.Label label3;
        internal System.Windows.Forms.TextBox textBoxUrl;
        internal System.Windows.Forms.SplitContainer splitContainer1;
    }
}

