using System.Linq;
using Baka.ContactSplitter.framework;
using Baka.ContactSplitter.frontendModel;
using Baka.ContactSplitter.services.interfaces;
using Baka.ContactSplitter.view;
using Baka.ContactSplitter.viewModel;

namespace Baka.ContactSplitter.controller
{
    /// <summary>
    /// Class which is used as the controller for the TitleWindowViewModel and the TitleWindow.
    /// This controller combines these two and defines the logic for methods of the TitleWindowViewModel.
    /// </summary>
    public class TitleController: BaseWindowController<TitleWindow, TitleWindowViewModel>
    {
        private ITitleService TitleService { get; }

        public TitleController(TitleWindow view, TitleWindowViewModel viewModel, ITitleService titleService): base(view, viewModel)
        {
            ViewModel.AddOrUpdateCommand = new RelayCommand(ExecuteAddOrUpdateCommand, CanExecuteAddOrUpdateCommand);
            ViewModel.DeleteCommand = new RelayCommand(ExecuteDeleteCommand, CanExecuteDeleteCommand);
            TitleService = titleService;
            LoadTitlesToTitleSalutations();

            ViewModel.SelectedTitleIndexChanged += OnSelectedTitleIndexChanged;
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
            //add can be executed, if the title and titleSalutation text is not null
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
            //delete can be executed, if a title is selected (index != -1)
            return ViewModel.SelectedTitleIndex != -1;
        }

        //fetches all titles and titleSalutations from the TitleService and adds them to the list of TitleToTitleSalutation
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

        private void OnSelectedTitleIndexChanged(int selectedTitleIndex)
        {
            if (selectedTitleIndex >= 0 && selectedTitleIndex < ViewModel.Titles.Count)
            {
                ViewModel.Title = ViewModel.Titles[selectedTitleIndex].Title;
                ViewModel.TitleSalutation = ViewModel.Titles[selectedTitleIndex].TitleSalutation;
            }
            else
            {
                ViewModel.Title = string.Empty;
                ViewModel.TitleSalutation = string.Empty;
            }
        }
    }
}