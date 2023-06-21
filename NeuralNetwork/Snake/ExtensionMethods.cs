using Microsoft.Xna.Framework;
using System;

namespace Snake
{
    public static class ExtensionMethods
    {
        public static int DistanceFrom(this Point a, Point b)
            => (int)Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2));
    }
}
