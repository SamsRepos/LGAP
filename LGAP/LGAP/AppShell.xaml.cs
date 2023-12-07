using LGAP.Views;

namespace LGAP
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(
                nameof(MediaPage),
                typeof(MediaPage)
            );
        }
    }
}
