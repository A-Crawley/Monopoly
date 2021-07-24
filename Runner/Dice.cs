using System;

namespace Runner
{
    public static class Dice
    {
        private static readonly Random Random = new ();

        public static int Roll()
            => Random.Next(6) + 1;
    }
}