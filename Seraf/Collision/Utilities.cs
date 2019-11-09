namespace Seraf.Collision
{
    public class Utilities
    {
        /// <summary>
        /// Check if two cirles overlap.
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="r1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <param name="r2"></param>
        /// <returns></returns>
        public bool DoCirclesOverlap(float x1, float y1, float r1, float x2, float y2, float r2)
        {
            return System.Math.Abs(
                (x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2)) <= (r1 + r2) * (r1 + r2);
        }

        /// <summary>
        /// Check if x and y poitions is inside a circle.
        /// </summary>
        /// <param name="x1">X position to check.</param>
        /// <param name="y1">Y position to check.</param>
        /// <param name="r1">Radius of position.</param>
        /// <param name="px">X position of circle.</param>
        /// <param name="py">Y position of circle.</param>
        /// <returns></returns>
        public bool IsPointInCircle(float x1, float y1, float r1, float px, float py)
        {
            return System.Math.Abs(
                (x1 - px) * (x1 - px) + (y1 - py) * (y1 - py)) < (r1 * r1);
        }
    }
}
