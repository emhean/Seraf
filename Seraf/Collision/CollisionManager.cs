using System;
using System.Collections.Generic;
using System.Text;

namespace Seraf.Collision
{
    public class CollisionManager
    {
        List<Circle> circles;
        List<CollisionResult> collisions;

        public CollisionManager()
        {
            this.circles = new List<Circle>();
            this.collisions = new List<CollisionResult>();
        }

        /// <summary>
        /// Add a circle to the manager.
        /// </summary>
        public void AddCircle(Circle circle)
        {
            this.circles.Add(circle);
            //return circles.Count;
        }
        //public int AddCircle(Circle circle, bool pushOthers)
        //{
        //    this.circles.Add(circle);
        //    this.coll_push_flags.Add(pushOthers);
        //    return circles.Count;
        //}

        bool DoCirclesOverlap(float x1, float y1, float r1, float x2, float y2, float r2)
        {
            return System.Math.Abs(
                (x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2)) <= (r1 + r2) * (r1 + r2);
        }

        bool IsPointInCircle(float x1, float y1, float r1, float px, float py)
        {
            return System.Math.Abs(
                (x1 - px) * (x1 - px) + (y1 - py) * (y1 - py)) < (r1 * r1);
        }

        void UpdatePosition(int index, float delta)
        {
            var circle = circles[index];

            // Add Drag to emulate rolling friction
            circle.ax = -circle.vx * 0.8f;
            circle.ay = -circle.vy * 0.8f;

            // Update ball physics
            circle.vx += circle.ax * delta;
            circle.vy += circle.ay * delta;
            circle.px += circle.vx * delta;
            circle.py += circle.vy * delta;

            //// Wrap the balls around screen
            //if (ball.px < 0) ball.px += (float)ScreenWidth();
            //if (ball.px >= ScreenWidth()) ball.px -= (float)ScreenWidth();
            //if (ball.py < 0) ball.py += (float)ScreenHeight();
            //if (ball.py >= ScreenHeight()) ball.py -= (float)ScreenHeight();

            // Clamp velocity near zero
            if (System.Math.Abs(circle.vx * circle.vx + circle.vy * circle.vy) < 0.01f)
            {
                circle.vx = 0;
                circle.vy = 0;
            }
        }

        void UpdateDynamicCollision()
        {
            for (int i = 0; i < collisions.Count; ++i)
            {
                // Distance between balls
                var fDistance = (float)System.Math.Sqrt((collisions[i].Collider.px - collisions[i].CollidingWith.px) * (collisions[i].Collider.px - collisions[i].CollidingWith.px) + (collisions[i].Collider.py - collisions[i].CollidingWith.py) * (collisions[i].Collider.py - collisions[i].CollidingWith.py));

                // Normal
                float nx = (collisions[i].CollidingWith.px - collisions[i].Collider.px) / fDistance;
                float ny = (collisions[i].CollidingWith.py - collisions[i].Collider.py) / fDistance;

                // Tangent
                float tx = -ny;
                float ty = nx;

                // Dot Product Tangent
                float dpTan1 = collisions[i].Collider.vx * tx + collisions[i].Collider.vy * ty;
                float dpTan2 = collisions[i].CollidingWith.vx * tx + collisions[i].CollidingWith.vy * ty;

                // Dot Product Normal
                float dpNorm1 = collisions[i].Collider.vx * nx + collisions[i].Collider.vy * ny;
                float dpNorm2 = collisions[i].CollidingWith.vx * nx + collisions[i].CollidingWith.vy * ny;

                // Conservation of momentum in 1D
                float mass_i = collisions[i].Collider.GetArea() ;
                float mass_j = collisions[i].CollidingWith.GetArea() ;

                float m1 = (dpNorm1 * (-mass_j) + 2.0f * mass_j * dpNorm2) / (mass_i + mass_j);
                float m2 = (dpNorm2 * (mass_j - mass_i) + 2.0f * mass_i * dpNorm1) / (mass_i + mass_j);

                // Update ball velocities
                collisions[i].Collider.vx = tx * dpTan1 + nx * m1;
                collisions[i].Collider.vy = ty * dpTan1 + ny * m1;
                collisions[i].CollidingWith.vx = tx * dpTan2 + nx * m2;
                collisions[i].CollidingWith.vy = ty * dpTan2 + ny * m2;

                // Wikipedia Version - Maths is smarter but same
                //float kx = (b1->vx - b2->vx);
                //float ky = (b1->vy - b2->vy);
                //float p = 2.0 * (nx * kx + ny * ky) / (b1->mass + b2->mass);
                //b1->vx = b1->vx - p * b2->mass * nx;
                //b1->vy = b1->vy - p * b2->mass * ny;
                //b2->vx = b2->vx + p * b1->mass * nx;
                //b2->vy = b2->vy + p * b1->mass * ny;

                collisions.Remove(collisions[i]);
            }

        }

