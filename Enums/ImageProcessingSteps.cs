using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleSegmenter.Enums
{
    public enum ImageProcessingSteps
    {
        Orignal, 
        Denoised,
        Grayscaled,
        Binarized,
        Dilated,
        Result
    }
}
