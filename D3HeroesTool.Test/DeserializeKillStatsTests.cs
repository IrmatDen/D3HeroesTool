using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D3HeroesTool.Test
{
    [TestFixture]
    public class DeserializeKillStatsTests
    {
        [Test]
        public void monsters()
        {
            D3Data.Career c = D3Data.Deserializer.AsCareer("{ 'kills': { 'monsters':1764 } }");
            Assert.AreEqual(1764, c.kills.monsters);
        }

        [Test]
        public void hardcoreMonsters()
        {
            D3Data.Career c = D3Data.Deserializer.AsCareer("{ 'kills': { 'hardcoreMonsters':6 } }");
            Assert.AreEqual(6, c.kills.hardcoreMonsters);
        }

        [Test]
        public void elites()
        {
            D3Data.Career c = D3Data.Deserializer.AsCareer("{ 'kills': { 'elites':42 } }");
            Assert.AreEqual(42, c.kills.elites);
        }
    }
}
