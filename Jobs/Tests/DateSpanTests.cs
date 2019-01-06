using Jobs.Anniversaries.Classes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Jobs.Tests
{
    [TestClass]
    public class DateSpanTests
    {
        [TestMethod]
        public void NoTime()
        {
            DateTime eventDate = new DateTime(2014, 12, 31);
            DateTime today = new DateTime(2014, 12, 31).Date;
            DateSpan span = new DateSpan(eventDate, today);
            Assert.AreEqual(0, span.Years);
            Assert.AreEqual(0, span.TotalMonths);
            Assert.AreEqual(0, span.TotalWeeks);
            Assert.AreEqual(0, span.TotalDays);
            Assert.AreEqual(0, span.MonthsInYear);
            Assert.AreEqual(0, span.DaysInMonth);
        }

        [TestMethod]
        public void OneDay()
        {
            DateTime eventDate = new DateTime(2014, 12, 31);
            DateTime today = new DateTime(2015, 1, 1).Date;
            DateSpan span = new DateSpan(eventDate, today);
            Assert.AreEqual(0, span.Years);
            Assert.AreEqual(0, span.TotalMonths);
            Assert.AreEqual(0, span.TotalWeeks);
            Assert.AreEqual(1, span.TotalDays);
            Assert.AreEqual(0, span.MonthsInYear);
            Assert.AreEqual(1, span.DaysInMonth);
        }

        [TestMethod]
        public void OneWeek()
        {
            DateTime eventDate = new DateTime(2018, 1, 29);
            DateTime today = new DateTime(2018, 2, 5).Date;
            DateSpan span = new DateSpan(eventDate, today);
            Assert.AreEqual(0, span.Years);
            Assert.AreEqual(0, span.TotalMonths);
            Assert.AreEqual(1, span.TotalWeeks);
            Assert.AreEqual(7, span.TotalDays);
            Assert.AreEqual(0, span.MonthsInYear);
            Assert.AreEqual(7, span.DaysInMonth);
        }

        [TestMethod]
        public void FiveWeeks()
        {
            DateTime eventDate = new DateTime(2018, 1, 29);
            DateTime today = new DateTime(2018, 3, 5).Date;
            DateSpan span = new DateSpan(eventDate, today);
            Assert.AreEqual(0, span.Years);
            Assert.AreEqual(1, span.TotalMonths);
            Assert.AreEqual(5, span.TotalWeeks);
            Assert.AreEqual(35, span.TotalDays);
            Assert.AreEqual(1, span.MonthsInYear);
            Assert.AreEqual(5, span.DaysInMonth);
        }

        [TestMethod]
        public void OneMonth()
        {
            DateTime eventDate = new DateTime(2018, 2, 5);
            DateTime today = new DateTime(2018, 3, 5).Date;
            DateSpan span = new DateSpan(eventDate, today);
            Assert.AreEqual(0, span.Years);
            Assert.AreEqual(1, span.TotalMonths);
            Assert.AreEqual(4, span.TotalWeeks);
            Assert.AreEqual(28, span.TotalDays);
            Assert.AreEqual(1, span.MonthsInYear);
            Assert.AreEqual(0, span.DaysInMonth);
        }

        [TestMethod]
        public void FiftyTwoWeeks()
        {
            DateTime eventDate = new DateTime(2018, 2, 5);
            DateTime today = new DateTime(2019, 2, 4).Date;
            DateSpan span = new DateSpan(eventDate, today);
            Assert.AreEqual(0, span.Years);
            Assert.AreEqual(11, span.TotalMonths);
            Assert.AreEqual(52, span.TotalWeeks);
            Assert.AreEqual(364, span.TotalDays);
            Assert.AreEqual(11, span.MonthsInYear);
            Assert.AreEqual(30, span.DaysInMonth);
        }

        [TestMethod]
        public void OneYear()
        {
            DateTime eventDate = new DateTime(2018, 2, 5);
            DateTime today = new DateTime(2019, 2, 5).Date;
            DateSpan span = new DateSpan(eventDate, today);
            Assert.AreEqual(1, span.Years);
            Assert.AreEqual(12, span.TotalMonths);
            Assert.AreEqual(52, span.TotalWeeks);
            Assert.AreEqual(365, span.TotalDays);
            Assert.AreEqual(0, span.MonthsInYear);
            Assert.AreEqual(0, span.DaysInMonth);
        }

        [TestMethod]
        public void ThreeYearsWithoutOneDay()
        {
            DateTime eventDate = new DateTime(2015, 1, 1);
            DateTime today = new DateTime(2017, 12, 31).Date;
            DateSpan span = new DateSpan(eventDate, today);
            Assert.AreEqual(2, span.Years);
            Assert.AreEqual(35, span.TotalMonths);
            Assert.AreEqual(156, span.TotalWeeks);
            Assert.AreEqual(1095, span.TotalDays);
            Assert.AreEqual(11, span.MonthsInYear);
            Assert.AreEqual(30, span.DaysInMonth);
        }

        [TestMethod]
        public void ThreeYears()
        {
            DateTime eventDate = new DateTime(2015, 1, 1);
            DateTime today = new DateTime(2018, 1, 1).Date;
            DateSpan span = new DateSpan(eventDate, today);
            Assert.AreEqual(3, span.Years);
            Assert.AreEqual(36, span.TotalMonths);
            Assert.AreEqual(156, span.TotalWeeks);
            Assert.AreEqual(1096, span.TotalDays);
            Assert.AreEqual(0, span.MonthsInYear);
            Assert.AreEqual(0, span.DaysInMonth);
        }

        [TestMethod]
        public void ThreeYearsAndOneDay()
        {
            DateTime eventDate = new DateTime(2014, 12, 31);
            DateTime today = new DateTime(2018, 1, 1).Date;
            DateSpan span = new DateSpan(eventDate, today);
            Assert.AreEqual(3, span.Years);
            Assert.AreEqual(36, span.TotalMonths);
            Assert.AreEqual(156, span.TotalWeeks);
            Assert.AreEqual(1097, span.TotalDays);
            Assert.AreEqual(0, span.MonthsInYear);
            Assert.AreEqual(1, span.DaysInMonth);
        }
    }
}
