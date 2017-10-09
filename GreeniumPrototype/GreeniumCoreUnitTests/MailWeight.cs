using System;
using GreeniumCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GreeniumCoreUnitTests
{
    [TestClass]
    public class MailWeightTest
    {
        [TestMethod]
        public void TestWeight()
        {
            var inm = new InMail();
            var result = inm.GmailWeight();


            Console.WriteLine($"{result}g de CO² from GMAIL" );
            Assert.AreNotEqual(0, result);

            result = inm.OutlookWeight("Your email","Your password"); // Won't work like that x)


            Console.WriteLine($"{result}g de CO² from OUTLOOK");
            Assert.AreNotEqual(0, result);

        }
    }
}
