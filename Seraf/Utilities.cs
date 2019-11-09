using System;

namespace Seraf
{
    public static class Utilities
    {
        public static void GetDirection2D(float x1, float y1, float x2, float y2, out float x, out float y)
        {
            float val1 = 1.0f / (float)Math.Sqrt((x1 * x1) + (y1 * y1));
            x1 *= val1;
            y1 *= val1;

            float val2 = 1.0f / (float)Math.Sqrt((x2 * x2) + (y2 * y2));
            x2 *= val2;
            y2 *= val2;

            x = x2 - x1;
            y = y2 - y1;
        }

        public static float Distance2D(float x1, float y1, float x2, float y2)
        {
            float v1 = x1 - x2, v2 = y1 - y2;
            return (float)Math.Sqrt((v1 * v1) + (v2 * v2));
        }

        public static float Length2D(float x, float y)
        {
            return (float)Math.Sqrt((x * x) + (y * y));
        }

        public static void Normalize2D(float x, float y, out float nx, out float ny)
        {
            nx = x;
            ny = y;
            float val = 1.0f / (float)Math.Sqrt((x * x) + (y * y));
            nx *= val;
            ny *= val;
        }

        public static void sqrt2D(float x, float y, out float sqrt_x, out float sqrt_y)
        {
            sqrt_x = (float)Math.Sqrt(x);
            sqrt_y = (float)Math.Sqrt(y);
        }
    }
}
