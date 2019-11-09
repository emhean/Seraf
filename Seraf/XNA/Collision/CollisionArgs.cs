using Microsoft.Xna.Framework;

namespace Seraf.XNA.Collision
{
    public class CollisionArgs
    {
        public COLLISION_SIDE Side { get; }

        public CollisionArgs(COLLISION_SIDE side)
        {
            Side = side;
        }
    }
}
