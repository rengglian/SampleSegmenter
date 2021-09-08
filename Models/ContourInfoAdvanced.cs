using SampleSegmenter.Interfaces;
using System;
using System.Collections.Generic;

namespace SampleSegmenter.Models
{
    public class ContourInfoAdvanced : IContourInfo
    {
        public string ContourName { get; set; }
        public int CentroidX { get; set; }
        public int CentroidY { get; set; }
        public double ContourArea { get; set; }
        public double ContourCircumference { get; set; }
        public float CircleX { get; set; }
        public float CircleY { get; set; }
        public double CircleRadius { get; set; }
        public double DistanceToCenter { get; set; }
        public List<float> HistogramValues { get; set; }

        public string GetHeader() => $"{nameof(ContourName)}\t{nameof(CentroidX)}\t{nameof(CentroidY)}" +
            $"\t{nameof(ContourArea)}\t{nameof(ContourCircumference)}" +
            $"\t{nameof(CircleX)}\t{nameof(CircleY)}" +
            $"\t{nameof(CircleRadius)}\t{nameof(DistanceToCenter)}\n";

        public override string ToString()
        {
            return $"{ContourName}\t{CentroidX}\t{CentroidY}" +
                $"\t{Math.Round(ContourArea, 2)}\t{Math.Round(ContourCircumference)}" +
                $"\t{Math.Round(CircleX, 2)}\t{Math.Round(CircleY, 2)}" +
                $"\t{Math.Round(CircleRadius, 2)}\t{Math.Round(DistanceToCenter, 2)}";
        }
    }
}
