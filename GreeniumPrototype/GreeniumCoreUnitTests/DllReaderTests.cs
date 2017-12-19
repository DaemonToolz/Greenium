using System;
using System.Reflection;
using GreeniumCore.Addons;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GreeniumCoreUnitTests {
    [TestClass]
    public class DllReaderTests {
        [TestMethod]
        public void TestLoading() {

            // load the assembly or type

                var reader = new DllReader($"{System.AppDomain.CurrentDomain.BaseDirectory}/Extensions/GreeniumExtensionExample.dll", true);
                Console.WriteLine(reader.LoadedAssembly);
                Console.WriteLine(reader.PType);
                if(reader.GetTypes() != null )
                    foreach (var type in reader.PluginTypes)
                        Console.WriteLine(type);
                reader.Execute(new object[]{new Object[0]{}});
            Console.WriteLine(reader.TargetType);
            Assert.IsTrue(reader.AssemblySecured());

        }
    }
}
