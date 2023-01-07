namespace AampLibrary.Core
{
    public class AampMeta
    {
        private readonly int _ioOffset;
        public int IoListsOffset => _ioOffset;
        public int IoObjsOffset => _ioOffset + ListWriters.Count * 12;
        public int IoParamsOffset => IoObjsOffset + ObjWriters.Count * 8;

        public int ObjsWritten { get; set; } = 0;
        public int ParamsWritten { get; set; } = 0;

        public List<Action<int>> ListWriters { get; } = new();
        public List<Action<int>> ObjWriters { get; } = new();
        public List<Action<int>> ParamWriters { get; } = new();
        public List<Action> DataWriters { get; } = new();
        public List<Action> StringWriters { get; } = new();

        public AampMeta(int ioOffset)
        {
            _ioOffset = ioOffset;
        }
    }
}
