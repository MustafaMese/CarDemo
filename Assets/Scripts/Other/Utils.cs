using UnityEngine;

namespace Other
{
    public static class Utils
    {
        public static float GetPercentage(float max, float value)
        {
            return value * 100f / max;
        }

        public static float GetValue(float max, float percentage)
        {
            return max * percentage / 100f;
        }

        public static float LerpValue(float startValue, float endValue, float delta)
        {
            return Mathf.Lerp(startValue, endValue, delta);
        }
        
        public static int LerpValue(int startValue, int endValue, int delta)
        {
            return (int)Mathf.Lerp(startValue, endValue, delta);
        }

        public static float ClampValue(float value, float min, float max)
        {
            return Mathf.Clamp(value, min, max);
        }

        public static float GetFrameDuration() => Time.deltaTime;

        public static float GetTime() => Time.time;
    }
}