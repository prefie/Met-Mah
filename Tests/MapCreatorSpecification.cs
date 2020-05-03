using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EscapeFromMetMah;
using MetMah.Additionally;
using MetMah.Creature;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    class MapCreatorSpecification
    {
        [Test]
        public void MapCreator_ShouldThrowException_WhenMapEmpty()
        {
            Assert.Catch(() => MapCreator.CreateMap(""));
        }

        [Test]
        public void MapCreator_ShouldThrowException_WhenMapIncorrect()
        {
            Assert.Catch(() => MapCreator.CreateMap(" \r\nTT"));
            Assert.Catch(() => MapCreator.CreateMap("   \r\n  N"));
        }

        [Test]
        public void MapCreator_ShouldReturnCorrectMap_WhenAllCorrect()
        {
            var str = "P C\r\nTTT\r\nLLL\r\nSSS\r\nBBB\r\n";
            var map = MapCreator.CreateMap(str);
            foreach (var e in map)
                Assert.IsTrue(e.Count < 2);
            Assert.IsTrue(map[0, 0].Any(x => x is Player));
            Assert.IsTrue(map[2, 0].Any(x => x is CleverStudent));
            AssertTypeInRow(map, typeof(Terrain), 3, 1);
            AssertTypeInRow(map, typeof(Stairs), 3, 2);
            AssertTypeInRow(map, typeof(Student), 3, 3);
            AssertTypeInRow(map, typeof(Beer), 3, 4);
        }

        private void AssertTypeInRow(List<ICreature>[,] map, Type type, int lengthRow, int numberRow)
        {
            for (int i = 0; i < lengthRow; i++)
                Assert.IsTrue(map[i, numberRow].Any(x => x.GetType() == type));
        }
    }
}
