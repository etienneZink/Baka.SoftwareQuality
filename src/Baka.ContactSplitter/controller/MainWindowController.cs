using Baka.ContactSplitter.framework;
using Baka.ContactSplitter.services.interfaces;
using Baka.ContactSplitter.view;
using Baka.ContactSplitter.viewModel;
using System.Linq;

namespace Baka.ContactSplitter.controller
{
    public class MainWindowController: BaseWindowController<MainWindow, MainWindowViewModel>
    {
        private IParserService ParserService { get; }

        private ISalutationService SalutationService { get; }

        public MainWindowController(MainWindow view, MainWindowViewModel viewModel, IParserService parserService, ISalutationService salutationService): base(view, viewModel)
        {
            ViewModel.AddCommand = new RelayCommand(ExecuteAddCommand, CanExecuteAddCommand);
            ViewModel.DeleteCommand = new RelayCommand(ExecuteDeleteCommand, CanExecuteDeleteCommand);
            ViewModel.InputChanged += OnInputChanged;
            ParserService = parserService;
            SalutationService = salutationService;
        }

        public void ExecuteAddCommand(object o)
        {
            var parseResult = ParserService.ParseContact(ViewModel.Input);
            ViewModel.Contacts.Add(parseResult.Model);

            ViewModel.Input = string.Empty;
        }

        public bool CanExecuteAddCommand(object o)
        {
            return ViewModel.Input is not null && ParserService.ParseContact(ViewModel.Input) is not null &&
                   ParserService.ParseContact(ViewModel.Input).Successful;
        }

        public void ExecuteDeleteCommand(object o)
        {
            ViewModel.Contacts.RemoveAt(ViewModel.SelectedContactIndex);
        }

        public bool CanExecuteDeleteCommand(object o)
        {
            return ViewModel.SelectedContactIndex != -1;
        }

        private void OnInputChanged(string input)
        {
            var parseResult = ParserService.ParseContact(ViewModel.Input);

            if (parseResult.Successful)
            {
                var titles = parseResult.Model.Titles.Count > 0 ? parseResult.Model.Titles : new[] { string.Empty };

                ViewModel.ParsedSalutation = parseResult.Model.Salutation;
                ViewModel.ParsedTitles = titles.Aggregate((current, title) => $"{ current } { title }");
                ViewModel.ParsedFirstName = parseResult.Model.FirstName;
                ViewModel.ParsedLastName = parseResult.Model.LastName;
                ViewModel.ParsedGender = SalutationService.GetGender(parseResult.Model.Salutation).ToString();
            }
            else
            {
                ViewModel.ParsedSalutation = null;
                ViewModel.ParsedTitles = null;
                ViewModel.ParsedFirstName = null;
                ViewModel.ParsedLastName = null;
                ViewModel.ParsedGender = null;
            }
        }
    }
}