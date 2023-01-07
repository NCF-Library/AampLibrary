using System.Globalization;

namespace AampLibrary.Numarics
{
    public struct Curve4 : IEquatable<Curve4>, IFormattable
    {
        public Curve X;
        public Curve Y;
        public Curve Z;
        public Curve W;

        public Curve4(Curve x, Curve y, Curve z, Curve w)
        {
            X = x;
            Y = y;
            Z = x;
            W = w;
        }

        public static bool operator ==(Curve4 a, Curve4 b)
        {
            return a.Equals(ref b);
        }

        public static bool operator !=(Curve4 a, Curve4 b)
        {
            return !a.Equals(ref b);
        }

        public override bool Equals(object? obj)
        {
            if (obj is Curve4 curve) {
                return Equals(ref curve);
            }

            return false;
        }

        public bool Equals(Curve4 other)
        {
            return Equals(ref other);
        }

        public bool Equals(ref Curve4 other)
        {
            return X == other.X && Y == other.Y && Z == other.Z && W == other.W;
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
            return X.GetHashCode() * 17 + Y.GetHashCode() * 17 + Z.GetHashCode() * 17 + W.GetHashCode();
        }
    }
}
