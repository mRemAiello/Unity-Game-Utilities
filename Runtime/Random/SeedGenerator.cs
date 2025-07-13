using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameUtils
{
    public class SeedGenerator
    {
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