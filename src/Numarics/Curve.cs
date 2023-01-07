using System.Globalization;
using System.Runtime.InteropServices;

namespace AampLibrary.Numarics
{
    public struct Curve
    {
        public uint A;
        public uint B;
        public float[] Floats = new float[30];

        public Curve() { }
        public Curve(uint a, uint b, float[] floats)
        {
            A = a;
            B = b;
            Floats = floats.Length == 30 ? floats : throw new InvalidDataException("Invalid float[], please provide a float[30] array");
        }

        public static bool operator ==(Curve a, Curve b)
        {
            return a.Equals(ref b);
        }

        public static bool operator !=(Curve a, Curve b)
        {
            return !a.Equals(ref b);
        }

        public override bool Equals(object? obj)
        {
            if (obj is Curve curve) {
                return Equals(ref curve);
            }

            return false;
        }

        public bool Equals(Curve other)
        {
            return Equals(ref other);
        }

        public bool Equals(ref Curve other)
        {
            return A == other.A && B == other.B && Floats == other.Floats;
        }

        public override string ToString()
        {
            return ToString(null, CultureInfo.InvariantCulture);
        }

        public string ToString(string? format, IFormatProvider? formatProvider)
        {
            return string.Format(formatProvider, format ?? "{{A={0}, B={1}, Floats={2.Length}}}", A, B, Floats);
        }

        public override int GetHashCode()
        {
            return A.GetHashCode() * 17 + B.GetHashCode() * 17 + Floats.GetHashCode();
        }
    }
}
