﻿using OxyPlot;
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
        private string _title;
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

        private string _contoursInfo;
        public string ContoursInfo
        {
            get { return _contoursInfo; }
            set { SetProperty(ref _contoursInfo, value); }
        }

        public DelegateCommand CloseDialogCommand { get; }
        public DelegateCommand ExportCommand { get; }

        private string fileName;

        private List<DataPoint> _histogramValues = new();

        public event Action<IDialogResult> RequestClose;

        public HistogramDialogViewModel()
        {
            HistogramWidth = 600;
            HistogramHeight = 500;

            PlotModelHisto = new();

            CloseDialogCommand = new DelegateCommand(CloseDialogCommandHandler);
            ExportCommand = new DelegateCommand(ExportCommandHandler);
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
            fileName = parameters.GetValue<string>("fileName");

            Title = @"Histogram of " + fileName;

            _histogramValues = contoursInfo.GroupBy(c => c.Y / 100).OrderBy(g => g.Key).Select(groups => new DataPoint( groups.Key * 100, groups.Count()) ).ToList();

            PlotModelHisto = PlotModelHelper.ColumnSeries(_histogramValues, fileName);

            string header = "X\tY\tArea\tCircumference\n";
            string result = string.Join(Environment.NewLine, contoursInfo);
            ContoursInfo = header + result;
        }

        private void ExportCommandHandler()
        {
            PlotModelHelper.ExportData(PlotModelHisto, HistogramWidth, HistogramHeight, fileName, ContoursInfo);
        }
    }
}