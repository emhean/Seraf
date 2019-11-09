namespace Seraf.AI
{
    public enum STATE_FLOCK
    {
        STATE_FOLLOWER,
        STATE_LEADER
    }

    /// <summary>
    /// Flock AI.
    /// </summary>
    public class Flock
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

        public Flock leader;

        public Flock(float px, float py, Flock leader, float followDistance)
        {
            this.px = px;
            this.py = py;
            this.leader = leader;
            this.followDistance = followDistance;
            this.t_check = 0.01f;
        }

        public Flock(float px, float py)
        {
            this.px = px;
            this.py = py;
            this.t_check = 0.01f;
        }


        public STATE_FLOCK FlockState { get; protected set; }

        public void Update(float delta)
        {
            t += delta; // Add time.

            if(t > t_check)
            {
                if (leader != null)
                {
                    currentDistance = Utilities.Distance2D(px, py, leader.px, leader.py);

                    if (currentDistance > followDistance) // If true then leader is too far away so we'll start following
                    {
                        Utilities.GetDirection2D(leader.px, leader.py, px, py, out float dir_x, out float dir_y);

                        // Get remaining value of distances to move 1px instead of speed to smooth it out.
                        float rem = currentDistance - followDistance;

                        if (rem < 4) // Move remaining
                        {
                            px += rem * dir_x;
                            py += rem * dir_y;
                        }
                        else // Move at normal speed
                        {
                            px += 4 * dir_x;
                            py += 4 * dir_y;
                        }

                        FlockState = STATE_FLOCK.STATE_FOLLOWER;
                    }
                    else // Leader is null therefore its a leader.
                        FlockState = STATE_FLOCK.STATE_LEADER; 
                }

                t = 0; // Reset time.
            }
        }
    }
}
