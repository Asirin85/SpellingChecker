using System.Collections.Generic;

namespace TestApplication.Interface
{
    public interface IReader
    {
        (IEnumerable<string> glossary, IEnumerable<string[]> input) Read(string input);
    }
}
