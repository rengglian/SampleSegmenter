using System;
using System.Collections.Generic;

namespace SampleSegmenter.Models
{
    public class ContourInfo
    {
        public int CentroidX { get; set; }
        public int CentroidY { get; set; }
        public double ContourArea { get; set; }
        public double ContourCircumference { get; set; }
        public float CircleX { get; set; }
        public float CircleY { get; set; }
        public double CircleRadius { get; set; }
        public List<float> HistogramValues { get; set; }

        public override string ToString()
        {
            return @"" + CentroidX + "\t" + CentroidY + "\t" + Math.Round(ContourArea, 2) + "\t" + Math.Round(ContourCircumference, 2);
        }
    }
}
