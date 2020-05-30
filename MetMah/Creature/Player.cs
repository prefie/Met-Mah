using MetMah.Additionally;
using System.Linq;
using System.Windows.Forms;

namespace MetMah.Creature
{
    public class Player : ICreature
    {
        public Status Status { get; private set; }
        public readonly int PlayerNumber;
        private int numberBeer;
        private int timeIgnor;

        public Player(int numberPlayer)
        {
            PlayerNumber = numberPlayer;
        }

        public Move Act(Level level, int x, int y)
        {
            if (PlayerNumber == 1 && timeIgnor > 0)
            {
                timeIgnor -= 1;
                if (timeIgnor == 0)
                    Status = Status.Active;
            }

            if (level.KeyPressed == Keys.R)
            {
                if (PlayerNumber == 0 && numberBeer > 0 && !level.CheckCreature(x, y, typeof(Python)))
                {
                    level.AddCreature(x, y, new Python());
                    numberBeer -= 1;
                }

                if (PlayerNumber == 1 && numberBeer > 0)
                {
                    Status = Status.Inactive;
                    timeIgnor = 15;
                }

                if (PlayerNumber == 3 && numberBeer > 0 && !level.CheckCreature(x, y, typeof(Device)))
                {
                    level.AddCreature(x, y, new Device());
                    numberBeer -= 1;
                }
            }

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

        public bool IsConflict(ICreature conflictedObject)
        {
            if (conflictedObject is Beer)
                numberBeer += 1;

            return false;
        }

        public void SetStatus(Status status) => Status = status;
    }
}