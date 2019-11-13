using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;

namespace Seraf.XNA
{
    public class Camera2DCutscene
    {
        public Camera2D Camera2D { get; set; }
        public bool Finished { get; private set; } // If fully finished.
        public Vector2 target_pos;
        Vector2 dir;

        float t; // Time
        bool there; // If destination reached but not fully finished.

        public Camera2DCutscene(Camera2D camera, Vector2 target_position)
        {
            this.Camera2D = camera;
            this.Finished = false;
            this.target_pos = target_position;
            this.dir = Vector2.Normalize(Vector2.Subtract(target_pos, camera.Position));
            this.t = 0f;
            this.there = false;
        }

        public event EventHandler DestinationReached;
        private void OnDestinationReached()
        {
            DestinationReached?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler CameraCutsceneFinished;
        private void OnCameraCutsceneFinished()
        {
            CameraCutsceneFinished?.Invoke(this, EventArgs.Empty);
        }

        public void UpdatePosition(float delta)
        {
            if (there)
            {
                t += delta;

                if (t > 2)
                {
                    Finished = true;
                    OnCameraCutsceneFinished();
                }
            }
            else
            {
                there = IsThere();
                Camera2D.Position += (dir * 100) * delta;
            }
        }


        public Vector2 GetDirection() => Vector2.Normalize(Vector2.Subtract(target_pos, Camera2D.Position));

        private bool IsThere()
        {
            var v = (Vector2.Subtract(new Vector2(
                (int)Camera2D.Position.X,
                (int)Camera2D.Position.Y),
                new Vector2((int)target_pos.X, (int)target_pos.Y)));

            bool x = v.X < 1;
            bool y = v.Y < 1;

            bool foo = (x == y && x && y);
            if (foo)
            {
                OnDestinationReached();
            }

            return foo;
        }
    }
}