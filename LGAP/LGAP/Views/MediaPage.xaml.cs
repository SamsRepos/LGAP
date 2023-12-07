using LGAP.ViewModels;

namespace LGAP.Views;

public partial class MediaPage : ContentPage
{
    public MediaPage(MediaViewModel vm)
    {
        BindingContext = vm;
        InitializeComponent();
    }
}