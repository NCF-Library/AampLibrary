using System.Globalization;

namespace AampLibrary.Numarics
{
    public struct Curve2 : IEquatable<Curve2>, IFormattable
    {
        public Curve X;
        public Curve Y;

        public Curve2(Curve x, Curve y)
        {
            X = x;
            Y = y;
        }

        public static bool operator ==(Curve2 a, Curve2 b)
        {
            return a.Equals(ref b);
        }

        public static bool operator !=(Curve2 a, Curve2 b)
        {
            return !a.Equals(ref b);
        }

        public override bool Equals(object? obj)
        {
            if (obj is Curve2 curve) {
                return Equals(ref curve);
            }

            return false;
        }

        public bool Equals(Curve2 other)
        {
            return Equals(ref other);
        }

        public bool Equals(ref Curve2 other)
        {
            return X == other.X && Y == other.Y;
        }

        public override string ToString()
        {
            return ToString(null, CultureInfo.InvariantCulture);
        }

        public string ToString(string? format, IFormatProvider? formatProvider)
        {
            return string.Format(formatProvider, format ?? "{{X={0}, Y={1}}}", X, Y);
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() * 17 + Y.GetHashCode();
        }
    }
}
