using System;

namespace BlockVanity
{
    public static class Easing
    {
        //https://easings.net/

        public static float CircularIn(float x) => 1f - (float)Math.Sqrt(1f - Math.Pow(x, 2));

        public static float CircularOut(float x) => (float)Math.Sqrt(1f - Math.Pow(x, 2));

        public static float CircularInOut(float x) => x < 0.5f ? (1f - (float)Math.Sqrt(1f - (float)Math.Pow(2f * x, 2))) / 2f : ((float)Math.Sqrt(1f - (float)Math.Pow(-2f * x + 2, 2)) + 1f) / 2f;

        public static float BackIn(float x)
        {
            const float c1 = 1.70158f;
            const float c2 = c1 + 1f;

            return c2 * (float)Math.Pow(x, 3) - c1 * (float)Math.Pow(x, 2);
        }        

        public static float BackOut(float x)
        {
            const float c1 = 1.70158f;
            const float c2 = c1 + 1f;

            return 1f + c2 * (float)Math.Pow(x - 1f, 3) + c1 * (float)Math.Pow(x - 1f, 2);
        }

        public static float BackInOut(float x) => x < 0.5f ? BackIn(x) : BackOut(x);

        public static float BounceIn(float x)
        {
            const float n1 = 7.5625f;
            const float d1 = 2.75f;

            if (x < 1f / d1)
                return n1 * (float)Math.Pow(x, 2);
            else if (x < 2f / d1)
                return n1 * (x -= 1.5f / d1) * x + 0.75f;
            else if (x < 2.5f / d1)
                return n1 * (x -= 2.25f / d1) * x + 0.9375f;
            else
                return n1 * (x -= 2.625f / d1) * x + 0.984375f;
        }

        public static float BounceOut(float x) => 1f - BounceIn(1f - x);

        public static float BounceInOut(float x) => x < 0.5f ? (1f - BounceIn(1f - 2f * x)) / 2f : (1f + BounceIn(2f * x - 1f)) / 2f;
    }
}
