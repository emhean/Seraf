namespace Seraf.Collision
{
    public class Circle
    {
        /// <summary>
        /// Position.
        /// </summary>
        public float px, py;

        /// <summary>
        /// Acceleration.
        /// </summary>
        public float ax, ay;

        /// <summary>
        /// Velocity.
        /// </summary>
        public float vx, vy;

        public float radius;

        public Circle(float px, float py, float radius)
        {
            this.px = px;
            this.py = py;
            this.radius = radius;

            this.vx = 0;
            this.vy = 0;
            this.ax = 0;
            this.ay = 0;
        }

        public float GetDiameter()
        {
            return radius * 2;
        }

        public float GetCircumference()
        {
            return (float)System.Math.PI * (radius * 2);
        }

        public float GetArea()
        {
            return (float)System.Math.PI * (radius * radius);
        }
    }
}
