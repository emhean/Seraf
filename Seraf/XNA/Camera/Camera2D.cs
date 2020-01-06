using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Seraf.XNA
{
    public class Camera2D
    {
        protected Matrix _transform; // Matrix Transform
        protected Vector2 _pos; // Camera Position
        protected float _zoom; // Camera Zoom
        protected float _rotation; // Camera Rotation

        public Camera2D()
        {
            _zoom = 1.0f;
            _rotation = 0.0f;
            _pos = Vector2.Zero;
        }

        public Camera2D(Vector2 position)
        {
            _zoom = 1.0f;
            _rotation = 0.0f;
            _pos = position;
        }

        /// <summary>
        /// The zoom value, default value 1. Minimum 0.1f.
        /// </summary>
        public float Zoom
        {
            get { return _zoom; }
            set { _zoom = value; if (_zoom < 0.1f) _zoom = 0.1f; }
        }

        /// <summary>
        /// The rotation of the camera.
        /// </summary>
        public float Rotation
        {
            get { return _rotation; }
            set { _rotation = value; }
        }

        /// <summary>
        /// Move the camera by an amount.
        /// </summary>
        public void Move(Vector2 amount)
        {
            _pos += amount;
        }

        /// <summary>
        /// The position of the camera.
        /// </summary>
        public Vector2 Position
        {
            get { return _pos; }
            //set { _pos.X = value.X; _pos.Y = value.Y; }
            set { _pos = value; }
        }

        public Matrix GetTransformation(GraphicsDevice graphicsDevice)
        {
            _transform = Matrix.CreateTranslation(new Vector3(-_pos.X, -_pos.Y, 0)) *
                                         Matrix.CreateRotationZ(_rotation) *
                                         Matrix.CreateScale(new Vector3(_zoom, _zoom, 1)) *
                                         Matrix.CreateTranslation(new Vector3(graphicsDevice.Viewport.Width * 0.5f, graphicsDevice.Viewport.Height * 0.5f, 0));
            return _transform;
        }

        // TODO: Do at home.
        //public Vector2 GetVector2Transformation(Vector2 vector2)
        //{

        //    Matrix.CreateTranslation()
        //}
    }
}
