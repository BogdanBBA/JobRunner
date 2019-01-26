using System;
using System.Collections.Generic;

namespace Jobs.Anniversaries.Classes
{
    /// <summary>
    /// 
    /// </summary>
    public class AnniversaryManager
    {
        private static Random RANDOM = new Random();

        public AnniversaryMemberDescription Description { get; set; }
        public DateTime LastExecution { get; set; }
        public List<AnniversaryDTO> Anniversaries { get; set; }

        public void AddAnniversary(string title, AnniversaryDTO.Categories category, int originalEventYear, int originalEventMonth, int originalEventDay, AnniversaryDTO.CelebrationFrequencies frequency, int headsUp)
        {
            this.Anniversaries.Add(new AnniversaryDTO()
            {
                Title = title,
                Category = category,
                OriginalEvent = new DateTime(originalEventYear, originalEventMonth, originalEventDay),
                Frequency = frequency,
                HeadsUpInDays = headsUp
            });
        }

        public static AnniversaryDTO GetSampleAnniversary()
        {
            return new AnniversaryDTO()
            {
                Title = "Sample",
                Category = (AnniversaryDTO.Categories)RANDOM.Next(Enum.GetValues(typeof(AnniversaryDTO.Categories)).Length),
                OriginalEvent = new DateTime(RANDOM.Next(1980, 2018), RANDOM.Next(1, 13), RANDOM.Next(1, 29)),
                Frequency = (AnniversaryDTO.CelebrationFrequencies)RANDOM.Next(Enum.GetValues(typeof(AnniversaryDTO.CelebrationFrequencies)).Length),
                HeadsUpInDays = RANDOM.Next(6)
            };
        }
    }
}
