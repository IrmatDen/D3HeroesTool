using D3Data.Attributes;
using D3HeroesTool.Utils;
using NUnit.Framework;
using System.Collections.ObjectModel;
using System.Windows.Data;

namespace D3HeroesTool.Test
{
    enum Heroes
    {
        [DisplayString("Doctor? Doctor Who?")]
        DrWho,
        [DisplayString("Cpt. Jack Harness")]
        CptJackHarness,
        Yoda
    }

    [TestFixture]
    class EnumDisplayerTests
    {
        EnumDisplayer ed;

        [TestFixtureSetUp]
        public void SetupFixture()
        {
            ed = new EnumDisplayer(typeof(Heroes));
            
            // Put EnumDisplayer in a usable state
            ReadOnlyCollection<string> dummy = ed.DisplayNames;
        }

        [Test]
        public void Test_DisplayNamesMatchHeroesCount()
        {
            Assert.AreEqual(3, ed.DisplayNames.Count);
        }

        [Test]
        public void Test_DisplayNameMatch_WhenKnown()
        {
            string displ = ((IValueConverter)ed).Convert(Heroes.DrWho, null, null, null) as string;
            Assert.AreEqual("Doctor? Doctor Who?", displ);
        }

        [Test]
        public void Test_DisplayNameMatch_WhenUnknown()
        {
            string displ = ((IValueConverter)ed).Convert(Heroes.Yoda, null, null, null) as string;
            Assert.AreEqual("Yoda", displ);
        }

        [Test]
        public void Test_EnumFromString_WhenKnown()
        {
            Heroes? value = ((IValueConverter)ed).ConvertBack("Doctor? Doctor Who?", null, null, null) as Heroes?;
            Assert.AreEqual(Heroes.DrWho, value);
        }

        [Test]
        public void Test_EnumFromString_WhenUnknown()
        {
            Heroes? value = ((IValueConverter)ed).ConvertBack("Yoda", null, null, null) as Heroes?;
            Assert.AreEqual(Heroes.Yoda, value);
        }
    }
}
