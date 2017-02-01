namespace LMB_User_Stats_Archivist
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
            this.archive_post_panel = new System.Windows.Forms.Panel();
            this.numEnd = new System.Windows.Forms.NumericUpDown();
            this.numStart = new System.Windows.Forms.NumericUpDown();
            this.textSaveFolder = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.textBox = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.archive_post_panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numEnd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numStart)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_archive
            // 
            this.button_archive.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.button_archive.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_archive.Location = new System.Drawing.Point(130, 84);
            this.button_archive.Name = "button_archive";
            this.button_archive.Size = new System.Drawing.Size(243, 48);
            this.button_archive.TabIndex = 0;
            this.button_archive.Text = "START ARCHIVE!";
            this.button_archive.UseVisualStyleBackColor = true;
            this.button_archive.Click += new System.EventHandler(this.button_archive_Click);
            // 
            // archive_post_panel
            // 
            this.archive_post_panel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.archive_post_panel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.archive_post_panel.Controls.Add(this.numEnd);
            this.archive_post_panel.Controls.Add(this.numStart);
            this.archive_post_panel.Controls.Add(this.textSaveFolder);
            this.archive_post_panel.Controls.Add(this.label3);
            this.archive_post_panel.Controls.Add(this.label2);
            this.archive_post_panel.Controls.Add(this.label1);
            this.archive_post_panel.Location = new System.Drawing.Point(12, 13);
            this.archive_post_panel.Name = "archive_post_panel";
            this.archive_post_panel.Size = new System.Drawing.Size(485, 58);
            this.archive_post_panel.TabIndex = 10;
            // 
            // numEnd
            // 
            this.numEnd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numEnd.Location = new System.Drawing.Point(382, 5);
            this.numEnd.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.numEnd.Name = "numEnd";
            this.numEnd.Size = new System.Drawing.Size(90, 20);
            this.numEnd.TabIndex = 21;
            this.numEnd.Value = new decimal(new int[] {
            3000000,
            0,
            0,
            0});
            // 
            // numStart
            // 
            this.numStart.Location = new System.Drawing.Point(93, 5);
            this.numStart.Maximum = new decimal(new int[] {
            3000000,
            0,
            0,
            0});
            this.numStart.Name = "numStart";
            this.numStart.Size = new System.Drawing.Size(90, 20);
            this.numStart.TabIndex = 19;
            // 
            // textSaveFolder
            // 
            this.textSaveFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textSaveFolder.Location = new System.Drawing.Point(93, 31);
            this.textSaveFolder.Name = "textSaveFolder";
            this.textSaveFolder.Size = new System.Drawing.Size(379, 20);
            this.textSaveFolder.TabIndex = 20;
            this.textSaveFolder.Text = "C:\\";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(5, 34);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(87, 13);
            this.label3.TabIndex = 19;
            this.label3.Text = "Save Folder : ";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(305, 7);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 13);
            this.label2.TabIndex = 18;
            this.label2.Text = "Ending ID :";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 13);
            this.label1.TabIndex = 16;
            this.label1.Text = "Starting ID :";
            // 
            // toolTip1
            // 
            this.toolTip1.AutomaticDelay = 50;
            this.toolTip1.AutoPopDelay = 5000;
            this.toolTip1.InitialDelay = 50;
            this.toolTip1.ReshowDelay = 10;
            this.toolTip1.ShowAlways = true;
            // 
            // textBox
            // 
            this.textBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox.Location = new System.Drawing.Point(3, 3);
            this.textBox.Multiline = true;
            this.textBox.Name = "textBox";
            this.textBox.ReadOnly = true;
            this.textBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox.Size = new System.Drawing.Size(479, 222);
            this.textBox.TabIndex = 8;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Enabled = false;
            this.panel1.Location = new System.Drawing.Point(0, 146);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(11, 0, 11, 11);
            this.panel1.Size = new System.Drawing.Size(509, 241);
            this.panel1.TabIndex = 18;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.textBox);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Enabled = false;
            this.panel2.Location = new System.Drawing.Point(11, 0);
            this.panel2.Margin = new System.Windows.Forms.Padding(20);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(487, 230);
            this.panel2.TabIndex = 19;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(509, 387);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.archive_post_panel);
            this.Controls.Add(this.button_archive);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(425, 300);
            this.Name = "Form1";
            this.Text = "LEGO Message Boards Archivist";
            this.archive_post_panel.ResumeLayout(false);
            this.archive_post_panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numEnd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numStart)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        internal System.Windows.Forms.Button button_archive;
        internal System.Windows.Forms.Panel archive_post_panel;
        internal System.Windows.Forms.Label label1;
        internal System.Windows.Forms.ToolTip toolTip1;
        internal System.Windows.Forms.TextBox textBox;
        internal System.Windows.Forms.Panel panel1;
        internal System.Windows.Forms.Panel panel2;
        internal System.Windows.Forms.Label label2;
        internal System.Windows.Forms.TextBox textSaveFolder;
        internal System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numEnd;
        private System.Windows.Forms.NumericUpDown numStart;
    }
}



