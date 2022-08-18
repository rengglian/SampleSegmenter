
namespace SampleSegmenter.Options
{
    public class AllOptions
    {
        public ResizeOptions ResizeOptions { get; set; } = new();
        public MaskOptions MaskOptions { get; set; } = new();
        public EqualizerOptions EqualizerOptions { get; set; } = new();
        public DenoiseOptions DenoiseOptions { get; set; } = new();
        public CannyEdgeOptions CannyEdgeOptions { get; set; } = new();
        public ThresholdOptions ThresholdOptions { get; set; } = new();
        public ContoursOptions ContoursOptions { get; set; } = new();
        public DilateOptions DilateOptions { get; set; } = new();
    }
}
