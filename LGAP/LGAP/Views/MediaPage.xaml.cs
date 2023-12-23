using CommunityToolkit.Maui.Views;
using LGAP.ViewModels;

namespace LGAP.Views;

public partial class MediaPage : ContentPage
{
    private readonly MediaViewModel _vm;

    //private MediaElement mediaElem;
    public MediaPage(MediaViewModel vm)
    {
        BindingContext = vm;
        _vm = vm;
        InitializeComponent();
    }
    
    protected override void OnAppearing()
    {
        base.OnAppearing();
        _vm.Init(ref mediaCollectionView);
        
    }

    private void UpdateTrackPositionText(object s, EventArgs e)
    {
        _vm.UpdateTrackPositionText();
    }

    private void MediaEnded(object sender, EventArgs e)
    {
        //_vm.MediaEnded(ref mediaCollectionView);
        
    }
}