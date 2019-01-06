using Jobs.Anniversaries;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Jobs.Tests
{
    [TestClass]
    public class AnniversaryTests
    {
        private static DateTime TODAY;
        private static DateTime ORIGINAL_EVENT;
        private AnniversaryDTO anni;
        private DateTime date;

        [ClassInitialize]
        public static void Initialize(TestContext context)
        {
            ORIGINAL_EVENT = new DateTime(2018, 5, 20);
            TODAY = new DateTime(2018, 5, 20);
        }

        [TestInitialize]
        public void Initialize()
        {
            anni = new AnniversaryDTO() { OriginalEvent = ORIGINAL_EVENT, HeadsUpInDays = 0 };
        }

        [TestMethod]
        public void AnniversaryIsToday()
        {
            // all frequencies: with passed original event being today, then tomorrow
            foreach (AnniversaryDTO.CelebrationFrequencies frequency in Enum.GetValues(typeof(AnniversaryDTO.CelebrationFrequencies)))
            {
                anni.Frequency = frequency;
                Assert.IsTrue(anni.AnniversaryIsToday(anni.NextAnniversary(TODAY), TODAY));
                if (frequency != AnniversaryDTO.CelebrationFrequencies.Daily)
                    Assert.IsFalse(anni.AnniversaryIsToday(anni.NextAnniversary(TODAY), TODAY.AddDays(1)));
            }
        }

        [TestMethod]
        public void ShouldSendReminder()
        {
            int[] headsUps = new int[] { 0, 1, 7, 31, 365 };

            // all frequencies (except once-only)
            foreach (AnniversaryDTO.CelebrationFrequencies frequency in Enum.GetValues(typeof(AnniversaryDTO.CelebrationFrequencies)))
                if (frequency != AnniversaryDTO.CelebrationFrequencies.OnlyOnce)
                {
                    anni.Frequency = frequency;

                    foreach (int headsUp in headsUps)
                    {
                        anni.OriginalEvent = TODAY.AddDays(headsUp);
                        anni.HeadsUpInDays = headsUp;
                        Assert.IsTrue(anni.ShouldSendReminder(TODAY));
                    }
                }

            // frequency: only once
            anni.Frequency = AnniversaryDTO.CelebrationFrequencies.OnlyOnce;
            foreach (int headsUp in headsUps)
            {
                anni.OriginalEvent = TODAY.AddDays(headsUp);
                anni.HeadsUpInDays = headsUp;
                Assert.IsTrue(anni.ShouldSendReminder(TODAY) == (headsUp == 0));
            }
        }

        [TestMethod]
        public void NextAnniversary()
        {
            // frequency: only once
            anni.Frequency = AnniversaryDTO.CelebrationFrequencies.OnlyOnce;
            foreach (int daysToAdd in new int[] { -1, 1 })
            {
                DateTime today = TODAY.AddDays(daysToAdd);
                Assert.AreNotEqual(today, anni.NextAnniversary(today));
            }

            // frequency: daily 
            anni.Frequency = AnniversaryDTO.CelebrationFrequencies.Daily;
            foreach (int daysToAdd in new int[] { 1, 7, 31, 365 })
            {
                DateTime today = TODAY.AddDays(daysToAdd);
                Assert.AreEqual(today, anni.NextAnniversary(today));
            }

            // frequency: weekly
            anni.Frequency = AnniversaryDTO.CelebrationFrequencies.Weekly;
            foreach (int weeksToAdd in new int[] { 0, 1, 4, 5, 52, 53 })
            {
                date = TODAY.AddDays(weeksToAdd * 7);
                Assert.AreEqual(date, anni.NextAnniversary(date));
                date = TODAY.AddDays(weeksToAdd * 7 + 1);
                Assert.AreNotEqual(date, anni.NextAnniversary(date));
            }

            // frequency: monthly
            anni.Frequency = AnniversaryDTO.CelebrationFrequencies.Monthly;
            foreach (int monthsToAdd in new int[] { 1, 11, 12, 13 })
            {
                date = TODAY.AddMonths(monthsToAdd);
                Assert.AreEqual(date, anni.NextAnniversary(date));
            }

            // frequency: yearly
            anni.Frequency = AnniversaryDTO.CelebrationFrequencies.Yearly;
            date = TODAY.AddYears(1);
            Assert.AreEqual(date, anni.NextAnniversary(date));
        }
    }
}
