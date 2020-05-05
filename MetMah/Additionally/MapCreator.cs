using MetMah.Creature;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MetMah.Additionally
{
    public static class MapCreator
    {
        private static Random random = new Random();
        public static List<ICreature>[,] CreateMap(string map, string separator = "\r\n")
        {
            var rows = map.Split(new[] { separator, "\n" }, StringSplitOptions.RemoveEmptyEntries);
            if (rows.Select(z => z.Length).Distinct().Count() != 1)
                throw new Exception($"Wrong map '{map}'");
            var result = new List<ICreature>[rows[0].Length, rows.Length];
            for (var x = 0; x < rows[0].Length; x++)
                for (var y = 0; y < rows.Length; y++)
                {
                    var creature = CreateCreatureBySymbol(rows[y][x]);
                    result[x, y] = new List<ICreature>();
                    if (creature != null)
                        result[x, y].Add(creature);
                }
            return result;
        }

        private static ICreature CreateCreatureBySymbol(char symbol)
        {
            switch (symbol)
            {
                case 'P':
                    return new Player();
                case 'T':
                    return new Terrain();
                case 'L':
                    return new Stairs();
                case 'S':
                    return new Student(new Dialogue("Do you love me?",
                                                new string[] { "Yes", "No", "ZOPA" },
                                                "Yes"));
                case 'C':
                    return new CleverStudent(new Dialogue("Do you love me?",
                                                new string[] { "Yes", "No", "Maybe", "I don't know", "Help me", "012345678901234567890" },
                                                "Maybe"));
                case 'B':
                    return new Beer();
                case ' ':
                    return null;
                default:
                    throw new Exception($"wrong character for ICreature {symbol}");
            }
        }
    }
}
