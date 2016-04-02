using System;
using System.Numerics;

namespace IteratedPrisonerDilemma2
{
    public class GlobalRandom
    {
        private Random random;
        private static GlobalRandom instance;
        private GlobalRandom ()
        {
            this.random = new Random (System.DateTime.Now.Millisecond);
        }

        public static GlobalRandom Instance() {
            if (instance == null) {
                instance = new GlobalRandom ();

            }
            return instance;
        }
        public double NextDouble() {
            return this.random.NextDouble();
        }
        public float NextFloat() {
            return (float)this.random.NextDouble ();
        }
        public int NextInt() {
            return this.random.Next();
        }
        public int NextInt(int maxValue) {
            return this.random.Next(maxValue);
        }
    }
}



