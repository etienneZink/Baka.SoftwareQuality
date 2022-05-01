using System.Security.RightsManagement;
using System.Windows;
using Baka.ContactSplitter.viewModel;

namespace Baka.ContactSplitter.view
{
    public partial class BaseWindow<TViewModel>: Window where TViewModel: BaseViewModel
    {
    }
}