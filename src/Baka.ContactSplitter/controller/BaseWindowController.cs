using Baka.ContactSplitter.View;
using Baka.ContactSplitter.ViewModel;

namespace Baka.ContactSplitter.Controller
{
    /// <summary>
    /// Base class for all controllers.
    /// </summary>
    /// <typeparam name="TView"></typeparam>
    /// <typeparam name="TViewModel"></typeparam>
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