using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Seraf.XNA
{
    public class Camera2DControlled : Camera2D
    {
        KeyboardState keyboardState; // Keyboard State

        public void UpdateControls(float delta)
        {
            keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.OemPeriod))
            {
                _zoom += delta * 4;
            }
            else if (keyboardState.IsKeyDown(Keys.OemMinus))
            {
                _zoom -= delta * 4;
            }

            if (keyboardState.IsKeyDown(Keys.M))
            {
                _rotation += delta;
            }
            else if (keyboardState.IsKeyDown(Keys.N))
            {
                _rotation -= delta;
            }

            if (keyboardState.IsKeyDown(Keys.Left))
            {
                _pos.X -= delta * 100;
            }
            else if (keyboardState.IsKeyDown(Keys.Right))
            {
                _pos.X += delta * 100;
            }

            if (keyboardState.IsKeyDown(Keys.Up))
            {
                _pos.Y -= delta * 100;
            }
            else if (keyboardState.IsKeyDown(Keys.Down))
            {
                _pos.Y += delta * 100;
            }
        }
    }
}
