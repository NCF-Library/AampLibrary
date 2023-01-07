using System.Globalization;

namespace AampLibrary.Numarics
{
    public struct Curve3 : IEquatable<Curve3>, IFormattable
    {
        public Curve X;
        public Curve Y;
        public Curve Z;

        public Curve3(Curve x, Curve y, Curve z)
        {
            X = x;
            Y = y;
            Z = x;
        }

        public static bool operator ==(Curve3 a, Curve3 b)
        {
            return a.Equals(ref b);
        }

        public static bool operator !=(Curve3 a, Curve3 b)
        {
            return !a.Equals(ref b);
        }

        public override bool Equals(object? obj)
        {
            if (obj is Curve3 curve) {
                return Equals(ref curve);
            }

            return false;
        }

        public bool Equals(Curve3 other)
        {
            return Equals(ref other);
        }

        public bool Equals(ref Curve3 other)
        {
            return X == other.X && Y == other.Y && Z == other.Z;
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
            return X.GetHashCode() * 17 + Y.GetHashCode() * 17 + Z.GetHashCode();
        }
    }
}
