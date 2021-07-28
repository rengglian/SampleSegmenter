using OxyPlot;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using SampleSegmenter.Helper;
using SampleSegmenter.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SampleSegmenter.Dialogs.ViewModels
{
    class HistogramDialogViewModel : BindableBase, IDialogAware
    {
        private string _title = "Histogramm";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private PlotModel _plotModelHisto;
        public PlotModel PlotModelHisto
        {
            get { return _plotModelHisto; }
            set { SetProperty(ref _plotModelHisto, value); }
        }

        private int _histogramWidth;
        public int HistogramWidth
        {
            get { return _histogramWidth; }
            set { SetProperty(ref _histogramWidth, value); }
        }

        private int _histogramHeight;
        public int HistogramHeight
        {
            get { return _histogramHeight; }
            set { SetProperty(ref _histogramHeight, value); }
        }

        public DelegateCommand CloseDialogCommand { get; }


        private List<DataPoint> _histogramValues = new();

        public event Action<IDialogResult> RequestClose;

        public HistogramDialogViewModel()
        {
            HistogramWidth = 600;
            HistogramHeight = 500;

            PlotModelHisto = new();

            CloseDialogCommand = new DelegateCommand(CloseDialogCommandHandler);
        }

        private void CloseDialogCommandHandler()
        {
            RequestClose?.Invoke(new DialogResult());
        }

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
            
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            var contoursInfo = parameters.GetValue<List<ContourInfo>>("contoursInfo");

            _histogramValues = contoursInfo.GroupBy(c => c.Y / 100).OrderBy(g => g.Key).Select(groups => new DataPoint( groups.Key * 100, groups.Count()) ).ToList();

            PlotModelHisto = PlotModelHelper.ColumnSeries(_histogramValues);
        }
    }
}
