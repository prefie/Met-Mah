using MetMah.Creature;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace MetMah.Additionally
{
    public static class Bfs
    {
        public static SinglyLinkedList<Point> FindPaths(List<ICreature>[,] map, Point start, Point finish)
        {
            if (start == finish)
                return null;
            var queue = new Queue<SinglyLinkedList<Point>>();
            var visited = new HashSet<Point>();
            queue.Enqueue(new SinglyLinkedList<Point>(start));
            var width = map.GetLength(0);
            var height = map.GetLength(1);

            while (queue.Count > 0)
            {
                var path = queue.Dequeue();
                var point = path.Value;
                if (!visited.Contains(point) && point.X >= 0 && point.X < width && point.Y >= 0 && point.Y < height - 1 &&
                    !map[point.X, point.Y].Any(x => x is Terrain) &&
                    (map[point.X, point.Y + 1].Any(x => x is Terrain || x is Stairs) ||
                    (map[point.X, point.Y].Any(x => x is Stairs) && path.Previous.Value.Y < point.Y)))
                {
                    if (finish == point)
                        return path.Reverse();
                    visited.Add(point);
                    AddPoint(queue, path);
                }
            }
            return null;
        }

        public static SinglyLinkedList<Point> FindPaths(string textMap, Point start, Point finish)
        {
            var map = MapCreator.CreateMap(textMap);
            return FindPaths(map, start, finish);
        }

        private static void AddPoint(Queue<SinglyLinkedList<Point>> queue, SinglyLinkedList<Point> cell)
        {
            var point = cell.Value;
            for (var dy = -1; dy <= 1; dy++)
                for (var dx = -1; dx <= 1; dx++)
                {
                    if (dx != 0 && dy != 0)
                        continue;
                    var newPoint = new Point(point.X + dx, point.Y + dy);
                    queue.Enqueue(new SinglyLinkedList<Point>(newPoint, cell));
                }
        }
    }
}
