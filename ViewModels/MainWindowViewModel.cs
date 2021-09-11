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
            get => _imageProcessingService ??= new ImageProcessingService();
            set => SetProperty(ref _imageProcessingService, value);
        }

        private ImageFromFile _imageFromFile;

        public MaskOptions MaskOptions { get; set; } = new();
        public EqualizerOptions EqualizerOptions { get; set; } = new();
        public DenoiseOptions DenoiseOptions { get; set; } = new();
        public ThresholdOptions ThresholdOptions { get; set; } = new();
        public ContoursOptions ContoursOptions { get; set; } = new();
        public DilateOptions DilateOptions { get; set; } = new();

        private DelegateCommand _openImageCommand;
        public DelegateCommand OpenImageCommand => _openImageCommand ??= new (OpenImageCommandHandler);

        private DelegateCommand _showVerticalDistributionCommand;
        public DelegateCommand ShowVerticalDistributionCommand => _showVerticalDistributionCommand ??= new(ShowVerticalDistributionCommandHandler);

        private DelegateCommand _showHistogramCommand;
        public DelegateCommand ShowHistogramCommand => _showHistogramCommand ??= new(ShowHistogramCommandHandler);

        private DelegateCommand<object> _setOptionsCommand;
        public DelegateCommand<object> SetOptionsCommand => _setOptionsCommand ??= new DelegateCommand<object>(SetOptionsCommandHandler);

        public MainWindowViewModel(IOpenFileService openFileService, IDialogService dialogService)
        {
            OpenFileService = openFileService;
            _dialogService = dialogService;
        }

        private void SetOptionsCommandHandler(object options)
            => ImageProcessingService.SetOptions(options);

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
            => _dialogService.ShowVerticalDistributionDialog(ImageProcessingService.GetContoursInfo(), OpenFileService.FileNameOnly, r => { });

        private void ShowHistogramCommandHandler()
            => _dialogService.ShowContoursInformationDialog(ImageProcessingService.GetContoursInfo(), OpenFileService.FileNameOnly, r => { });
    }
}
