using System;

namespace SampleSegmenter.Models
{
    public class ContourInfo
    {
        public int X { get; set; }
        public int Y { get; set; }
        public double Area { get; set; }
        public double Circumference { get; set; }

        public override string ToString()
        {
            return @"" + X + "\t" + Y + "\t" + Math.Round(Area, 2) + "\t" + Math.Round(Circumference, 2);
        }
    }
}
