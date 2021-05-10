
namespace openCVWork_with_files_
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
            this.video_feed = new System.Windows.Forms.PictureBox();
            this.detected_feed = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.video_feed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.detected_feed)).BeginInit();
            this.SuspendLayout();
            // 
            // video_feed
            // 
            this.video_feed.Location = new System.Drawing.Point(12, 12);
            this.video_feed.Name = "video_feed";
            this.video_feed.Size = new System.Drawing.Size(568, 374);
            this.video_feed.TabIndex = 9;
            this.video_feed.TabStop = false;
            // 
            // detected_feed
            // 
            this.detected_feed.Location = new System.Drawing.Point(586, 206);
            this.detected_feed.Name = "detected_feed";
            this.detected_feed.Size = new System.Drawing.Size(202, 180);
            this.detected_feed.TabIndex = 10;
            this.detected_feed.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.detected_feed);
            this.Controls.Add(this.video_feed);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.video_feed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.detected_feed)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.PictureBox video_feed;
        private System.Windows.Forms.PictureBox detected_feed;
    }
}

