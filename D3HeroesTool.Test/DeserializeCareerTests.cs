using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            D3Data.Career c = D3Data.Deserializer.AsCareer("{ 'lastHeroPlayed': 17313570 }");
            Assert.AreEqual(17313570, c.lastHeroPlayed);
        }

        [Test]
        public void lastUpdated()
        {
            D3Data.Career c = D3Data.Deserializer.AsCareer("{ 'lastUpdated': 1398794591 }");
            Assert.AreEqual(DateTime.Parse("2014-04-29 18:03:11.000"), c.lastUpdated);
        }
    }
}
