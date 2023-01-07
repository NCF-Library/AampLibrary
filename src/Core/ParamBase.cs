using AampLibrary.Core.v2;
using AampLibrary.Numarics;
using CeadLibrary.IO;
using System.Diagnostics;
using System.Numerics;
using System.Reflection;

namespace AampLibrary.Core
{
    public enum ParameterType : ushort
    {
        Bool,
        Float32,
        Int32,
        Vector2,
        Vector3,
        Vector4,
        Color,
        String32,
        String64,
        Curve1,
        Curve2,
        Curve3,
        Curve4,
        Int32Buffer,
        Float32Buffer,
        String256,
        Quat,
        UInt32,
        UInt32Buffer,
        BinaryBuffer,
        StringRef
    }

    [DebuggerDisplay("Value = {GetValue()}, Type = {GetParamType()}")]
    public abstract class ParamBase
    {
        public bool? Bool { get; set; } = null;
        public float? Float32 { get; set; } = null;
        public int? Int32 { get; set; } = null;
        public Vector2? Vector2 { get; set; } = null;
        public Vector3? Vector3 { get; set; } = null;
        public Vector4? Vector4 { get; set; } = null;
        public Color? Color { get; set; } = null;
        public string? String32 { get; set; } = null;
        public string? String64 { get; set; } = null;
        public Curve? Curve1 { get; set; } = null;
        public Curve2? Curve2 { get; set; } = null;
        public Curve3? Curve3 { get; set; } = null;
        public Curve4? Curve4 { get; set; } = null;
        public int[]? Int32Buffer { get; set; } = null;
        public float[]? Float32Buffer { get; set; } = null;
        public string? String256 { get; set; } = null;
        public Quaternion? Quat { get; set; } = null;
        public uint? UInt32 { get; set; } = null;
        public uint[]? UInt32Buffer { get; set; } = null;
        public byte[]? BinaryBuffer { get; set; } = null;
        public string? StringRef { get; set; } = null;

        internal ParamBase(CeadReader reader) => Read(reader);
        public ParamBase(bool boolean) => Bool = boolean;
        public ParamBase(float float32) => Float32 = float32;
        public ParamBase(int int32) => Int32 = int32;
        public ParamBase(Vector2 vector2) => Vector2 = vector2;
        public ParamBase(Vector3 vector3) => Vector3 = vector3;
        public ParamBase(Vector4 vector4) => Vector4 = vector4;
        public ParamBase(Color color) => Color = color;
        public ParamBase(Curve curve1) => Curve1 = curve1;
        public ParamBase(Curve2 curve2) => Curve2 = curve2;
        public ParamBase(Curve3 curve3) => Curve3 = curve3;
        public ParamBase(Curve4 curve4) => Curve4 = curve4;
        public ParamBase(int[] int32Buffer) => Int32Buffer = int32Buffer;
        public ParamBase(float[] float32Buffer) => Float32Buffer = float32Buffer;
        public ParamBase(Quaternion quat) => Quat = quat;
        public ParamBase(uint uInt32) => UInt32 = uInt32;
        public ParamBase(uint[] uInt32Buffer) => UInt32Buffer = uInt32Buffer;
        public ParamBase(byte[] binaryBuffer) => BinaryBuffer = binaryBuffer;
        public ParamBase(string value)
        {
            if (value.Length <= 32) {
                String32 = value;
            }
            else if (value.Length <= 64) {
                String64 = value;
            }
            else if (value.Length <= 256) {
                String256 = value;
            }
            else {
                StringRef = value;
            }
        }

