namespace TestApplication.Interface
{
    public interface ISpellChecker
    {
        void CheckSpelling(string inputPath, string outputPath, int errorThreshold);
    }
}
