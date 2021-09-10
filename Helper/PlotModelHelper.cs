using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Legends;
using OxyPlot.Series;
using OxyPlot.Wpf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SampleSegmenter.Helper
{
    public static class PlotModelHelper
    {
        public static PlotModel ColumnSeries(List<DataPoint> histogramPoints, string title = "")
        {
            var plotModel = new PlotModel();

            var barSeries = new BarSeries()
            {
                LabelPlacement = LabelPlacement.Inside,
                LabelFormatString = "{0}",
                LabelMargin = 5
            };

            var categoryAxis = new CategoryAxis
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

            plotModel.Title = title;
            plotModel.Series.Add(barSeries);
            plotModel.Axes.Add(categoryAxis);

            return plotModel;
        }

        public static PlotModel LineSeries(List<List<float>> histogramLists, string title= "")
        {
            var plotModel = new PlotModel();

            plotModel.Legends.Add(new Legend
            {
                LegendTitle = @"Legend",
                LegendPosition = LegendPosition.TopRight
            });

            for(int i = 0; i < histogramLists.Count(); i++)
            {
                plotModel.Series.Add(new LineSeries
                {
                    LineStyle = LineStyle.Solid,
                    Title = i.ToString()
                });

                for (int j = 0; j < histogramLists[i].Count(); j++)
                {
                    ((LineSeries)plotModel.Series[i]).Points.Add(new DataPoint(j, histogramLists[i][j]));
                }
            }

            plotModel.Title = title;

            return plotModel;
        }

        static public void ExportData(PlotModel plotModel, int width, int heigt, string description = "", string contoursInfo = "")
        {
            DateTime dateAndTime = DateTime.Now;
            var path = @".\\Logs\\";

            _ = Directory.CreateDirectory(path);

            string fileDescription = string.IsNullOrEmpty(description) ? "" : "_" + description;

            var filename = @"" + dateAndTime.ToString("yyyMMdd") + "_" + dateAndTime.ToString("HHmmss") + fileDescription;

            var pngExporter = new PngExporter { Width = width, Height = heigt };
            pngExporter.ExportToFile(plotModel, path + filename + ".png");

            File.WriteAllText(path + filename + ".csv", contoursInfo);
        }
    }
}
