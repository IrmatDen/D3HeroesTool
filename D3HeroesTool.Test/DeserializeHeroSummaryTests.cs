using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D3HeroesTool.Test
{
    [TestFixture]
    public class DeserializeHeroSummaryTests
    {
        [Test]
        public void id()
        {
            D3Data.Career c = D3Data.Deserializer.AsCareer("{ 'heroes': [ { 'id':6174823 } ] }");
            Assert.AreEqual(6174823, c.heroes.First().id);
        }

        [Test]
        public void name()
        {
            D3Data.Career c = D3Data.Deserializer.AsCareer("{ 'heroes': [ { 'name':'Doctor' } ] }");
            Assert.AreEqual("Doctor", c.heroes.First().name);
        }

        [Test]
        public void level()
        {
            D3Data.Career c = D3Data.Deserializer.AsCareer("{ 'heroes': [ { 'level':70 } ] }");
            Assert.AreEqual(70, c.heroes.First().level);
            Assert.AreNotEqual(60, c.heroes.First().level);
        }

        [Test]
        public void hardcore()
        {
            D3Data.Career c = D3Data.Deserializer.AsCareer("{ 'heroes': [ { 'hardcore':false } ] }");
            Assert.IsFalse(c.heroes.First().hardcore);

            c = D3Data.Deserializer.AsCareer("{ 'heroes': [ { 'hardcore':true } ] }");
            Assert.IsTrue(c.heroes.First().hardcore);
        }

        [Test]
        public void dead()
        {
            D3Data.Career c = D3Data.Deserializer.AsCareer("{ 'heroes': [ { 'dead':false } ] }");
            Assert.IsFalse(c.heroes.First().dead);
        }

        [Test]
        public void genders()
        {
            D3Data.Career c = D3Data.Deserializer.AsCareer("{ 'heroes': [ { 'gender':0 } ] }");
            Assert.AreEqual(D3Data.Gender.Male, c.heroes.First().gender);

            c = D3Data.Deserializer.AsCareer("{ 'heroes': [ { 'gender':1 } ] }");
            Assert.AreEqual(D3Data.Gender.Female, c.heroes.First().gender);

            Assert.That(() => { D3Data.Deserializer.AsCareer("{ 'heroes': [ { 'gender':2 } ] }"); },
                        Throws.InnerException.TypeOf<IndexOutOfRangeException>());
        }

        [Test]
        public void lastUpdated()
        {
            D3Data.Career c = D3Data.Deserializer.AsCareer("{ 'heroes': [ { 'last-updated':1398727307 } ] }");
            Assert.AreEqual(DateTime.Parse("2014-04-28 23:21:47.000"), c.heroes.First().lastUpdated);
        }
    }
}
