using CommunityToolkit.Maui.Views;
using LGAP.ViewModels;
using Microsoft.Maui.Platform;

namespace LGAP.Views;

public partial class MediaPage : ContentPage
{
    private MediaElement mediaElem;
    public MediaPage(MediaViewModel vm)
    {
        BindingContext = vm;

        mediaElem = new MediaElement();
        mediaElem.IsVisible = false;

        InitializeComponent();
    }
}