using MetMah.Additionally;
using System.Linq;
using System.Windows.Forms;

namespace MetMah.Creature
{
    public class Player : ICreature
    {
        public Status Status { get; private set; }

        public Move Act(Level level, int x, int y)
        {
            if (level.KeyPressed == Keys.R && !level.CheckCreature(x, y, typeof(Python)))
                level.AddCreature(x, y, new Python());
            if (y + 1 <= level.Height - 1 &&
                !(level.CheckCreature(x, y + 1, typeof(Terrain)) ||
                level.CheckCreature(x, y + 1, typeof(Stairs))))
                return new Move { DeltaY = 1 };

            if (level.KeyPressed == Keys.Up && y - 1 >= 0 &&
                level.CheckCreature(x, y, typeof(Stairs)) &&
                !level.CheckCreature(x, y - 1, typeof(Terrain)))
                return new Move { DeltaY = -1 };

            if (level.KeyPressed == Keys.Down && y + 1 <= level.Height - 1 &&
                level.CheckCreature(x, y + 1, typeof(Stairs)))
                return new Move { DeltaY = 1 };

            if (level.KeyPressed == Keys.Left && x - 1 >= 0 &&
                !level.CheckCreature(x - 1, y, typeof(Terrain)))
                return new Move { DeltaX = -1 };

            if (level.KeyPressed == Keys.Right && x + 1 <= level.Width - 1 &&
                !level.CheckCreature(x + 1, y, typeof(Terrain)))
                return new Move { DeltaX = 1 };

            return new Move();
        }

        public Status GetStatus() => Status;

        public bool IsConflict(ICreature conflictedObject) =>
            conflictedObject is Student ||
            conflictedObject is CleverStudent ||
            conflictedObject is Beer;

        public void SetStatus(Status status) => Status = status;
    }
}