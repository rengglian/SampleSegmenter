namespace SampleSegmenter.Interfaces
{
    public interface IOpenFileService
    {
        public bool? OpenFile();
        public string[] FileNames { get; }

        public string FileName { get; }
    }
}
