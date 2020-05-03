using MetMah.Creature;
using System.Drawing;

namespace MetMah.Additionally
{
    public class CreatureAction
    {
        public Move Command;
        public ICreature Creature;
        public Point Location;
        public Point TargetLogicalLocation;
    }
}
