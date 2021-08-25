using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using SampleSegmenter.Extensions;
using SampleSegmenter.Interfaces;
using SampleSegmenter.Models;
using SampleSegmenter.Options;
using SampleSegmenter.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SampleSegmenter.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        public IOpenFileService OpenFileService { get; set; }
        private readonly IDialogService _dialogService;
        private List<IImageProcessingService> _imageProcessingServices { get; set; }
        private IImageProcessingService _imageProcessingService;
        public IImageProcessingService ImageProcessingService 
        {
            get { return _imageProcessingService; }
            set { SetProperty(ref _imageProcessingService, value); }
        }

        private ImageFromFile _imageFromFile;

        public MaskOptions MaskOptions { get; set; } = new();
        public EqualizerOptions EqualizerOptions { get; set; } = new();
        public DenoiseOptions DenoiseOptions { get; set; } = new();
        public ThresholdOptions ThresholdOptions { get; set; } = new();
        public ContoursOptions ContoursOptions { get; set; } = new();
        public DilateOptions DilateOptions { get; set; } = new();
        
        public DelegateCommand OpenImageCommand { get; }
        public DelegateCommand<object> SetOptionsCommand { get; }
        public DelegateCommand ShowVerticalDistributionCommand { get; }
        public DelegateCommand ShowHistogramCommand { get; }

        public MainWindowViewModel(IOpenFileService openFileService, IDialogService dialogService)
        {
            OpenFileService = openFileService;
            _dialogService = dialogService;
            
            ImageProcessingService = new ImageProcessingService();

            OpenImageCommand = new DelegateCommand(OpenImageCommandHandler);
            SetOptionsCommand = new DelegateCommand<object>(SetOptionsCommandHandler);
            ShowVerticalDistributionCommand = new DelegateCommand(ShowVerticalDistributionCommandHandler);
            ShowHistogramCommand = new DelegateCommand(ShowHistogramCommandHandler);
        }

        private void SetOptionsCommandHandler(object options)
        {
            ImageProcessingService.SetOptions(options);
        }

        private void OpenImageCommandHandler()
        {
            if ((bool)OpenFileService.OpenFile())
            {
                _imageProcessingServices = new();

                _imageFromFile = new ImageFromFile(OpenFileService.FileNames[0]);
                _imageProcessingServices.Add(new ImageProcessingService());
                _imageProcessingServices.First().SetOrigMat(_imageFromFile.GetImageMat());
                ImageProcessingService = _imageProcessingServices.First();
            }
        }

        private void ShowVerticalDistributionCommandHandler()
        {
            _dialogService.ShowVerticalDistributionDialog(ImageProcessingService.GetContoursInfo(), OpenFileService.FileNameOnly, r => { });
        }

        private void ShowHistogramCommandHandler()
        {
            _dialogService.ShowContoursInformationDialog(ImageProcessingService.GetContoursInfo(), OpenFileService.FileNameOnly, r => { });
        }
    }
}
