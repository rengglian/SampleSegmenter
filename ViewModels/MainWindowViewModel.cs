using Prism.Commands;
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

        public MaskOptions MaskOptions { get; set; } = new();
        public EqualizerOptions EqualizerOptions { get; set; } = new();
        public DenoiseOptions DenoiseOptions { get; set; } = new();
        public ThresholdOptions ThresholdOptions { get; set; } = new();
        public ContoursOptions ContoursOptions { get; set; } = new();
        public DilateOptions DilateOptions { get; set; } = new();
        
        public DelegateCommand OpenImageCommand { get; }
        public DelegateCommand<object> SetOptionsCommand { get; }
        public DelegateCommand ShowHistogramCommand { get; }

        public MainWindowViewModel(IOpenFileService openFileService, IDialogService dialogService)
        {
            OpenFileService = openFileService;
            _dialogService = dialogService;
            
            ImageProcessingService = new ImageProcessingService();

            OpenImageCommand = new DelegateCommand(OpenImageCommandHandler);
            SetOptionsCommand = new DelegateCommand<object>(SetOptionsCommandHandler);
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
                _imageFromFile = new ImageFromFile(OpenFileService.FileNames[0]);
                ImageProcessingService.SetOrigMat(_imageFromFile.GetImageMat());
                MaskOptions.IsEnabled = false;
                MaskOptions.X = 0;
                MaskOptions.Y = 0;
                MaskOptions.Width = 0;
                MaskOptions.Height = 0;
            }
        }

        private void ShowHistogramCommandHandler()
        {
            _dialogService.ShowHistogramDialog(ImageProcessingService.GetContoursInfo(), OpenFileService.FileNameOnly, r=> { });
        }
    }
}
