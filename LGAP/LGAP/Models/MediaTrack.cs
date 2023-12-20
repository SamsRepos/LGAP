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
    
    public MediaTrack(string path, int index)
    {
        Path = path;
        ShouldAutoPlay = (index == 0);

        name = $"{nameof(MediaTrack)}{index}";
    }
}

