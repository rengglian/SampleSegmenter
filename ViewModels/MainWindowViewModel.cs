using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using SampleSegmenter.Converters;
using SampleSegmenter.Enums;
using SampleSegmenter.Extensions;
using SampleSegmenter.Interfaces;
using SampleSegmenter.Models;
using SampleSegmenter.Options;
using SampleSegmenter.Services;
using System.Windows.Media;

namespace SampleSegmenter.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly IOpenFileService _openFileService;
        private readonly IDialogService _dialogService;
        private readonly ImageProcessingService _imageProcessingService;

        private ImageFromFile _imageFromFile;

        private string _fileName;
        public string FileName
        {
            get { return _fileName; }
            set { SetProperty(ref _fileName, value); }
        }

        private ImageSource _image;
        public ImageSource Image
        {
            get { return _image; }
            set { SetProperty(ref _image, value); }
        }

        public DenoiseOptions DenoiseOptions { get; set; } = new();
        public ThresholdOptions ThresholdOptions { get; set; } = new();
        public ContoursOptions ContoursOptions { get; set; } = new();
        
        private string _contoursInfo;
        public string ContoursInfo
        {
            get { return _contoursInfo; }
            set { SetProperty(ref _contoursInfo, value); }
        }

        private bool _isImageLoaded;
        public bool IsImageLoaded
        {
            get { return _isImageLoaded; }
            set { SetProperty(ref _isImageLoaded, value); }
        }

        private bool isEqualizerEnabled = true;
        public bool IsEqualizerEnabled
        {
            get { return isEqualizerEnabled; }
            set { 
                SetProperty(ref isEqualizerEnabled, value);
                _imageProcessingService.SetEnableEqualized(IsEqualizerEnabled);
                UpdateImage(SelectedImageProcessingStep);
            }
        }

        private ImageProcessingSteps _selectedImageProcessingStep = ImageProcessingSteps.Result;
        public ImageProcessingSteps SelectedImageProcessingStep
        {
            get { return _selectedImageProcessingStep; }
            set
            {
                SetProperty(ref _selectedImageProcessingStep, value);
                UpdateImage(SelectedImageProcessingStep);
            }
        }
        

        public DelegateCommand OpenImageCommand { get; }
        public DelegateCommand SetDenoiseOptionsCommand { get; }
        public DelegateCommand SetThresholdOptionsCommand { get; }
        public DelegateCommand SetMinimumAreaCommand { get; }
        public DelegateCommand ShowHistogramCommand { get; }

        public MainWindowViewModel(IOpenFileService openFileService, IDialogService dialogService)
        {
            _openFileService = openFileService;
            _dialogService = dialogService;
            
            _imageProcessingService = new();

            OpenImageCommand = new DelegateCommand(OpenImageCommandHandler);
            SetDenoiseOptionsCommand = new DelegateCommand(SetDenoiseOptionsCommandHandler);
            SetThresholdOptionsCommand = new DelegateCommand(SetThresholdOptionsCommandHandler);
            SetMinimumAreaCommand = new DelegateCommand(SetMinimumAreaCommandHandler);
            ShowHistogramCommand = new DelegateCommand(ShowHistogramCommandHandler);
        }

        private void OpenImageCommandHandler()
        {
            if ((bool)_openFileService.OpenFile())
            {
                _imageFromFile = new ImageFromFile( _openFileService.FileNames[0]);
                FileName = _imageFromFile.GetFileName();
                _imageProcessingService.SetOrigMat(_imageFromFile.GetImageMat());
                UpdateImage(ImageProcessingSteps.Result);
                IsImageLoaded = true;
            }
        }

        private void SetDenoiseOptionsCommandHandler()
        {
            _imageProcessingService.SetDenoiseOptions(DenoiseOptions);
            UpdateImage(SelectedImageProcessingStep);
        }

        private void SetThresholdOptionsCommandHandler()
        {
            _imageProcessingService.SetThresholdOptions(ThresholdOptions);
            UpdateImage(SelectedImageProcessingStep);
        }

        private void SetMinimumAreaCommandHandler()
        {
            _imageProcessingService.SetMinimumArea(ContoursOptions);
            UpdateImage(SelectedImageProcessingStep);
        }

        private void ShowHistogramCommandHandler()
        {
            _dialogService.ShowHistogramDialog(_imageProcessingService.GetContoursInfo(), r=> { });
        }

        private void UpdateImage(ImageProcessingSteps imageProcessingSteps)
        {
            switch(imageProcessingSteps)
            {
                case ImageProcessingSteps.Orignal:
                    {
                        Image = ImageConverter.Convert(_imageFromFile.GetImageMat());
                        break;
                    }
                case ImageProcessingSteps.Denoised:
                    {
                        Image = ImageConverter.Convert(_imageProcessingService.GetDenoisedMat());
                        break;
                    }
                case ImageProcessingSteps.Grayscaled:
                    {
                        Image = ImageConverter.Convert(_imageProcessingService.GetGrayScaledMat());
                        break;
                    }
                case ImageProcessingSteps.Binarized:
                    {
                        Image = ImageConverter.Convert(_imageProcessingService.GetBinarizedMat());
                        break;
                    }
                case ImageProcessingSteps.Result:
                    {
                        Image = ImageConverter.Convert(_imageProcessingService.GetResultMat());
                        break;
                    }
            }
            ContoursInfo = _imageProcessingService.GetContoursInfoText();
        }
    }
}
