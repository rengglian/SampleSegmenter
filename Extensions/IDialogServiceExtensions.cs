using Prism.Services.Dialogs;
using SampleSegmenter.Interfaces;
using System;
using System.Collections.Generic;

namespace SampleSegmenter.Extensions
{
    public static class IDialogServiceExtensions
    {
        public static void ShowVerticalDistributionDialog(this IDialogService dialogService, List<IContourInfo> contoursInfo, string fileName, Action<IDialogResult> callback)
        {
            var p = new DialogParameters
            {
                { "contoursInfo", contoursInfo },
                { "fileName", fileName }
            };

            dialogService.ShowDialog("VerticalDistributionDialogView", p, callback);
        }

        public static void ShowContoursInformationDialog(this IDialogService dialogService, List<IContourInfo> contoursInfo, string fileName, Action<IDialogResult> callback)
        {
            var p = new DialogParameters
            {
                { "contoursInfo", contoursInfo },
                { "fileName", fileName }
            };

            dialogService.ShowDialog("ContoursInformationDialogView", p, callback);
        }

    }
}
