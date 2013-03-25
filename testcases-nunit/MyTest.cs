using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonUtils
{
    using NUnit.Framework;

    [TestFixture]
    public class MyTest
    {
        [Test]
        public void test1()
        {
            Assert.AreEqual(1,1);
        }

        [Test]
        public void test2()
        {
            Assert.AreEqual(1, 1);
        }
        [Test]
        public void test3()
        {
            Assert.AreEqual(1, 1);
        }

        [Test]
        public void test4()
        {
            Assert.AreEqual(1, 1);
        }

        [Test]
        public void test5()
        {
            Assert.AreEqual(1, 1);
        }

        [Test]
        public void test6()
        {
            Assert.AreEqual(1, 1);
        }

        [Test]
        public void test7()
        {
            Assert.AreEqual(1, 1);
        }

        [Test]
        public void test8()
        {
            Assert.AreEqual(1, 3);
        }
    }
}
