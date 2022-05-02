using System;
using Baka.ContactSplitter.framework;
using Baka.ContactSplitter.model;
using Baka.ContactSplitter.services.interfaces;
using Baka.ContactSplitter.view;
using Baka.ContactSplitter.viewModel;

namespace Baka.ContactSplitter.controller
{
    public class MainWindowController: BaseWindowController<MainWindow, MainWindowViewModel>
    {
        public IParserService ParserService { get; }
        private TitleController TitleController { get; }

        private SalutationController SalutationController { get; }

        public MainWindowController(MainWindow view, MainWindowViewModel viewModel, IParserService parserService, TitleController titleController,
            SalutationController salutationController) : base(view, viewModel)
        {
            ViewModel.AddCommand = new RelayCommand(ExecuteAddCommand, CanExecuteAddCommand);
            ViewModel.DeleteCommand = new RelayCommand(ExecuteDeleteCommand, CanExecuteDeleteCommand);
            ViewModel.TitlesCommand = new RelayCommand(ExecuteTitlesCommand);
            ViewModel.SalutationsCommand = new RelayCommand(ExecuteSalutationsCommand);

            if (parserService is null)
            {
                throw new ArgumentNullException(nameof(parserService));
            }

            ParserService = parserService;

            if (titleController is null)
            {
                throw new ArgumentNullException(nameof(titleController));
            }

            TitleController = titleController;

            if (salutationController is null)
            {
                throw new ArgumentNullException(nameof(salutationController));
            }

            SalutationController = salutationController;
        }

        public void ExecuteTitlesCommand(object o)
        {
            TitleController.Initialize();
        }

        public void ExecuteSalutationsCommand(object o)
        {
            SalutationController.Initialize();
        }

        public void ExecuteAddCommand(object o)
        {
            var parserResult = ParserService.ParseContact(ViewModel.Input);
            var newContact = parserResult.Model;
            newContact.FirstName = ViewModel.SelectedContactFirstName;
            newContact.LastName = ViewModel.SelectedContactLastName;
            ViewModel.Contacts.Add(newContact);
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