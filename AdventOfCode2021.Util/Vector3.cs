using System;

namespace AdventOfCode2021.Util
{
    public record Vector3(int X, int Y, int Z)
    {

        public Vector3 Add(Vector3 other)
        {
            return this with
            {
                X = X + other.X,
                Y = Y + other.Y,
                Z = Z + other.Z
            };
        }

        public Vector3 Sub(Vector3 other)
        {
            return this with
            {
                X = X - other.X,
                Y = Y - other.Y,
                Z = Z - other.Z
            };
        }

        public Vector3 Mult(Vector3 other)
        {
            return this with
            {
                X = X * other.X,
                Y = Y * other.Y,
                Z = Z * other.Z
            };
        }

        public int Manhattan()
        {
            return Math.Abs(X) + Math.Abs(Y) + Math.Abs(Z);
        }
        
        public static Vector3 Origin => new Vector3(0, 0, 0);
    }
}