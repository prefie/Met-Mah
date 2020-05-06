using System.Drawing;
using MetMah.Additionally;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    class BfsSpecification
    {
        [Test]
        public void Bfs_ShouldReturnNull_WhenEmptyMap()
        {
            Assert.IsNull(Bfs.FindPaths(" ", new Point(), new Point()));
        }

        [Test]
        public void Bfs_ShouldReturnCorrectPath_OnSimpleMap()
        {
            var str = "     \r\nTTTTT";
            var path = new Point[] { new Point(3, 0), new Point(2, 0), new Point(1, 0), new Point(0, 0) };
            var resultBfs = Bfs.FindPaths(str, new Point(3, 0), new Point(0, 0));
            AssertPath(resultBfs, path);
        }

        [Test]
        public void Bfs_ShouldReturnCorrectPath_OnMapWithStairs()
        {
            var str = "     \r\nTTTLT\r\n   L \r\nTTTTT";
            var path = new Point[] { new Point(2, 2), new Point(3, 2), new Point(3, 1), new Point(3, 0), new Point(2, 0) };
            var resultBfs = Bfs.FindPaths(str, new Point(2, 2), new Point(2, 0));
            AssertPath(resultBfs, path);
        }

        [Test]
        public void Bfs_ShouldReturnCorrectPath_WhenSeveralPaths()
        {
            var str = "      \r\nLTTTLT\r\nL   L \r\nTTTTTT";
            var path = new Point[] { new Point(2, 2), new Point(1, 2),
                new Point(0, 2), new Point(0, 1), new Point(0, 0), new Point(1, 0) };
            var resultBfs = Bfs.FindPaths(str, new Point(2, 2), new Point(1, 0));
            AssertPath(resultBfs, path);
        }

        [Test]
        public void Bfs_ShouldReturnNull_WhenStairsFar()
        {
            var str = "   \r\nTTL\r\n  L\r\n   \r\nTTT";
            Assert.IsNull(Bfs.FindPaths(str, new Point(0, 3), new Point(0, 0)));
        }

        [Test]
        public void Bfs_ShouldReturnNull_WhenPathNotExist()
        {
            var str = "   \r\nTTT\r\n   ";
            Assert.IsNull(Bfs.FindPaths(str, new Point(0, 2), new Point(0, 0)));
        }

        [Test]
        public void Bfs_ShouldReturnCorrectPath_WhenPathLeadStairs()
        {
            var str = "   \r\nTTL\r\n   ";
            var path = new Point[] { new Point(0, 0), new Point(1, 0), new Point(2, 0), new Point(2, 1) };
            var resultBfs = Bfs.FindPaths(str, new Point(0, 0), new Point(2, 1));
            AssertPath(resultBfs, path);
        }

        [Test]
        public void Bfs_ShouldReturnNull_WhenPointInBottomMap()
        {
            var str = "   \r\nTTT\r\n";
            Assert.IsNull(Bfs.FindPaths(str, new Point(0, 0), new Point(2, 1)));
        }

        private void AssertPath(SinglyLinkedList<Point> resultBfs, Point[] path)
        {
            var c = 0;
            foreach (var e in resultBfs)
                Assert.AreEqual(e, path[c++]);
        }
    }
}