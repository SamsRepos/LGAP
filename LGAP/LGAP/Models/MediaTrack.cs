using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LGAP.Models;
public partial class MediaTrack : ObservableObject
{
    [ObservableProperty]
    private string path;

    [ObservableProperty]
    private int index;

    [ObservableProperty]
    private string name;

    [ObservableProperty]
    private bool shouldAutoPlay;

    public MediaElement MediaElem { get; set; }
    
    public MediaTrack(string path, int index, ref CollectionView cv)
    {
        Path = path;
        ShouldAutoPlay = (index == 0);

        name = $"{nameof(MediaTrack)}{index}";

        MediaElem = new MediaElement();
        MediaElem.Source = path;

        cv.AddLogicalChild(MediaElem);
    }
}

