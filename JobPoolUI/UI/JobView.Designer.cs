namespace JobPoolUI.UI
{
    partial class JobView
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.NameL = new System.Windows.Forms.Label();
            this.DetailsL = new System.Windows.Forms.Label();
            this.StatusPB = new System.Windows.Forms.PictureBox();
            this.ProgressPrB = new System.Windows.Forms.ProgressBar();
            ((System.ComponentModel.ISupportInitialize)(this.StatusPB)).BeginInit();
            this.SuspendLayout();
            // 
            // NameL
            // 
            this.NameL.AutoSize = true;
            this.NameL.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NameL.Location = new System.Drawing.Point(56, 6);
            this.NameL.Name = "NameL";
            this.NameL.Size = new System.Drawing.Size(84, 21);
            this.NameL.TabIndex = 0;
            this.NameL.Text = "Job name";
            this.NameL.MouseEnter += new System.EventHandler(this.JobView_MouseEnter);
            this.NameL.MouseLeave += new System.EventHandler(this.JobView_MouseLeave);
            // 
            // DetailsL
            // 
            this.DetailsL.AutoSize = true;
            this.DetailsL.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DetailsL.Location = new System.Drawing.Point(57, 27);
            this.DetailsL.Name = "DetailsL";
            this.DetailsL.Size = new System.Drawing.Size(224, 15);
            this.DetailsL.TabIndex = 1;
            this.DetailsL.Text = "Idle for 00:00:00 / ran 1 time(s) / no errors";
            this.DetailsL.MouseEnter += new System.EventHandler(this.JobView_MouseEnter);
            this.DetailsL.MouseLeave += new System.EventHandler(this.JobView_MouseLeave);
            // 
            // StatusPB
            // 
            this.StatusPB.Image = global::JobPoolUI.Properties.Resources.waiting;
            this.StatusPB.Location = new System.Drawing.Point(6, 6);
            this.StatusPB.Name = "StatusPB";
            this.StatusPB.Size = new System.Drawing.Size(48, 48);
            this.StatusPB.TabIndex = 2;
            this.StatusPB.TabStop = false;
            this.StatusPB.MouseEnter += new System.EventHandler(this.JobView_MouseEnter);
            this.StatusPB.MouseLeave += new System.EventHandler(this.JobView_MouseLeave);
            // 
            // ProgressPrB
            // 
            this.ProgressPrB.Location = new System.Drawing.Point(60, 44);
            this.ProgressPrB.Name = "ProgressPrB";
            this.ProgressPrB.Size = new System.Drawing.Size(334, 10);
            this.ProgressPrB.TabIndex = 3;
            this.ProgressPrB.MouseEnter += new System.EventHandler(this.JobView_MouseEnter);
            this.ProgressPrB.MouseLeave += new System.EventHandler(this.JobView_MouseLeave);
            // 
            // JobView
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.ProgressPrB);
            this.Controls.Add(this.StatusPB);
            this.Controls.Add(this.DetailsL);
            this.Controls.Add(this.NameL);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "JobView";
            this.Size = new System.Drawing.Size(400, 60);
            this.Load += new System.EventHandler(this.JobView_Load);
            this.MouseEnter += new System.EventHandler(this.JobView_MouseEnter);
            this.MouseLeave += new System.EventHandler(this.JobView_MouseLeave);
            ((System.ComponentModel.ISupportInitialize)(this.StatusPB)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label NameL;
        private System.Windows.Forms.Label DetailsL;
        private System.Windows.Forms.PictureBox StatusPB;
        private System.Windows.Forms.ProgressBar ProgressPrB;
    }
}
