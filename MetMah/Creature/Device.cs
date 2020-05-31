using MetMah.Additionally;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;

namespace MetMah.Creature
{
    class Device : ICreature
    {
        public Status Status { get; private set; }
        public int LifeTime { get; private set; }
        public List<ICreature> Students { get; private set; }

        public Device()
        {
            LifeTime = 15;
            Students = new List<ICreature>();
        }

        public Move Act(Level level, int x, int y)
        {
            LifeTime -= 1;
            DeactivateStudents(level, new Point(x, y));
            return new Move();
        }

        public Status GetStatus() => Status;

        public bool IsConflict(ICreature conflictedObject) => true;

        public void SetStatus(Status status) => Status = status;

        private void DeactivateStudents(Level level, Point point)
        {
            var radius = 5;
            for (int dx = -radius; dx < radius + 1; dx++)
                for (int dy = -radius; dy < radius + 1; dy++)
                {
                    var x = point.X + dx;
                    var y = point.Y + dy;
                    if (x < 0 || x >= level.Width || y < 0 || y >= level.Height)
                        continue;

                    var creatures = level.GetCreatures(x, y).Where(c => c is Student || c is CleverStudent).ToArray();
                    if (creatures.Length < 1)
                        continue;

                    foreach (var e in creatures)
                    {
                        if (e.GetStatus() == Status.Inactive)
                            continue;

                        Students.Add(e);
                        e.SetStatus(Status.Inactive);
                    }
                }
        }
    }
}
