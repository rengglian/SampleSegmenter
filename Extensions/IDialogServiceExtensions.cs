﻿using Prism.Services.Dialogs;
using SampleSegmenter.Models;
using System;
using System.Collections.Generic;

namespace SampleSegmenter.Extensions
{
    public static class IDialogServiceExtensions
    {
        public static void ShowHistogramDialog(this IDialogService dialogService, List<ContourInfo> contoursInfo, Action<IDialogResult> callback)
        {
            var p = new DialogParameters
            {
                { "contoursInfo", contoursInfo }
            };

            dialogService.ShowDialog("HistogramDialogView", p, callback);
        }
    }
}
