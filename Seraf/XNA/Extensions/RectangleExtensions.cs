using Microsoft.Xna.Framework;

namespace Seraf.XNA.Extensions
{
    public static class RectangleExtensions
    {
        public static Vector2[] GetCorners(this Rectangle rectangle)
        {
            var corners = new Vector2[4];
            corners[0] = new Vector2(rectangle.Left, rectangle.Top);
            corners[1] = new Vector2(rectangle.Right, rectangle.Top);
            corners[2] = new Vector2(rectangle.Right, rectangle.Bottom);
            corners[3] = new Vector2(rectangle.Left, rectangle.Bottom);
            return corners;
        }
    }
}
