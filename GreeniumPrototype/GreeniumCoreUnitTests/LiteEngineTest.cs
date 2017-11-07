using System;
using System.Collections.Generic;
using GreeniumCoreSQL.Engine;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace GreeniumCoreUnitTests
{
    [TestClass]
    public class LiteEngineTest
    {
        private LiteEngine Engine;

        [TestMethod]
        public void TestAll()
        {
            Engine = new LiteEngine();
            AddTest();
            ReadTest();
        }
        
        public void AddTest(){
            Engine.Add("https://google.com/");
            Engine.Add("https://MyDomain.com/5s4dqdq+sdze/Index?param=sdokndf546d98fs4fdsf948ff&type=GET");
        }

        public void ReadTest()
        {
            var content = Engine.Read("History");
            Assert.IsNotNull(content);
            Assert.IsTrue(content.ToList().Count > 0);

            foreach (var history in content)
            {
                Console.WriteLine($"{history.Time:dd/MM/yyyy} - { history.URL}");
                Engine.Remove(history);
            }
        }
    }
}
