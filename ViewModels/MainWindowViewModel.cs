using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using SampleSegmenter.Extensions;
using SampleSegmenter.Interfaces;
using SampleSegmenter.Models;
using SampleSegmenter.Options;
using SampleSegmenter.Services;
using System.Collections.Generic;
using System.Linq;

namespace SampleSegmenter.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {

        private AllOptions _allOptions = new();
        private string _optionName = "Options.json";
        public IOpenImageFileService OpenImageFileService { get; set; }
        public IOpenOptionFileService OpenOptionFileService { get; set; }
        private readonly IDialogService _dialogService;
        private List<IImageProcessingService> ImageProcessingServices { get; set; }
        private IImageProcessingService _imageProcessingService;
        public IImageProcessingService ImageProcessingService 
        {
            get => _imageProcessingService ??= new ImageProcessingService();
            set => SetProperty(ref _imageProcessingService, value);
        }

        private ImageFromFile _imageFromFile;

        public string OptionName
        {
            get => _optionName;
            set => SetProperty(ref _optionName, value);
        }
        public AllOptions AllOptions
        {
            get => _allOptions;
            set => SetProperty(ref _allOptions, value);
        }

        private DelegateCommand _openImageCommand;
        public DelegateCommand OpenImageCommand => _openImageCommand ??= new (OpenImageCommandHandler);

        private DelegateCommand _showVerticalDistributionCommand;
        public DelegateCommand ShowVerticalDistributionCommand => _showVerticalDistributionCommand ??= new(ShowVerticalDistributionCommandHandler);

        private DelegateCommand _showHistogramCommand;
        public DelegateCommand ShowHistogramCommand => _showHistogramCommand ??= new(ShowHistogramCommandHandler);

        private DelegateCommand<object> _setOptionsCommand;
        public DelegateCommand<object> SetOptionsCommand => _setOptionsCommand ??= new DelegateCommand<object>(SetOptionsCommandHandler);

        private DelegateCommand _saveOptionsCommand;
        public DelegateCommand SaveOptionsCommand => _saveOptionsCommand ??= new(SaveOptionsCommandHandler);

        private DelegateCommand _loadOptionsCommand;
        public DelegateCommand LoadOptionsCommand => _loadOptionsCommand ??= new(LoadOptionsCommandHandler);


        public MainWindowViewModel(IOpenImageFileService openImageFileService, IOpenOptionFileService openOptionFileService, IDialogService dialogService)
        {
            OpenImageFileService = openImageFileService;
            OpenOptionFileService = openOptionFileService;
            _dialogService = dialogService;
        }

        private void SetOptionsCommandHandler(object options)
            => ImageProcessingService.SetOptions(options);

        private void OpenImageCommandHandler()
        {
            if ((bool)OpenImageFileService.OpenFile())
            {
                ImageProcessingServices = new();

                _imageFromFile = new ImageFromFile(OpenImageFileService.FileNames[0]);
                ImageProcessingServices.Add(new ImageProcessingService());
                ImageProcessingServices.First().SetOrigMat(_imageFromFile.GetImageMat());
                ImageProcessingService = ImageProcessingServices.First();
            }
        }

        private void ShowVerticalDistributionCommandHandler()
            => _dialogService.ShowVerticalDistributionDialog(ImageProcessingService.GetContoursInfo(), OpenImageFileService.FileNameOnly, r => { });

        private void ShowHistogramCommandHandler()
            => _dialogService.ShowContoursInformationDialog(ImageProcessingService.GetContoursInfo(), OpenImageFileService.FileNameOnly, r => { });

        private void SaveOptionsCommandHandler()
        {
            _ = OptionsService.SaveOptionsAsync(AllOptions, OptionName);
        }

        private void LoadOptionsCommandHandler()
        {
            if ((bool)OpenOptionFileService.OpenFile())
            {
                AllOptions = OptionsService.LoadOptionsAsync(OpenOptionFileService.FileNames[0]);
            }
        }
    }
}
