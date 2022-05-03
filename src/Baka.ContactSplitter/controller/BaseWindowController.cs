using System.Windows;
using System.Windows.Navigation;
using Baka.ContactSplitter.view;
using Baka.ContactSplitter.viewModel;

namespace Baka.ContactSplitter.controller
{
    public class BaseWindowController<TView, TViewModel> where TView : BaseWindow<TViewModel>
                                                         where TViewModel : BaseViewModel
    {
        protected TView View { get; }
        protected TViewModel ViewModel { get; }

        public BaseWindowController(TView view, TViewModel viewModel)
        {
            View = view;
            ViewModel = viewModel;
            View.DataContext = ViewModel;
        }

        public virtual bool? Show() => View.ShowDialog();
    }
}