namespace SampleSegmenter.Enums
{
    public enum ImageProcessingSteps
    {
        Orignal,
        Resize,
        Grayscaled,
        Denoised,
        CannyEdge,
        Masked,
        Binarized,
        Dilated,
        Result
    }
}
