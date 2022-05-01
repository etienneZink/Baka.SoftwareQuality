using Baka.ContactSplitter.framework;
using Baka.ContactSplitter.services.interfaces;
using Baka.ContactSplitter.view;
using Baka.ContactSplitter.viewModel;

namespace Baka.ContactSplitter.controller
{
    public class TitleController
    {
        private TitleWindow View { get; }
        private TitleWindowViewModel ViewModel { get; }

        private ITitleService TitleService { get; }

        public TitleController(TitleWindow view, TitleWindowViewModel viewModel, ITitleService titleService)
        {
            View = view;
            ViewModel = viewModel;
            ViewModel.AddOrUpdateCommand = new RelayCommand(ExecuteAddOrUpdateCommand, CanExecuteAddOrUpdateCommand);
            ViewModel.DeleteCommand = new RelayCommand(ExecuteDeleteCommand, CanExecuteDeleteCommand);
            ViewModel.TitleService = titleService;
            View.DataContext = ViewModel;
            TitleService = titleService;

            foreach (var title in TitleService.GetTitles())
            {
                ViewModel.Titles.Add(title);
            }
        }

        public void Initialize()
        {
            View.ShowDialog();
        }

        public void ExecuteAddOrUpdateCommand(object o)
        {
            TitleService.SaveOrUpdateTitle(ViewModel.Title, ViewModel.TitleSalutation);
            if (!ViewModel.Titles.Contains(ViewModel.Title))
            {
                ViewModel.Titles.Add(ViewModel.Title);
            }

            ViewModel.Title = string.Empty;
            ViewModel.TitleSalutation = string.Empty;
        }

        public bool CanExecuteAddOrUpdateCommand(object o)
        {
            return ViewModel.Title is not null && ViewModel.TitleSalutation is not null;
        }

        public void ExecuteDeleteCommand(object o)
        {
            var title = ViewModel.Titles[ViewModel.SelectedTitleIndex];
            ViewModel.Titles.RemoveAt(ViewModel.SelectedTitleIndex);
            TitleService.DeleteTitle(title);
            ViewModel.Title = string.Empty;
            ViewModel.TitleSalutation = string.Empty;
        }

        public bool CanExecuteDeleteCommand(object o)
        {
            return ViewModel.SelectedTitleIndex != -1;
        }
    }
}