        public delegate CollisionResult Collision(object sender, CollisionResult result);
        public event Collision CollisionEnter;
        public event Collision CollisionLeave;

        protected void OnCollisionEnter(CollisionResult result)
        {
            CollisionEnter?.Invoke(this, result);
        }

        protected void OnCollisionLeave(CollisionResult result)
        {
            CollisionLeave?.Invoke(this, result);
        }

        //public CollisionResult PushCollider(object sender, CollisionResult result)
        //{
        //    // Displace Current Ball away from collision
        //    result.Collider.px -= result.Overlap * (result.Collider.px - result.ColldingWith.px) / result.Distance;
        //    result.Collider.py -= result.Overlap * (result.Collider.py - result.ColldingWith.py) / result.Distance;


        //    return result;
        //}
        //public CollisionResult PushColliding(object sender, CollisionResult result)
        //{
        //    // Displace Target Ball away from collision
        //    result.ColldingWith.px += result.Overlap * (result.Collider.px - result.ColldingWith.px) / result.Distance;
        //    result.ColldingWith.py += result.Overlap * (result.Collider.py - result.ColldingWith.py) / result.Distance;

        //    return result;
        //}

        public void Update(float delta)
        {
            for (int i = 0; i < circles.Count; ++i)
            {
                this.UpdatePosition(i, delta);

                for (int j = 0; j < circles.Count; ++j)
                {

                    if (i != j) // To avoid checking collision on itself
                    {
                        if (DoCirclesOverlap(circles[i].px, circles[i].py, circles[i].radius, circles[j].px, circles[j].py, circles[j].radius))
                        {
                            // Collision has occured
                            // We calculate distance between the two and the intersection (overlap)
                            // Then we add it to the list of collisions.

                            // Distance between ball centers
                            var fDistance = (float)System.Math.Sqrt((circles[i].px - circles[j].px) * (circles[i].px - circles[j].px) + (circles[i].py - circles[j].py) * (circles[i].py - circles[j].py));

                            // Calculate displacement required
                            var fOverlap = 0.5f * (fDistance - circles[i].radius - circles[j].radius);

                            // Add collision to the list 
                            var result = new CollisionResult(circles[i], circles[j], fDistance, fOverlap);
                            collisions.Add(result);

                            // Invoke collision event
                            OnCollisionEnter(result);

                            //// Displace Current Ball away from collision
                            circles[i].px -= fOverlap * (circles[i].px - circles[j].px) / fDistance;
                            circles[i].py -= fOverlap * (circles[i].py - circles[j].py) / fDistance;

                            // Displace Target Ball away from collision
                            circles[j].px += fOverlap * (circles[i].px - circles[j].px) / fDistance;
                            circles[j].py += fOverlap * (circles[i].py - circles[j].py) / fDistance;

                            // Invoke collision event
                            OnCollisionLeave(result);
                        }
                    }
                }


                this.UpdateDynamicCollision();
            }
        }
    }
}
