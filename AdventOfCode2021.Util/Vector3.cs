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

    }
}