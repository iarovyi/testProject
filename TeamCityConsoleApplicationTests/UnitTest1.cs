using System;
using NUnit;
using NUnit.Framework;

namespace TeamCityConsoleApplicationTests
{
    [TestFixture]
    public class UnitTest1
    {
        [Test]
        public void TestMethod1()
        {
            Assert.AreEqual(10, 10);
        }

        [Test]
        public void TestMethod2()
        {
            Assert.AreEqual(15, 15);
        }
    }
}
