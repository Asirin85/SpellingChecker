using System.Collections.Generic;

namespace TestApplication.Interface
{
    public interface IWriter
    {
        void Write(IEnumerable<string[]> data, string path);
    }
}
