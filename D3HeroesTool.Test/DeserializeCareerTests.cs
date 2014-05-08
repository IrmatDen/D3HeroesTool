using NUnit.Framework;
using System;
using System.Linq;

namespace D3HeroesTool.Test
{
    [TestFixture]
    public class DeserializeCareerTests
    {
        [Test]
        public void battleTag()
        {
            D3Data.Career c = D3Data.Deserializer.AsCareer("{ 'battleTag': 'IrmatDen#2108' }");
            Assert.AreEqual("IrmatDen#2108", c.battleTag);
        }

        [Test]
        public void paragonLevel()
        {
            D3Data.Career c = D3Data.Deserializer.AsCareer("{ 'paragonLevel': 238 }");
            Assert.AreEqual(238, c.paragonLevel);
        }

        [Test]
        public void paragonLevelHardcore()
        {
            D3Data.Career c = D3Data.Deserializer.AsCareer("{ 'paragonLevelHardcore': 63 }");
            Assert.AreEqual(63, c.paragonLevelHardcore);
        }

        [Test]
        public void lastHeroPlayed()
        {
            D3Data.Career c = D3Data.Deserializer.AsCareer(@"{ 'heroes': [ { 'id':0, 'name':'Krash', 'class':'barbarian' },
                                                                           { 'id':1, 'name':'DeMolay', 'class':'crusader' } ],
                                                               'lastHeroPlayed': 0 }");
            Assert.AreEqual("Krash", c.lastHeroPlayed.name);
        }

        [Test]
        public void lastUpdated()
        {
            D3Data.Career c = D3Data.Deserializer.AsCareer("{ 'lastUpdated': 1398794591 }");
            Assert.AreEqual(DateTime.Parse("2014-04-29 18:03:11.000"), c.lastUpdated);
        }

        [Test]
        public void heroes_count()
        {
            D3Data.Career c = D3Data.Deserializer.AsCareer(@"{ 'heroes': [ { 'id':0, 'name':'Krash', 'class':'barbarian' },
                                                                           { 'id':1, 'name':'DeMolay', 'class':'crusader' } ] }");
            Assert.AreEqual(2, c.heroes.Count);
        }

        [Test]
        public void heroes_last_class()
        {
            D3Data.Career c = D3Data.Deserializer.AsCareer(@"{ 'heroes': [ { 'id':0, 'name':'Krash', 'class':'barbarian' },
                                                                           { 'id':1, 'name':'DeMolay', 'class':'crusader' } ] }");
            Assert.AreEqual(D3Data.D3Class.Barbarian, c.heroes.First().d3class);
        }

        [Test]
        public void heroes_first_name()
        {
            D3Data.Career c = D3Data.Deserializer.AsCareer(@"{ 'heroes': [ { 'id':0, 'name':'Krash', 'class':'barbarian' },
                                                                           { 'id':1, 'name':'DeMolay', 'class':'crusader' } ] }");
            Assert.AreEqual("DeMolay", c.heroes.Last().name);
        }

        [Test]
        public void heroes_same_name()
        {
            D3Data.Career c = D3Data.Deserializer.AsCareer(@"{ 'heroes': [ { 'id':0, 'name':'Krash', 'class':'barbarian' },
                                                                           { 'id':1, 'name':'Krash', 'class':'barbarian' },
                                                                           { 'id':2, 'name':'DeMolay', 'class':'crusader' } ] }");
            Assert.AreEqual(3, c.heroes.Count);
        }
    }
}
