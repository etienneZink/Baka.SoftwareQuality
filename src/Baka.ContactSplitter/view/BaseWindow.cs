using System.Windows;
using Baka.ContactSplitter.ViewModel;

namespace Baka.ContactSplitter.View
{
    public partial class BaseWindow<TViewModel>: Window where TViewModel: BaseViewModel
    {
    }
}