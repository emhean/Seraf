namespace Seraf.Collision
{
    public class CollisionResult
    {
        public CollisionResult(Circle circle1, Circle circle2, float distance, float overlap)
        {
            this.Collider = circle1;
            this.CollidingWith = circle2;
            this.Distance = distance;
            this.Overlap = overlap;
        }
        public Circle Collider { get; }
        public Circle CollidingWith { get; }
        public float Distance { get; }
        public float Overlap { get; }
    }
}
