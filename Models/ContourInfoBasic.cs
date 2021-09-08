using SampleSegmenter.Interfaces;
using System;
using System.Collections.Generic;

namespace SampleSegmenter.Models
{
    public class ContourInfoBasic : IContourInfo
    {
        public string ContourName { get; set; }
        public int CentroidX { get; set; }
        public int CentroidY { get; set; }
        public double ContourArea { get; set; }
        public double ContourCircumference { get; set; }

        public string GetHeader() => $"{nameof(ContourName)}\t{nameof(CentroidX)}\t{nameof(CentroidY)}" +
            $"\t{nameof(ContourArea)}\t{nameof(ContourCircumference)}\n";

        public override string ToString()
        {
            return $"{ContourName}\t{CentroidX}\t{CentroidY}" +
                $"\t{Math.Round(ContourArea, 2)}\t{Math.Round(ContourCircumference, 2)}";
        }
    }
}
