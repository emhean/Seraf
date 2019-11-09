using System.Collections.Generic;

namespace Seraf.AI.Flock
{
    /// <summary>
    /// The leader of a flock.
    /// </summary>
    public class FlockLeader
    {
        List<FlockFollower> followers;
        public float px, py;

        public FlockLeader(float px, float py)
        {
            this.px = px;
            this.py = py;
        }

        public void Update(float delta)
        {
        }
    }
}