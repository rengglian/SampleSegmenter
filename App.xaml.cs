using SampleSegmenter.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using Prism.Unity;
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
        private static IOpenFileService _openFileService;
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            _openFileService = new OpenFileService();
            containerRegistry.RegisterInstance(_openFileService);
            containerRegistry.RegisterDialog<HistogramDialogView, HistogramDialogViewModel>();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
        
        }

        protected override void ConfigureRegionAdapterMappings(RegionAdapterMappings regionAdapterMappings)
        {
         
        }

        protected override void ConfigureDefaultRegionBehaviors(IRegionBehaviorFactory regionBehaviors)
        {
         
        }
    }
}
