using MetMah.Additionally;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetMah.Creature
{
    class Door : ICreature
    {
        public Status Status { get; private set; }

        public Door()
        {
            Status = Status.Inactive;
        }

        public Move Act(Level level, int x, int y)
        {
            if (level.IsOver)
                Status = Status.Active;

            return new Move();
        }

        public Status GetStatus() => Status;

        public bool IsConflict(ICreature conflictedObject) => conflictedObject is Player;

        public void SetStatus(Status status) => Status = status;
    }
}
