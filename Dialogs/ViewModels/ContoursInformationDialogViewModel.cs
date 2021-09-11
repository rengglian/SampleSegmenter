using OxyPlot;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using SampleSegmenter.Helper;
using SampleSegmenter.Interfaces;
using SampleSegmenter.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SampleSegmenter.Dialogs.ViewModels
{
    public class ContoursInformationDialogViewModel : BindableBase, IDialogAware
    {
        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private PlotModel _plotModelHisto;
        public PlotModel PlotModelHisto
        {
            get => _plotModelHisto ??= new();
            set => SetProperty(ref _plotModelHisto, value);
        }

        private int _histogramWidth = 600;
        public int HistogramWidth
        {
            get => _histogramWidth;
            set => SetProperty(ref _histogramWidth, value);
        }

        private int _histogramHeight = 500;
        public int HistogramHeight
        {
            get => _histogramHeight;
            set => SetProperty(ref _histogramHeight, value);
        }

        private string _contoursInfo;
        public string ContoursInfo
        {
            get => _contoursInfo;
            set => SetProperty(ref _contoursInfo, value);
        }

        private DelegateCommand _closeDialogCommand;
        public DelegateCommand CloseDialogCommand => _closeDialogCommand ??= new(CloseDialogCommandHandler);
        
        private DelegateCommand _exportCommand;
        public DelegateCommand ExportCommand => _exportCommand ??= new(ExportCommandHandler);

        private string fileName;

        private List<List<float>> _histogramValues = new();

        public event Action<IDialogResult> RequestClose;

        private void CloseDialogCommandHandler()
        {
            RequestClose?.Invoke(new DialogResult());
        }

        public bool CanCloseDialog()
            => true;

        public void OnDialogClosed() { }  

        public void OnDialogOpened(IDialogParameters parameters)
        {
            var contoursInfo = parameters.GetValue<List<IContourInfo>>("contoursInfo").Cast<ContourInfoAdvanced>();
            fileName = parameters.GetValue<string>("fileName");

            Title = @"Histogram of " + fileName;


            _histogramValues = contoursInfo.Select(list => list.HistogramValues).ToList();

            PlotModelHisto = PlotModelHelper.LineSeries(_histogramValues, fileName);

            string header = contoursInfo.First().GetHeader();
            string result = string.Join(Environment.NewLine, contoursInfo);
            ContoursInfo = header + result;
        }

        private void ExportCommandHandler()
        {
            PlotModelHelper.ExportData(PlotModelHisto, HistogramWidth, HistogramHeight, fileName, ContoursInfo);
        }
    }
}
