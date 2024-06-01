using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Arbinty.Utils
{
    public static class MathUtility
    {
        public static float Normalize(float value, float min, float max)
        {
            return (value - min) / (max - min);
        }

        public static float SmoothTime(float t)
        {
            return t * t * (3f - 2f * t);
        }

        public static int GenerateSeed()
        {
            int seed = Random.Range(0, int.MaxValue);
            seed ^= (int)DateTime.Now.Ticks;
            seed ^= (int)Time.unscaledTime;
            seed &= int.MaxValue;

            return seed;
        }
    }
}