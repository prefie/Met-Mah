using MetMah.Creature;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MetMah.Additionally
{
    public class Level
    {
        private readonly List<ICreature>[,] Map;
        public bool IsOver => CountBeer() == 0;
        public readonly int Width;
        public readonly int Height;
        public readonly string TextInitiallyMap;

        public Level(string map)
        {
            Map = MapCreator.CreateMap(map);
            Width = Map.GetLength(0);
            Height = Map.GetLength(1);
            TextInitiallyMap = map;
        }

        public int CountBeer()
        {
            var countBeer = 0;
            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                    if (Map[x, y].Any(creature => creature is Beer))
                        countBeer++;
            return countBeer;
        }

        public void AddCreature(int x, int y, ICreature creature)
        {
            if (x < 0 || x >= Width || y < 0 || y >= Height)
                throw new ArgumentException();

            Map[x, y].Add(creature);
        }

        public void RemoveCreature(int x, int y, Type typeCreature)
        {
            if (x < 0 || x >= Width || y < 0 || y >= Height)
                throw new ArgumentException();

            ICreature creature = null;
            foreach (var e in Map[x, y])
                if (e.GetType() == typeCreature)
                {
                    creature = e;
                    break;
                }
            if (creature != null)
                Map[x, y].Remove(creature);
        }

        public void SetCreatures(int x, int y, IEnumerable<ICreature> creatures) =>
            Map[x, y] = creatures.ToList();

        public bool CheckCreature(int x, int y, Type typeCreature)
        {
            if (x < 0 || x >= Width || y < 0 || y >= Height)
                throw new ArgumentException();

            return Map[x, y].Any(z => z != null && z.GetType() == typeCreature);
        }

        public IEnumerable<ICreature> GetCreatures(int x, int y)
        {
            if (x < 0 || x >= Width || y < 0 || y >= Height)
                throw new ArgumentException();

            foreach (var e in Map[x, y])
                yield return e;
        }
    }
}
