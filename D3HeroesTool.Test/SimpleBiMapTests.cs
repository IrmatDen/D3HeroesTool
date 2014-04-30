using D3HeroesTool.Utils;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D3HeroesTool.Test
{
    [TestFixture]
    class SimpleBiMapTests
    {
        [Test]
        public void Test_Count_1()
        {
            SimpleBiMap<string, int> bm = new SimpleBiMap<string, int>();
            bm.Add("answer", 42);
            Assert.AreEqual(1, bm.Count);
        }

        [Test]
        public void Test_Count_2()
        {
            SimpleBiMap<string, int> bm = new SimpleBiMap<string, int>();
            bm.Add("answer", 42);
            bm.Add("answer²", 42*42);
            Assert.AreEqual(2, bm.Count);
        }

        [Test]
        public void Test_ByLeft()
        {
            SimpleBiMap<string, int> bm = new SimpleBiMap<string, int>();
            bm.Add("answer", 42);

            int theAnswer;
            Assert.AreEqual(true, bm.TryGetByLeft("answer", out theAnswer));
        }

        [Test]
        public void Test_ByRight()
        {
            SimpleBiMap<string, int> bm = new SimpleBiMap<string, int>();
            bm.Add("answer", 42);

            string theQuestion;
            Assert.AreEqual(true, bm.TryGetByRight(42, out theQuestion));
        }

        [Test]
        public void Test_FailedAdd()
        {
            SimpleBiMap<string, int> bm = new SimpleBiMap<string, int>();
            bm.Add("answer", 42);
            Assert.Throws<ArgumentException>(() => { bm.Add("answer", 42); });
            Assert.AreEqual(1, bm.Count);
        }

        [Test]
        public void Test_Failed_ByLeft()
        {
            SimpleBiMap<string, int> bm = new SimpleBiMap<string, int>();
            bm.Add("answer", 42);

            int theAnswer;
            Assert.AreEqual(false, bm.TryGetByLeft("foo", out theAnswer));
        }

        [Test]
        public void Test_Failed_ByRight()
        {
            SimpleBiMap<string, int> bm = new SimpleBiMap<string, int>();
            bm.Add("answer", 42);

            string theQuestion;
            Assert.AreEqual(false, bm.TryGetByRight(21, out theQuestion));
        }

        [Test]
        public void Test_Add_Using_Explicit_IBimap()
        {
            SimpleBiMap<string, int> bm = new SimpleBiMap<string, int>();
            ((IBiMap)bm).Add("answer", 42);
            Assert.AreEqual(1, bm.Count);
        }

        [Test]
        public void Test_ByLeft_Using_Explicit_IBimap()
        {
            SimpleBiMap<string, int> bm = new SimpleBiMap<string, int>();
            ((IBiMap)bm).Add("answer", 42);

            object theAnswer;
            Assert.AreEqual(true, ((IBiMap)bm).TryGetByLeft("answer", out theAnswer));
            Assert.AreEqual(42, (int)theAnswer);
        }

        [Test]
        public void Test_ByRight_Using_Explicit_IBimap()
        {
            SimpleBiMap<string, int> bm = new SimpleBiMap<string, int>();
            ((IBiMap)bm).Add("answer", 42);

            object theQuestion;
            Assert.AreEqual(true, ((IBiMap)bm).TryGetByRight(42, out theQuestion));
            Assert.AreEqual("answer", (string)theQuestion);
        }

        [Test]
        public void Test_LeftValues()
        {
            SimpleBiMap<string, int> bm = new SimpleBiMap<string, int>();
            bm.Add("answer", 42);
            bm.Add("answer²", 42 * 42);

            string[] exp_lvalues = new string[] { "answer", "answer²" };
            Assert.That(bm.LeftValues, Is.EquivalentTo(exp_lvalues));
        }

        [Test]
        public void Test_RightValues()
        {
            SimpleBiMap<string, int> bm = new SimpleBiMap<string, int>();
            bm.Add("answer", 42);
            bm.Add("answer²", 42 * 42);

            int[] exp_rvalues = new int[] { 42, 1764 };
            Assert.That(bm.RightValues, Is.EquivalentTo(exp_rvalues));
        }
    }
}
