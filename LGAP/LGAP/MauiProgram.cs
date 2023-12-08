﻿using LGAP.ViewModels;
using LGAP.Views;
using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;

namespace LGAP
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .UseMauiCommunityToolkitMediaElement()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
            builder.Services.AddSingleton<SourcePage>();
            builder.Services.AddSingleton<SourceViewModel>();

            builder.Services.AddTransient<MediaPage>();
            builder.Services.AddTransient<MediaViewModel>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
