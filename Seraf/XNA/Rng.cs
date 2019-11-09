using Microsoft.Xna.Framework;
using System;

namespace Engine
{
    /// <summary>
    /// A global Random number generator.
    /// </summary>
    public static class Rng
    {
        static private Random rnd;

        static Rng()
        {
            rnd = new Random();
        }

        public static byte GetByte(int min = 0, int max = 255)
        {
            return (byte)rnd.Next(min, max);
        }

        public static int GetInt(int min, int max)
        {
            return rnd.Next(min, max);
        }

        public static int GetInt_ExcludeZero(int min, int max)
        {
            int value = GetInt(min, max);
            while (value == 0)
            {
                value = GetInt(min, max);
            }
            return value;

            /*
            // Old code:
            int value = rnd.Next(min, max);
            if (value == 0)
            {
                return GetChance(50) ? 1 : -1; // 50%
            }
            else return value;*/
        }

        public static double GetDouble(double minimum, double maximum)
        {
            return rnd.NextDouble() * (maximum - minimum) + minimum;
        }

        public static float GetFloat(float minimum, float maximum)
        {
            return (float)rnd.NextDouble() * (maximum - minimum) + minimum;
        }

        public static Vector2 GetVector2(Vector2 minimum, Vector2 maximum)
        {
            return new Vector2(GetFloat(minimum.X, maximum.X), GetFloat(minimum.Y, maximum.Y) );
        }

        public static bool GetChance(int percent)
        {
            // Will return true if percent is bigger than 0-99. 
            // If percent is 100 it will always be bigger because max value is excluded in rnd.Next().
            return rnd.Next(0, 100) <= percent;
        }

        public static bool GetChance(int percent, int percentageMod = 100)
        {
            // Will return true if percent is bigger than 0-99. 
            // If percent is 100 it will always be bigger because max value is excluded in rnd.Next().
            return rnd.Next(0, percentageMod) <= percent;
        }
    }
}
