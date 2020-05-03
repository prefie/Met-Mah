using MetMah.Additionally;

namespace MetMah.Creature
{
    public class Beer : ICreature
    {
        public Status Status { get; private set; }

        public Move Act(Level level, int x, int y) => new Move();

        public Status GetStatus() => Status;

        public bool IsConflict(ICreature conflictedObject) => conflictedObject is Player;

        public void SetStatus(Status status) => Status = status;
    }
}
