using CeadLibrary.IO;

namespace AampLibrary.Core
{
    public abstract class ParamListBase
    {
        internal string Name { get; set; } = string.Empty;

        public ParamListBase() { }
        internal ParamListBase(CeadReader reader) => Read(reader);

        public abstract void Read(CeadReader reader);
        public abstract void Write(CeadWriter writer, string name, AampMeta meta);
    }
}
