using System;
using System.Threading;

namespace Satisfy.Utility
{
    public static class StaticRandom
    {
        private static int seed;

        private static ThreadLocal<System.Random> threadLocal = new ThreadLocal<System.Random>
                        (() => new System.Random(Interlocked.Increment(ref seed)));

        static StaticRandom() => seed = Environment.TickCount;

        public static System.Random Instance { get { return threadLocal.Value; } }
    }
}