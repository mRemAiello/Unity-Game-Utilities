namespace GameUtils
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
    }
}