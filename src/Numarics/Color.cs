using System.Globalization;

namespace AampLibrary.Numarics
{
    public struct Color : IEquatable<Color>, IFormattable
    {
        public byte R;
        public byte G;
        public byte B;
        public byte A;

        public Color(byte red, byte green, byte blue)
        {
            R = red;
            G = green;
            B = blue;
            A = byte.MaxValue;
        }

        public Color(byte red, byte green, byte blue, byte alpha)
        {
            R = red;
            G = green;
            B = blue;
            A = alpha;
        }

        public Color(Color color, byte alpha)
        {
            R = color.R;
            G = color.G;
            B = color.B;
            A = alpha;
        }

        public static bool operator ==(Color a, Color b)
        {
            return a.Equals(ref b);
        }

        public static bool operator !=(Color a, Color b)
        {
            return !a.Equals(ref b);
        }

        public override bool Equals(object? obj)
        {
            if (obj is Color color) {
                return Equals(ref color);
            }

            return false;
        }

        public bool Equals(Color other)
        {
            return Equals(ref other);
        }

        public bool Equals(ref Color other)
        {
            return R == other.R && G == other.G && B == other.B && A == other.A;
        }

        public override string ToString()
        {
            return ToString(null, CultureInfo.InvariantCulture);
        }

        public string ToString(string? format, IFormatProvider? formatProvider)
        {
            return string.Format(formatProvider, format ?? "{{R={0},G={1},B={2},A={3}}}", R, G, B, A);
        }

        public override int GetHashCode()
        {
            return 373 * (541 + R.GetHashCode()) * (541 + G.GetHashCode()) * (541 + B.GetHashCode()) * (541 + A.GetHashCode());
        }
    }
}
