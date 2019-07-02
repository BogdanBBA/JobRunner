using CommonCode.DataLayer;
using CommonCode.Utils;
using JobPoolUI.Classes;
using JobPoolUI.UI;
using Jobs;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace JobPoolUI
{
    public partial class FMain : Form
    {
        public const int INITIAL_TIMER_DURATION = 4000;
        public const int POST_JOB_RUN_SLEEP = 1000;

        protected DateTime appStartMoment;

        protected List<JobDescription> Jobs { get; private set; }
        protected List<JobView> JobViews { get; private set; }

        public FMain()
        {
            InitializeComponent();
            InitializeJobControls();

            //using (RegistryKey appKey = Registry.CurrentUser.CreateSubKey(Const.REGISTRY_PATH))
            //{
            //    appKey.SetValue(Const.REGISTRY_KEY_NAME, Path.GetFullPath(Directory.GetCurrentDirectory() + @"\..\..\..\"));
            //}
        }

        private void FMain_Load(object sender, EventArgs e)
        {
            appStartMoment = DateTime.Now;
            foreach (JobView jobView in JobViews)
                jobView.RefreshData();
            InitialTimer.Interval = INITIAL_TIMER_DURATION;
            InitialTimer.Start();
        }

        private void InitializeJobControls()
        {
            const int controlHeight = 60;

            ClientSize = new Size(400, JobAgency.JOB_TEMPLATES.Count * controlHeight);

            Jobs = new List<JobDescription>();
            JobViews = new List<JobView>();

            Rectangle bounds = new Rectangle(0, 0, ClientSize.Width, controlHeight);
            foreach (Tuple<int, int> template in JobAgency.JOB_TEMPLATES)
            {
                Jobs.Add(new JobDescription(template.Item1, JobFactory.GetName(template.Item1), template.Item2));
                JobViews.Add(new JobView(Jobs.Last()) { Parent = this, Bounds = bounds });
                bounds.Location = new Point(bounds.X, bounds.Y + controlHeight);
            }
        }

        private void InitialTimer_Tick(object sender, EventArgs e)
        {
            InitialTimer.Stop();
            CheckTimer_Tick(sender, e);
            CheckTimer.Start();
        }

        private void UpdateJobStatus(JobDescription job, JobDescription.JobStatus status, bool refreshData = true)
        {
            job.Status = status;
            if (refreshData)
                JobViews[Jobs.IndexOf(job)].RefreshData();
        }

        /// <summary>Starts a job process (based on the job name, via the homonymous in-solution project), waits for it to end and return its exit code, then returns said exit code. 
        /// If an error occurs, appends an error log and returns ReturnCodes.JOB_PROCESS_ERROR.</summary>
        public static int StartJobProcessAndReturnExitCode(JobDescription job)
        {
            Process proc = Process.Start(new ProcessStartInfo(Const.FILE_JOBS_EXE)
            {
                WorkingDirectory = Const.FOLDER_JOBS_BIN.Replace("JobPoolUI", "Jobs"),
				Arguments = $"Jobs\\bin\\Debug {job.JobID}"
            });
            proc.WaitForExit();
            return proc.ExitCode;
        }

        private void CheckTimer_Tick(object sender, EventArgs e)
        {
            Text = $"Job runner / running for {DateTime.Now.Subtract(appStartMoment).ToDHMS()}";

            // refresh each job control
            foreach (JobView jobView in JobViews)
                jobView.RefreshData();

            // if any job is executing, give it peace
            if (WorkerBgW.IsBusy)
                return;
            foreach (JobDescription job in Jobs)
                if (job.Status == JobDescription.JobStatus.Executing)
                    return;

            // check if there's a job ready to execute, and if yes, leave
            foreach (JobDescription job in Jobs)
                if (job.ItsAboutTimeToRun())
                {
                    UpdateJobStatus(job, JobDescription.JobStatus.Executing);
                    WorkerBgW.RunWorkerAsync(job);
                    return;
                }
        }

        private void WorkerBgW_DoWork(object sender, DoWorkEventArgs e)
        {
            JobDescription job = e.Argument as JobDescription;
            int resultCode = StartJobProcessAndReturnExitCode(job);
            e.Result = new KeyValuePair<JobDescription, bool>(job, !JobRunErrorCodes.ThereWasAProblem(resultCode));
            Thread.Sleep(POST_JOB_RUN_SLEEP);
        }

        private void WorkerBgW_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            KeyValuePair<JobDescription, bool> result = (KeyValuePair<JobDescription, bool>)e.Result;
            result.Key.RunCount++;
            if (!result.Value)
                result.Key.ErrorCount++;
            result.Key.NextTargetExecutionMoment = DateTime.Now.Add(result.Key.ExecutionInterval);
            UpdateJobStatus(result.Key, JobDescription.JobStatus.Waiting);
        }
    }
}
