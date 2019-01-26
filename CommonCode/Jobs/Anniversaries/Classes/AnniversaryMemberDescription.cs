using System;
using System.Text;

namespace Jobs.Anniversaries.Classes
{
    /// <summary>
    /// 
    /// </summary>
    public struct AnniversaryMemberDescription
    {
        public string Title
        {
            get { return "The name of the celebrated person / holiday / event / etc. (text string)."; }
            set { }
        }

        public string Category
        {
            get
            {
                StringBuilder sb = new StringBuilder("The type of anniversary, which determines the thematic styling of the reminder (integer number: ");
                foreach (var category in Enum.GetValues(typeof(AnniversaryDTO.Categories)))
                    sb.Append(category.ToString()).Append('=').Append((int)category).Append(", ");
                return sb.Remove(sb.Length - 2, 2).Append(").").ToString();
            }
            set { }
        }

        public string OriginalEvent
        {
            get { return "The date of the original event (text string in a 'yyyy-MM-dd' format)."; }
            set { }
        }

        public string Frequency
        {
            get
            {
                StringBuilder sb = new StringBuilder("Reminder frequency / at what type of time interval is an anniversary celebrated (integer number: ");
                foreach (var frequency in Enum.GetValues(typeof(AnniversaryDTO.CelebrationFrequencies)))
                    sb.Append(frequency.ToString()).Append('=').Append((int)frequency).Append(", ");
                return sb.Remove(sb.Length - 2, 2).Append(").").ToString();
            }
            set { }
        }

        public string HeadsUpInDays
        {
            get { return "The number of days before the event when the reminder should be sent (positive integer number; 0 means no heads-up). A single reminder on the anniversary day is sent indifferent of the heads up."; }
            set { }
        }
    }
}
