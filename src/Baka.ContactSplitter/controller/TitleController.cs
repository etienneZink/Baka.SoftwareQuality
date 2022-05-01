using System.Linq;
using Baka.ContactSplitter.framework;
using Baka.ContactSplitter.frontendModel;
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
                ViewModel.Titles.Add(new TitleToTitleSalutation
                {
                    Title = title,
                    TitleSalutation = TitleService.GetTitleSalutation(title)
                });
            }
        }

        public void Initialize()
        {
            View.ShowDialog();
        }

        public void ExecuteAddOrUpdateCommand(object o)
        {
            TitleService.SaveOrUpdateTitle(ViewModel.Title, ViewModel.TitleSalutation);

            if (ViewModel.Titles.All(tts => tts.Title != ViewModel.Title))
            {
                ViewModel.Titles.Add(new TitleToTitleSalutation
                {
                    Title = View.Title,
                    TitleSalutation = TitleService.GetTitleSalutation(ViewModel.Title)
                });
            }
            else
            {
                var tts = ViewModel.Titles.FirstOrDefault(tts => tts.Title == ViewModel.Title);
                if(tts is not null) tts.TitleSalutation = ViewModel.TitleSalutation;
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
    }
}