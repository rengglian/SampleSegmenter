﻿using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using SampleSegmenter.Extensions;
using SampleSegmenter.Interfaces;
using SampleSegmenter.Models;
using SampleSegmenter.Options;
using SampleSegmenter.Services;

namespace SampleSegmenter.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        public IOpenFileService OpenFileService { get; set; }
        private readonly IDialogService _dialogService;
        public IImageProcessingService ImageProcessingService { get; set; }

        private ImageFromFile _imageFromFile;

        public EqualizerOptions EqualizerOptions { get; set; } = new();
        public DenoiseOptions DenoiseOptions { get; set; } = new();
        public ThresholdOptions ThresholdOptions { get; set; } = new();
        public ContoursOptions ContoursOptions { get; set; } = new();
        public DilateOptions DilateOptions { get; set; } = new();
        
        public DelegateCommand OpenImageCommand { get; }
        public DelegateCommand SetEqualizerOptionsCommand { get; }
        public DelegateCommand SetDenoiseOptionsCommand { get; }
        public DelegateCommand SetThresholdOptionsCommand { get; }
        public DelegateCommand SetDilateOptionsCommand { get; }
        public DelegateCommand SetContourOptionsCommand { get; }
        public DelegateCommand ShowHistogramCommand { get; }

        public MainWindowViewModel(IOpenFileService openFileService, IDialogService dialogService)
        {
            OpenFileService = openFileService;
            _dialogService = dialogService;
            
            ImageProcessingService = new ImageProcessingService();

            OpenImageCommand = new DelegateCommand(OpenImageCommandHandler);
            SetEqualizerOptionsCommand = new DelegateCommand(SetEqualizerOptionsCommandHandler);
            SetDenoiseOptionsCommand = new DelegateCommand(SetDenoiseOptionsCommandHandler);
            SetThresholdOptionsCommand = new DelegateCommand(SetThresholdOptionsCommandHandler);
            SetDilateOptionsCommand = new DelegateCommand(SetDilateOptionsCommandHandler);
            SetContourOptionsCommand = new DelegateCommand(SetContoursOptionsCommandHandler);
            ShowHistogramCommand = new DelegateCommand(ShowHistogramCommandHandler);
        }

        private void OpenImageCommandHandler()
        {
            if ((bool)OpenFileService.OpenFile())
            {
                _imageFromFile = new ImageFromFile(OpenFileService.FileNames[0]);
                ImageProcessingService.SetOrigMat(_imageFromFile.GetImageMat());
            }
        }

        private void SetEqualizerOptionsCommandHandler()
        {
            ImageProcessingService.SetOptions(EqualizerOptions);
        }

        private void SetDenoiseOptionsCommandHandler()
        {
            ImageProcessingService.SetOptions(DenoiseOptions);
        }

        private void SetThresholdOptionsCommandHandler()
        {
            ImageProcessingService.SetOptions(ThresholdOptions);
        }

        private void SetDilateOptionsCommandHandler()
        {
            ImageProcessingService.SetOptions(DilateOptions);
        }

        private void SetContoursOptionsCommandHandler()
        {
            ImageProcessingService.SetOptions(ContoursOptions);
        }

        private void ShowHistogramCommandHandler()
        {
            _dialogService.ShowHistogramDialog(ImageProcessingService.GetContoursInfo(), OpenFileService.FileNameOnly, r=> { });
        }
    }
}
