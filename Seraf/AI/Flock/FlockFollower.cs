namespace Seraf.AI.Flock
{
    public class FlockFollower
    {
        /// <summary>
        /// The distance variable that decides when to start following (when follower is too far away).
        /// </summary>
        public float followDistance;

        /// <summary>
        /// The current distance from its leader.
        /// </summary>
        public float currentDistance;

        /// <summary>
        /// Intervals when to check.
        /// </summary>
        public float t_check;

        /// <summary>
        /// Time variable.
        /// </summary>
        float t;

        public float px, py;

        FlockLeader leader;

        public FlockFollower(float px, float py, FlockLeader leader)
        {
            this.px = px;
            this.py = py;
            this.leader = leader;
        }

        public void Update(float delta)
        {
            t += delta; // Add time.

            if(t > t_check)
            {
            }
        }
    }
}
