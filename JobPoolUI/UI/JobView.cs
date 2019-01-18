using CommonCode;
using JobPoolUI.Classes;
using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace JobPoolUI.UI
{
    public partial class JobView : UserControl
    {
        protected JobDescription Job { get; private set; }

        public JobView(JobDescription job)
        {
            InitializeComponent();
            ProgressPrB.Maximum = ProgressPrB.Width;
            Job = job;
        }

        private void JobView_Load(object sender, EventArgs e)
        {
            //
        }

        public void RefreshData()
        {
            StatusPB.Image = Job.Status == JobDescription.JobStatus.Waiting ? Properties.Resources.waiting : Properties.Resources.executing;
            NameL.Text = $"{Job.JobName}{(Job.ErrorCount == 0 ? "" : " :(")}";
            DetailsL.Text = GetDetailsText();
            ProgressPrB.Value = (int)(GetProgressPercentage() * ProgressPrB.Maximum);
        }

        private string GetDetailsText()
        {
            string state = "Executing";
            if (Job.Status == JobDescription.JobStatus.Waiting)
            {
                DateTime now = DateTime.Now;
                if (now.CompareTo(Job.NextTargetExecutionMoment) < 0)
                {
                    TimeSpan timeSpan = Job.NextTargetExecutionMoment.Subtract(now);
                    state = $"Waiting for another {timeSpan.ToDHMS()}";
                }
                else
                {
                    TimeSpan timeSpan = now.Subtract(Job.NextTargetExecutionMoment);
                    state = $"Waiting, late {timeSpan.ToDHMS()}";
                }
            }
            string timesRan = $"{Job.RunCount} {(Job.RunCount == 1 ? "time" : "times")}";
            string timesRelative = Job.Status == JobDescription.JobStatus.Waiting ? "so far" : "before now";
            string errors = $"{(Job.ErrorCount == 0 ? "no errors" : (Job.ErrorCount == 1 ? "1 error" : Job.ErrorCount + "errors"))}";
            return $"{state} / ran {timesRan} {timesRelative} / {errors}";
        }

        private double GetProgressPercentage()
        {
            DateTime start = Job.NextTargetExecutionMoment.Subtract(Job.ExecutionInterval);
            double result = (double)(DateTime.Now.Ticks - start.Ticks) / (Job.NextTargetExecutionMoment.Ticks - start.Ticks);
            return Math.Max(0.0, Math.Min(1.0, result));
        }

        private void JobView_MouseEnter(object sender, EventArgs e)
        {
            BackColor = Color.MintCream;
        }

        private void JobView_MouseLeave(object sender, EventArgs e)
        {
            BackColor = SystemColors.Control;
        }
    }
}
