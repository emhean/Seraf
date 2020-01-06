using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Seraf.XNA
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

        public static void ShuffleList<T>(List<T> list)
        {
            int rnd_index1 = 0, rnd_index2 = 0;
            T temp;

            for(int n = 0; n < list.Count * 10; ++n) // iterations
            {
                for (int i = 0; i < list.Count; ++i) // index
                {
                    rnd_index1 = Rng.GetInt(0, list.Count);

                    // We assign temp variable before getting the next random,
                    //  - to do some cycles before getting next random to avoid getting two identical indexes
                    temp = list[rnd_index1]; // Used when swapping two elements.

                    while(rnd_index2 == rnd_index1) // To avoid same index
                    {
                        rnd_index2 = Rng.GetInt(0, list.Count);
                    }

                    list[rnd_index1] = list[rnd_index2];
                    list[rnd_index2] = temp;
                }
            }
        }

        public static byte GetByte(int min = 0, int max = 255)
        {
            return (byte)rnd.Next(min, max);
        }

        /// <summary>
        /// Gets a random value. GetInt(0,3) will return anything from 0 to 2.
        /// </summary>
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

        /// <summary>
        /// Rolls a dice of a random number between 1-6. 
        /// </summary>
        public static bool RollDice6()
        {
            return (rnd.Next(1, 7) == rnd.Next(1, 7)); // This is 1-6. Max is exclusive.
        }


        /// <summary>
        /// Rolls a dice, returns true if given number matches.
        /// </summary>
        /// <param name="number">The wanted dice number between 1-6. </param>
        /// <returns></returns>
        public static bool RollDice6(int number)
        {
            if (number < 0 || number > 6)
                throw new Exception("Number is outside the range!");

            return (rnd.Next(1, 7) == number); // This is 1-6. Max is exclusive.
        }

        /// <summary>
        /// Get percentage chance.
        /// </summary>
        public static bool GetChance(int percent)
        {
            // Will return true if percent is bigger than 0-99. 
            // If percent is 100 it will always be bigger because max value is excluded in rnd.Next().
            return rnd.Next(0, 100) <= percent;
        }

        /// <summary>
        /// Get percentage chance with percentage modifier.
        /// </summary>
        public static bool GetChance(int percent, int percentageMod = 100)
        {
            // Will return true if percent is bigger than 0-99. 
            // If percent is 100 it will always be bigger because max value is excluded in rnd.Next().
            return rnd.Next(0, percentageMod) <= percent;
        }
    }
}
