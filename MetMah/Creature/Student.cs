using MetMah.Additionally;
using System;
using System.Linq;

namespace MetMah.Creature
{
    public class Student : ICreature
    {
        public Status Status { get; private set; }
        public readonly Dialogue Dialogue;
        private static readonly Random random = new Random();

        public Student(Dialogue dialogue)
        {
            Dialogue = dialogue;
            Status = Status.Active;
        }

        public Status GetStatus() => Status;

        public Move Act(Level level, int x, int y)
        {
            if (y + 1 <= level.Height - 1 &&
                !(level.CheckCreature(x, y + 1, typeof(Terrain)) ||
                level.CheckCreature(x, y + 1, typeof(Stairs))))
                return new Move { DeltaY = 1 };
            if (Status == Status.Inactive)
                return new Move();
            var next = random.Next(-1, 2);
            if (x + next >= 0 && x + next <= level.Width - 1 &&
                !level.CheckCreature(x + next, y, typeof(Terrain)) &&
                (level.CheckCreature(x + next, y + 1, typeof(Terrain)) ||
                level.CheckCreature(x + next, y + 1, typeof(Stairs))))
                return new Move { DeltaX = next };
            return new Move();
        }

        public bool IsConflict(ICreature conflictedObject)
        {
            if (Status == Status.Active &&
                (conflictedObject is Player ||
                (conflictedObject is Python && conflictedObject.GetStatus() == Status.Active)))
            {
                if (conflictedObject is Python)
                    conflictedObject.SetStatus(Status.Inactive);
                Status = Status.Inactive;
                return true;
            }

            return false;
        }

        public void SetStatus(Status status) => Status = status;
    }
}
