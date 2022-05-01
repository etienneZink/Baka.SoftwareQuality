using System.Linq;
using Baka.ContactSplitter.framework;
using Baka.ContactSplitter.frontendModel;
using Baka.ContactSplitter.services.interfaces;
using Baka.ContactSplitter.view;
using Baka.ContactSplitter.viewModel;

namespace Baka.ContactSplitter.controller
{
    public class TitleController: BaseWindowController<TitleWindow, TitleWindowViewModel>
    {
        private ITitleService TitleService { get; }

        public TitleController(TitleWindow view, TitleWindowViewModel viewModel, ITitleService titleService): base(view, viewModel)
        {
            ViewModel.AddOrUpdateCommand = new RelayCommand(ExecuteAddOrUpdateCommand, CanExecuteAddOrUpdateCommand);
            ViewModel.DeleteCommand = new RelayCommand(ExecuteDeleteCommand, CanExecuteDeleteCommand);
            TitleService = titleService;
            LoadTitlesToTitleSalutations();
        }

        public void ExecuteAddOrUpdateCommand(object o)
        {
            TitleService.SaveOrUpdateTitle(ViewModel.Title, ViewModel.TitleSalutation);
            LoadTitlesToTitleSalutations();
            ViewModel.Title = string.Empty;
            ViewModel.TitleSalutation = string.Empty;
        }

        public bool CanExecuteAddOrUpdateCommand(object o)
        {
            return ViewModel.Title is not null && ViewModel.TitleSalutation is not null;
        }

        public void ExecuteDeleteCommand(object o)
        {
            var tts = ViewModel.Titles[ViewModel.SelectedTitleIndex];
            ViewModel.Titles.RemoveAt(ViewModel.SelectedTitleIndex);
            TitleService.DeleteTitle(tts.Title);
            ViewModel.Title = string.Empty;
            ViewModel.TitleSalutation = string.Empty;
        }

        public bool CanExecuteDeleteCommand(object o)
        {
            return ViewModel.SelectedTitleIndex != -1;
        }

        private void LoadTitlesToTitleSalutations()
        {
            ViewModel.Titles.Clear();
            var titleToTitleSalutations = TitleService
                .GetTitles()
                .Select(t => new TitleToTitleSalutation
                {
                    Title = t,
                    TitleSalutation = TitleService.GetTitleSalutation(t)
                });

            foreach (var tts in titleToTitleSalutations)
            {
                ViewModel.Titles.Add(tts);
            }
        }
    }
}