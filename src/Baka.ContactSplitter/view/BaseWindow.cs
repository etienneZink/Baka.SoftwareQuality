using System.Security.RightsManagement;
using System.Windows;

namespace Baka.ContactSplitter.view
{
    public partial class BaseWindow<TViewModel>: Window
    {
        public BaseWindow(TViewModel viewModel)
        {
            DataContext = viewModel;
        }
    }
}