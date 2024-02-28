// ----------------------------------------------------------------------------------------------------------
// Author: Austin Peel
//
// © All rights reserved. ECOLE POLYTECHNIQUE FEDERALE DE LAUSANNE, Switzerland, Section de Physique, 2024.
// See the LICENSE.md file for more details.
// ----------------------------------------------------------------------------------------------------------
﻿using UnityEngine;

namespace Utils
{
    public static class Random
    {
        /// <summary>
        /// Marsaglia polar method for sampling from a Gaussian distribution
        /// </summary>
        /// <param name="mu"></param>
        /// <param name="sigma"></param>
        /// <returns></returns>
        public static float NormalValue(float mu, float sigma)
        {
            float x1, x2, s;
            do
            {
                x1 = 2f * UnityEngine.Random.value - 1f;
                x2 = 2f * UnityEngine.Random.value - 1f;
                s = x1 * x1 + x2 * x2;
            } while (s == 0f || s >= 1f);

            s = Mathf.Sqrt(-2f * Mathf.Log(s) / s);

            return mu + x1 * s * sigma;
        }

        /// <summary>
        /// Box-Muller algorithm for drawing a sample from a Gaussian distribution
        /// </summary>
        /// <param name="mu"></param>
        /// <param name="sigma"></param>
        /// <returns></returns>
        public static float NormalValueBoxMuller(float mu, float sigma)
        {
            float u1 = UnityEngine.Random.value;
            float u2 = UnityEngine.Random.value;
            float z0 = Mathf.Sqrt(-2.0f * Mathf.Log(u1)) * Mathf.Cos(2.0f * Mathf.PI * u2);

            return mu + sigma * z0;
        }
    }
}
