using MetMah.Additionally;

namespace MetMah.Creature
{
    public class Python : ICreature
    {
        public Status Status { get; private set; }
        public Move Act(Level level, int x, int y) => new Move();

        public Status GetStatus() => Status;

        public bool IsConflict(ICreature conflictedObject) =>
            conflictedObject is Student || conflictedObject is CleverStudent;

        public void SetStatus(Status status) => Status = status;
    }
}
