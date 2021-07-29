using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.Wpf;
using System;
using System.Collections.Generic;
using System.IO;

namespace SampleSegmenter.Helper
{
    public static class PlotModelHelper
    {
        public static PlotModel ColumnSeries(List<DataPoint> histogramPoints, string title = "")
        {
            var model = new PlotModel();

            var barSeries = new OxyPlot.Series.BarSeries()
            {
                LabelPlacement = LabelPlacement.Inside,
                LabelFormatString = "{0}",
                LabelMargin = 5
            };

            var categoryAxis = new OxyPlot.Axes.CategoryAxis
            {
                Position = AxisPosition.Left,
                StartPosition = 1,
                EndPosition = 0
            };

            foreach (DataPoint histogramPoint in histogramPoints)
            {
                barSeries.Items.Add(new BarItem { Value = histogramPoint.Y });
                categoryAxis.Labels.Add(histogramPoint.X.ToString());
            }

            model.Title = title;

            model.Series.Add(barSeries);

            model.Axes.Add(categoryAxis);



            return model;
        }

        static public void ExportData(PlotModel plotModel, int width, int heigt, string description = "", string contoursInfo = "")
        {
            DateTime dateAndTime = DateTime.Now;
            var path = @".\\Logs\\";

            _ = System.IO.Directory.CreateDirectory(path);

            string fileDescription = String.IsNullOrEmpty(description) ? "" : "_" + description;

            var filename = @"" + dateAndTime.ToString("yyyMMdd") + "_" + dateAndTime.ToString("HHmmss") + fileDescription;

            var pngExporter = new PngExporter { Width = width, Height = heigt };
            pngExporter.ExportToFile(plotModel, path + filename + ".png");

            File.WriteAllText(path + filename + ".csv", contoursInfo);
        }
    }
}
