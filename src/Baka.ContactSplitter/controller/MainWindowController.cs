using Baka.ContactSplitter.framework;
using Baka.ContactSplitter.services.interfaces;
using Baka.ContactSplitter.view;
using Baka.ContactSplitter.viewModel;

namespace Baka.ContactSplitter.controller
{
    public class MainWindowController: BaseWindowController<MainWindow, MainWindowViewModel>
    {
        private IParserService ParserService { get; }

        public MainWindowController(MainWindow view, MainWindowViewModel viewModel, IParserService parserService): base(view, viewModel)
        {
            ViewModel.AddCommand = new RelayCommand(ExecuteAddCommand, CanExecuteAddCommand);
            ViewModel.DeleteCommand = new RelayCommand(ExecuteDeleteCommand, CanExecuteDeleteCommand);
            ParserService = parserService;
        }

        public void ExecuteAddCommand(object o)
        {
            var parseResult = ParserService.ParseContact(ViewModel.Input);
            ViewModel.Contacts.Add(parseResult.Model);
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
    }
}