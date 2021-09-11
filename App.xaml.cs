using SampleSegmenter.Views;
using Prism.Ioc;
using Prism.DryIoc;
using System.Windows;
using SampleSegmenter.Services;
using SampleSegmenter.Interfaces;
using SampleSegmenter.Dialogs.Views;
using SampleSegmenter.Dialogs.ViewModels;

namespace SampleSegmenter
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override Window CreateShell()
            => Container.Resolve<MainWindow>();

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterManySingleton<OpenFileService>(typeof(IOpenFileService));
            containerRegistry.RegisterDialog<VerticalDistributionDialogView, VerticalDistributionDialogViewModel>();
            containerRegistry.RegisterDialog<ContoursInformationDialogView, ContoursInformationDialogViewModel>();
        }
    }
}
