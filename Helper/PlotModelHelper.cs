using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System.Collections.Generic;

namespace SampleSegmenter.Helper
{
    public static class PlotModelHelper
    {
        public static PlotModel ColumnSeries(List<DataPoint> histogramPoints)
        {
            var model = new PlotModel();

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

            model.Series.Add(barSeries);

            model.Axes.Add(categoryAxis);



            return model;
        }
    }
}
