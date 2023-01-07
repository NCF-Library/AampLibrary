using CeadLibrary.Extensions;
using CeadLibrary.Generics;
using CeadLibrary.IO;

namespace AampLibrary.Core.v2
{
    public class ParamObject : ParamObjectBase
    {
        public Dictionary<string, Param> Params { get; set; } = new();

        public ParamObject() { }
        public ParamObject(CeadReader reader) : base(reader) { }

        public override void Read(CeadReader reader)
        {
            long ioObjStart = reader.BaseStream.Position;

            uint hash = reader.ReadUInt32();
            Name = AampHelper.Hashes.TryGetValue(hash, out string? name) ? name! : $"0x{hash:X8}";

            uint paramsInfo = reader.ReadUInt32();
            uint paramsOffset = (paramsInfo & 0x0000FFFF) * 4;
            uint paramCount = (paramsInfo & 0xFFFF0000) >> 16;

            reader.ReadObjects((int)paramCount, paramsOffset + ioObjStart, SeekOrigin.Begin, () => {
                Param param = new(reader);
                Params.Add(param.Name, param);
                return param;
            });
        }

        public override void Write(CeadWriter writer, string name, AampMeta meta)
        {
            meta.ObjWriters.Add((index) => {
                writer.Write(name.StartsWith("0x") ? Convert.ToUInt32(name, 16) : CRC32.Compute(name));
                int paramOffset = (meta.IoParamsOffset - (meta.IoObjsOffset + 8 * index) + 8 * meta.ParamsWritten) / 4;
                writer.Write(paramOffset | (Params.Count << 16));
                meta.ParamsWritten += Params.Count;
            });

            writer.WriteObjects(Params, x => x.Value.Write(writer, x.Key, meta));
        }
    }
}
