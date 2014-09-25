using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TeamCityConsoleApplicationTests
{
    [TestClass]
    //[TestFixture]
    public class UnitTest1
    {
        //[Test]
        [TestMethod]
        public void TestMethod1()
        {
            Assert.AreEqual(10, 10);
        }

        //[Test]
        [TestMethod]
        public void TestMethod2()
        {
            Assert.AreEqual(15, 15);
        }
    }
}
