using MetMah.Additionally;
using System.Drawing;

namespace MetMah.Creature
{
    public class CleverStudent : ICreature
    {
        public Status Status { get; private set; }
        public readonly Dialogue Dialogue;

        public CleverStudent(Dialogue dialogue)
        {
            Dialogue = dialogue;
            Status = Status.Active;
        }

        public Status GetStatus() => Status;

        private static Point? FindPlayer(Level level)
        {
            Point? coordinatePlayer = null;
            for (int x = 0; x < level.Width; x++)
                for (int y = 0; y < level.Height; y++)
                    if (level.CheckCreature(x, y, typeof(Player)))
                        coordinatePlayer = new Point(x, y);
            return coordinatePlayer;
        }

        public Move Act(Level level, int x, int y)
        {
            if (y + 1 <= level.Height - 1 &&
                !(level.CheckCreature(x, y + 1, typeof(Terrain)) ||
                level.CheckCreature(x, y + 1, typeof(Stairs))))
                return new Move { DeltaY = 1 };
            if (Status == Status.Inactive)
                return new Move();
            var player = FindPlayer(level);
            if (player is null)
                return new Move();
            var path = Bfs.FindPaths(level.TextInitiallyMap, new Point(x, y), new Point(player.Value.X, player.Value.Y));
            if (path is null)
                return new Move();
            return new Move() { DeltaX = path.Previous.Value.X - x, DeltaY = path.Previous.Value.Y - y };
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
