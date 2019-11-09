using System;

namespace Seraf.XNA.Collision
{
    public enum COLLISION_SIDE
    {
        /// <summary>No collision occurred.</summary>
        None = 0,
        /// <summary>Collision occurred at the top side.</summary>
        Top = 1,
        /// <summary>Collision occurred at the bottom side.</summary>
        Bottom = 2,
        /// <summary>Collision occurred at the left side.</summary>
        Left = 4,
        /// <summary>Collision occurred at the right side.</summary>
        Right = 8
    }
}
