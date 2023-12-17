﻿using LGAP.ViewModels;

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

        if(mediaElem is not null)
        {
            _vm.Init(ref mediaElem);
        }
    }

    private void UpdateTrackPositionText(object s, EventArgs e)
    {
        _vm.UpdateTrackPositionText();
    }
}