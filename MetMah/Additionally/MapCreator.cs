using MetMah.Creature;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MetMah.Additionally
{
    public static class MapCreator
    {
        private static List<Dialogue> dialogues;
        private static readonly Random random = new Random();

        public static List<ICreature>[,] CreateMap(string map, int numberPlayer, string separator = "\r\n")
        {
            dialogues = FillDialogues();
            var rows = map.Split(new[] { separator, "\n" }, StringSplitOptions.RemoveEmptyEntries);
            if (rows.Select(z => z.Length).Distinct().Count() != 1)
                throw new Exception($"Wrong map '{map}'");
            var result = new List<ICreature>[rows[0].Length, rows.Length];
            for (var x = 0; x < rows[0].Length; x++)
                for (var y = 0; y < rows.Length; y++)
                {
                    var creature = CreateCreatureBySymbol(rows[y][x], numberPlayer);
                    result[x, y] = new List<ICreature>();
                    if (creature != null)
                        result[x, y].Add(creature);
                }
            return result;
        }

        private static ICreature CreateCreatureBySymbol(char symbol, int numberPlayer)
        {
            switch (symbol)
            {
                case 'P':
                    return new Player(numberPlayer);
                case 'T':
                    return new Terrain();
                case 'L':
                    return new Stairs();
                case 'S':
                    var number = random.Next(dialogues.Count);
                    var dialogue = dialogues[number];
                    dialogues.RemoveAt(number);
                    return new Student(dialogue);
                case 'C':
                    number = random.Next(dialogues.Count);
                    dialogue = dialogues[number];
                    dialogues.RemoveAt(number);
                    return new CleverStudent(dialogue);
                case 'B':
                    return new Beer();
                case 'D':
                    return new Door();
                case ' ':
                    return null;
                default:
                    throw new Exception($"wrong character for ICreature {symbol}");
            }
        }

        private static List<Dialogue> FillDialogues()
        {
            var listDialogues = new List<Dialogue>();
            var pathDialogues = new DirectoryInfo(@"Resources\Dialogues");
            foreach (var e in pathDialogues.GetFiles("*.txt"))
            {
                var dialogueString = e.OpenText();
                var question = dialogueString.ReadLine();
                var indexCorrectAnswer = dialogueString.ReadLine();
                if (question is null || indexCorrectAnswer is null)
                    continue;

                var correctAnswer = int.Parse(indexCorrectAnswer);
                var answers = new List<string>();

                var str = dialogueString.ReadLine();
                while (str != null)
                {
                    answers.Add(str);
                    str = dialogueString.ReadLine();
                }
                listDialogues.Add(new Dialogue(question, answers.ToArray(), correctAnswer));
            }

            return listDialogues;
        }
    }
}
