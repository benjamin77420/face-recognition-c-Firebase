
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
            this.worker_id_text = new System.Windows.Forms.MaskedTextBox();
            this.name_text = new System.Windows.Forms.TextBox();
            this.worker_ID_lable = new System.Windows.Forms.Label();
            this.full_name_lable = new System.Windows.Forms.Label();
            this.video_feed = new System.Windows.Forms.PictureBox();
            this.detected_feed = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.video_feed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.detected_feed)).BeginInit();
            this.SuspendLayout();
            // 
            // worker_id_text
            // 
            this.worker_id_text.Location = new System.Drawing.Point(605, 61);
            this.worker_id_text.Name = "worker_id_text";
            this.worker_id_text.ReadOnly = true;
            this.worker_id_text.Size = new System.Drawing.Size(183, 20);
            this.worker_id_text.TabIndex = 1;
            // 
            // name_text
            // 
            this.name_text.Location = new System.Drawing.Point(605, 139);
            this.name_text.Name = "name_text";
            this.name_text.ReadOnly = true;
            this.name_text.Size = new System.Drawing.Size(183, 20);
            this.name_text.TabIndex = 2;
            // 
            // worker_ID_lable
            // 
            this.worker_ID_lable.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.worker_ID_lable.Location = new System.Drawing.Point(605, 23);
            this.worker_ID_lable.Name = "worker_ID_lable";
            this.worker_ID_lable.Size = new System.Drawing.Size(105, 23);
            this.worker_ID_lable.TabIndex = 8;
            this.worker_ID_lable.Text = "Worker id :";
            // 
            // full_name_lable
            // 
            this.full_name_lable.AutoSize = true;
            this.full_name_lable.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.full_name_lable.Location = new System.Drawing.Point(602, 100);
            this.full_name_lable.Name = "full_name_lable";
            this.full_name_lable.Size = new System.Drawing.Size(71, 24);
            this.full_name_lable.TabIndex = 4;
            this.full_name_lable.Text = "Name :";
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
            this.Controls.Add(this.full_name_lable);
            this.Controls.Add(this.worker_ID_lable);
            this.Controls.Add(this.name_text);
            this.Controls.Add(this.worker_id_text);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.video_feed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.detected_feed)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MaskedTextBox worker_id_text;
        private System.Windows.Forms.TextBox name_text;
        private System.Windows.Forms.Label worker_ID_lable;
        private System.Windows.Forms.Label full_name_lable;
        private System.Windows.Forms.PictureBox video_feed;
        private System.Windows.Forms.PictureBox detected_feed;
    }
}

