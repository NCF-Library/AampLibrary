using AampLibrary.Numarics;
using CeadLibrary.IO;
using System.Reflection;
using System.Text.Json;

namespace AampLibrary.Core
{
    public static class AampHelper
    {
        private static Dictionary<uint, string>? _hashes;
        internal static Dictionary<uint, string> Hashes {
            get {
                return _hashes ??= JsonSerializer.Deserialize<Dictionary<uint, string>>(
                    Assembly.GetCallingAssembly()?.GetManifestResourceStream("AampLibrary.Data.Hashes.json")!)!;
            }
        }

        public static Curve ReadCurve(this CeadReader reader)
        {
            return new(reader.ReadUInt32(), reader.ReadUInt32(),
                reader.ReadObjects(30, 0, SeekOrigin.Current, reader.ReadSingle));
        }

        public static void Write(this CeadWriter writer, Curve curve)
        {
            writer.Write(curve.A);
            writer.Write(curve.B);
            writer.WriteObjects(curve.Floats, writer.Write);
        }
    }
}
