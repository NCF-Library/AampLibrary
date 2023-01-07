using CeadLibrary.Extensions;
using CeadLibrary.IO;

namespace AampLibrary.Core.v2
{
    public class ParamList : ParamListBase
    {
        public Dictionary<string, ParamObject> Objs { get; set; } = new();
        public Dictionary<string, ParamList> Lists { get; set; } = new();

        public ParamList() { }
        internal ParamList(CeadReader reader) : base(reader) { }

        public override void Read(CeadReader reader)
        {
            long ioListStart = reader.BaseStream.Position;

            uint hash = reader.ReadUInt32();
            Name = AampHelper.Hashes.TryGetValue(hash, out string? name) ? name! : $"0x{hash:X8}";

            uint listsInfo = reader.ReadUInt32();
            uint listsOffset = (listsInfo & 0x0000FFFF) * 4;
            uint listCount = (listsInfo & 0xFFFF0000) >> 16;

            uint objsInfo = reader.ReadUInt32();
            uint objsOffset = (objsInfo & 0x0000FFFF) * 4;
            uint objCount = (objsInfo & 0xFFFF0000) >> 16;

            reader.ReadObjects((int)listCount, listsOffset + ioListStart, SeekOrigin.Begin, () => {
                ParamList list = new(reader);
                Lists.Add(list.Name, list);
                return list;
            });
            
            reader.ReadObjects((int)objCount, objsOffset + ioListStart, SeekOrigin.Begin, () => {
                ParamObject obj = new(reader);
                Objs.Add(obj.Name, obj);
                return obj;
            });
        }

        public override void Write(CeadWriter writer, string name, AampMeta meta)
        {
            meta.ListWriters.Add((index) => {
                writer.Write(name.StartsWith("0x") ? Convert.ToUInt32(name, 16) : CRC32.Compute(name));
                writer.Write((12 / 4) | (Lists.Count << 16));
                int objsOffset = (meta.IoObjsOffset - (meta.IoListsOffset + 12 * index) + 8 * meta.ObjsWritten) / 4;
                writer.Write(objsOffset | (Objs.Count << 16));
                meta.ObjsWritten += Objs.Count;
            });

            writer.WriteObjects(Objs, x => x.Value.Write(writer, x.Key, meta));
            writer.WriteObjects(Lists, x => x.Value.Write(writer, x.Key, meta));
        }
    }
}
