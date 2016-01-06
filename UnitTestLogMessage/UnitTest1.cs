using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using InterviewCodeReview;

namespace UnitTestLogMessage
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethodLogMessage()
        {
            JobLogger.LogMessage("Test message", true, true, true, 0);
        }
    }
}
