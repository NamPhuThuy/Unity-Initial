using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UCExtension
{
    public static class RandomExtension
    {
        public static bool Bool(float ratio)
        {
            ratio = Mathf.Clamp(ratio, 0.0001f, 1f);
            float randomValue = UnityEngine.Random.Range(0, 1f);
            return randomValue < ratio;
        }
    }
}
