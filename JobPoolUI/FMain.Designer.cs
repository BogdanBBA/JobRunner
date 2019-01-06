namespace JobPoolUI
{
    partial class FMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FMain));
            this.WorkerBgW = new System.ComponentModel.BackgroundWorker();
            this.CheckTimer = new System.Windows.Forms.Timer(this.components);
            this.InitialTimer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // WorkerBgW
            // 
            this.WorkerBgW.DoWork += new System.ComponentModel.DoWorkEventHandler(this.WorkerBgW_DoWork);
            this.WorkerBgW.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.WorkerBgW_RunWorkerCompleted);
            // 
            // CheckTimer
            // 
            this.CheckTimer.Interval = 1000;
            this.CheckTimer.Tick += new System.EventHandler(this.CheckTimer_Tick);
            // 
            // InitialTimer
            // 
            this.InitialTimer.Interval = 5000;
            this.InitialTimer.Tick += new System.EventHandler(this.InitialTimer_Tick);
            // 
            // FMain
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(384, 361);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Job runner";
            this.Load += new System.EventHandler(this.FMain_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.ComponentModel.BackgroundWorker WorkerBgW;
        private System.Windows.Forms.Timer CheckTimer;
        private System.Windows.Forms.Timer InitialTimer;
    }
}

