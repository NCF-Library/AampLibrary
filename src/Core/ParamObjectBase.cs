using CeadLibrary.IO;

namespace AampLibrary.Core
{
    public abstract class ParamObjectBase
    {
        internal string Name { get; set; } = string.Empty;

        public ParamObjectBase() { }
        internal ParamObjectBase(CeadReader reader) => Read(reader);

        public abstract void Read(CeadReader reader);
        public abstract void Write(CeadWriter writer, string name, AampMeta meta);
    }
}
