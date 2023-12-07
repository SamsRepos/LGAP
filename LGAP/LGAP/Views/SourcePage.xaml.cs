using LGAP.ViewModels;

namespace LGAP.Views;

public partial class SourcePage : ContentPage
{
    public SourcePage(SourceViewModel vm)
    {
        BindingContext = vm;
        InitializeComponent();
    }
}