        public abstract void Read(CeadReader reader);
        public void Read(CeadReader reader, ParameterType type)
        {
            if (type == ParameterType.Bool) {
                Bool = reader.ReadBool(BoolType.Byte);
            }
            else if (type == ParameterType.Float32) {
                Float32 = reader.ReadSingle();
            }
            else if (type == ParameterType.Int32) {
                Int32 = reader.ReadInt32();
            }
            else if (type == ParameterType.Vector2) {
                Vector2 = new(reader.ReadSingle(), reader.ReadSingle());
            }
            else if (type == ParameterType.Vector3) {
                Vector3 = new(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
            }
            else if (type == ParameterType.Vector4) {
                Vector4 = new(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
            }
            else if (type == ParameterType.Color) {
                Span<byte> buffer = stackalloc byte[4];
                reader.Read(buffer);
                Color = new(buffer[0], buffer[1], buffer[2], buffer[3]);
            }
            else if (type == ParameterType.String32) {
                String32 = reader.ReadString(StringType.ZeroTerminated);
                if (String32.Length > 32) {
                    throw new InvalidDataException($"Could not parse String32 parameter. Expected char[32], got char[{String32.Length}]");
                }
            }
            else if (type == ParameterType.String64) {
                String64 = reader.ReadString(StringType.ZeroTerminated);
                if (String64.Length > 64) {
                    throw new InvalidDataException($"Could not parse String32 parameter. Expected char[64], got char[{String64.Length}]");
                }
            }
            else if (type == ParameterType.Curve1) {
                Curve1 = reader.ReadCurve();
            }
            else if (type == ParameterType.Curve2) {
                Curve2 = new(reader.ReadCurve(), reader.ReadCurve());
            }
            else if (type == ParameterType.Curve3) {
                Curve3 = new(reader.ReadCurve(), reader.ReadCurve(), reader.ReadCurve());
            }
            else if (type == ParameterType.Curve4) {
                Curve4 = new(reader.ReadCurve(), reader.ReadCurve(), reader.ReadCurve(), reader.ReadCurve());
            }
            else if (type == ParameterType.Int32Buffer) {
                reader.Seek(-4, SeekOrigin.Current);
                Int32Buffer = reader.ReadObjects(reader.ReadInt32(), 0, SeekOrigin.Current, reader.ReadInt32);
            }
            else if (type == ParameterType.Float32Buffer) {
                reader.Seek(-4, SeekOrigin.Current);
                Float32Buffer = reader.ReadObjects(reader.ReadInt32(), 0, SeekOrigin.Current, reader.ReadSingle);
            }
            else if (type == ParameterType.String256) {
                String256 = reader.ReadString(StringType.ZeroTerminated);
                if (String256.Length > 256) {
                    throw new InvalidDataException($"Could not parse String32 parameter. Expected char[64], got char[{String256.Length}]");
                }
            }
            else if (type == ParameterType.Quat) {
                Quat = new(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
            }
            else if (type == ParameterType.UInt32) {
                UInt32 = reader.ReadUInt32();
            }
            else if (type == ParameterType.UInt32Buffer) {
                reader.Seek(-4, SeekOrigin.Current);
                UInt32Buffer = reader.ReadObjects(reader.ReadInt32(), 0, SeekOrigin.Current, reader.ReadUInt32);
            }
            else if (type == ParameterType.BinaryBuffer) {
                reader.Seek(-4, SeekOrigin.Current);
                BinaryBuffer = reader.ReadBytes(reader.ReadInt32());
            }
            else if (type == ParameterType.StringRef) {
                StringRef = reader.ReadString(StringType.ZeroTerminated);
            }
        }

        public abstract void Write(CeadWriter writer, string name, AampMeta meta);
        public void Write(CeadWriter writer)
        {
            if (Bool is bool boolean) {
                writer.Write(boolean, BoolType.Byte);
            }
            else if (Float32 is float _float) {
                writer.Write(_float);
            }
            else if (Int32 is int _int) {
                writer.Write(_int);
            }
            else if (Vector2 is Vector2 vector2) {
                writer.Write(vector2.X);
                writer.Write(vector2.Y);
            }
            else if (Vector3 is Vector3 vector3) {
                writer.Write(vector3.X);
                writer.Write(vector3.Y);
                writer.Write(vector3.Z);
            }
            else if (Vector4 is Vector4 vector4) {
                writer.Write(vector4.X);
                writer.Write(vector4.Y);
                writer.Write(vector4.Z);
                writer.Write(vector4.W);
            }
            else if (Color is Color color) {
                writer.Write(color.R);
                writer.Write(color.G);
                writer.Write(color.B);
                writer.Write(color.A);
            }
            else if (String32 is string str32 && str32.Length <= 32) {
                writer.Write(str32, StringType.ZeroTerminated);
            }
            else if (String64 is string str64 && str64.Length <= 64) {
                writer.Write(str64, StringType.ZeroTerminated);
            }
            else if (Curve1 is Curve curve) {
                writer.Write(curve);
            }
            else if (Curve2 is Curve2 curve2) {
                writer.Write(curve2.X);
                writer.Write(curve2.Y);
            }
            else if (Curve3 is Curve3 curve3) {
                writer.Write(curve3.X);
                writer.Write(curve3.Y);
                writer.Write(curve3.Z);
            }
            else if (Curve4 is Curve4 curve4) {
                writer.Write(curve4.X);
                writer.Write(curve4.Y);
                writer.Write(curve4.Z);
                writer.Write(curve4.W);
            }
            else if (Int32Buffer is int[] int32Buffer) {
                writer.Write(int32Buffer.Length);
                writer.WriteObjects(int32Buffer, writer.Write);
            }
            else if (Float32Buffer is float[] float32Buffer) {
                writer.Write(float32Buffer.Length);
                writer.WriteObjects(float32Buffer, writer.Write);
            }
            else if (String256 is string str256 && str256.Length <= 256) {
                writer.Write(str256, StringType.ZeroTerminated);
            }
            else if (Quat is Quaternion quat) {
                writer.Write(quat.X);
                writer.Write(quat.Y);
                writer.Write(quat.Z);
                writer.Write(quat.W);
            }
            else if (UInt32 is uint uint32) {
                writer.Write(uint32);
            }
            else if (UInt32Buffer is uint[] uint32Buffer) {
                writer.Write(uint32Buffer.Length);
                writer.WriteObjects(uint32Buffer, writer.Write);
            }
            else if (BinaryBuffer is byte[] binaryBuffer) {
                writer.Write(binaryBuffer.Length);
                writer.Write(binaryBuffer);
            }
            else if (StringRef is string str) {
                writer.Write(str, StringType.ZeroTerminated);
            }
        }

        public object GetValue()
        {
            foreach (var param in typeof(Param).GetProperties(BindingFlags.Public | BindingFlags.Instance)) {
                object? value = param.GetValue(this);
                if (value != null) {
                    return value;
                }
            }

            throw new InvalidDataException($"Could not find any non-null parameter type in '{typeof(Param).FullName}'");
        }

        public ParameterType GetParamType()
        {
            foreach (var param in typeof(Param).GetProperties(BindingFlags.Public | BindingFlags.Instance)) {
                if (param.GetValue(this) != null) {
                    return Enum.Parse<ParameterType>(param.Name);
                }
            }

            throw new InvalidDataException($"Could not find any non-null parameter type in '{typeof(Param).FullName}'");
        }
    }
}
