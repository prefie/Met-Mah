using MetMah.Additionally;

namespace MetMah.Creature
{
    public interface ICreature
    {
        void SetStatus(Status status);
        Status GetStatus();
        bool IsConflict(ICreature conflictedObject);
        Move Act(Level level, int x, int y);
    }
}
