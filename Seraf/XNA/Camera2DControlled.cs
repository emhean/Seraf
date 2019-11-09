using Microsoft.Xna.Framework.Input;

namespace Seraf.XNA
{
    public class Camera2DControlled : Camera2D
    {
        float s = 100; // Speed
        KeyboardState keyboardState; // Keyboard State

        public void Update(float delta)
        {
        }

        public void UpdateControls(float delta)
        {
            s = 100 * delta;
            keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.OemPeriod))
            {
                _zoom += s;
            }
            else if (keyboardState.IsKeyDown(Keys.OemMinus))
            {
                _zoom -= s;
            }

            if (keyboardState.IsKeyDown(Keys.M))
            {
                _rotation += s;
            }
            else if (keyboardState.IsKeyDown(Keys.N))
            {
                _rotation -= s;
            }

            if (keyboardState.IsKeyDown(Keys.Left))
            {
                _pos.X -= s;
            }
            else if (keyboardState.IsKeyDown(Keys.Right))
            {
                _pos.X += s;
            }

            if (keyboardState.IsKeyDown(Keys.Up))
            {
                _pos.Y -= s;
            }
            else if (keyboardState.IsKeyDown(Keys.Down))
            {
                _pos.Y += s;
            }
        }
    }
}
