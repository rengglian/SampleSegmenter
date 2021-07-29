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
        public ImageProcessingService ImageProcessingService { get; set; }

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
        public DilateOptions DilateOptions { get; set; } = new();

        private bool isEqualizerEnabled = true;
        public bool IsEqualizerEnabled
        {
            get { return isEqualizerEnabled; }
            set { 
                SetProperty(ref isEqualizerEnabled, value);
                ImageProcessingService.SetEnableEqualized(IsEqualizerEnabled);
            }
        }

        private ImageProcessingSteps _selectedImageProcessingStep = ImageProcessingSteps.Result;
        public ImageProcessingSteps SelectedImageProcessingStep
        {
            get { return _selectedImageProcessingStep; }
            set
            {
                SetProperty(ref _selectedImageProcessingStep, value);
            }
        }
        

        public DelegateCommand OpenImageCommand { get; }
        public DelegateCommand SetDenoiseOptionsCommand { get; }
        public DelegateCommand SetThresholdOptionsCommand { get; }
        public DelegateCommand SetDilateOptionsCommand { get; }
        public DelegateCommand SetContourOptionsCommand { get; }
        public DelegateCommand ShowHistogramCommand { get; }

        public MainWindowViewModel(IOpenFileService openFileService, IDialogService dialogService)
        {
            _openFileService = openFileService;
            _dialogService = dialogService;
            
            ImageProcessingService = new();

            OpenImageCommand = new DelegateCommand(OpenImageCommandHandler);
            SetDenoiseOptionsCommand = new DelegateCommand(SetDenoiseOptionsCommandHandler);
            SetThresholdOptionsCommand = new DelegateCommand(SetThresholdOptionsCommandHandler);
            SetDilateOptionsCommand = new DelegateCommand(SetDilateOptionsCommandHandler);
            SetContourOptionsCommand = new DelegateCommand(SetContoursOptionsCommandHandler);
            ShowHistogramCommand = new DelegateCommand(ShowHistogramCommandHandler);
        }

        private void OpenImageCommandHandler()
        {
            if ((bool)_openFileService.OpenFile())
            {
                _imageFromFile = new ImageFromFile( _openFileService.FileNames[0]);
                FileName = _imageFromFile.GetFileName();
                ImageProcessingService.SetOrigMat(_imageFromFile.GetImageMat());
            }
        }

        private void SetDenoiseOptionsCommandHandler()
        {
            ImageProcessingService.SetDenoiseOptions(DenoiseOptions);;
        }

        private void SetThresholdOptionsCommandHandler()
        {
            ImageProcessingService.SetThresholdOptions(ThresholdOptions);
        }

        private void SetDilateOptionsCommandHandler()
        {
            ImageProcessingService.SetDilateOptions(DilateOptions);
        }

        private void SetContoursOptionsCommandHandler()
        {
            ImageProcessingService.SetContoursOptions(ContoursOptions);
        }

        private void ShowHistogramCommandHandler()
        {
            _dialogService.ShowHistogramDialog(ImageProcessingService.GetContoursInfo(), _openFileService.FileName, r=> { });
        }
    }
}
