using System;
using GreeniumCore.Network.Discovery;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GreeniumCoreUnitTests
{
    [TestClass]
    public class SiteDiscoveryTest {
        [TestMethod]
        public void TestDiscovery()
        {
            Assert.IsTrue(SiteDiscovery.Discover("https://www.google.com/"));
            Assert.IsTrue(SiteDiscovery.Discover("https://www.facebook.com"));
            Assert.IsTrue(SiteDiscovery.Discover("stackoverflow.com"));
            Assert.IsFalse(SiteDiscovery.Discover("http:/duckduckgo.com"));

        }
    }
}
