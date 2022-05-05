using System;
using System.Linq;
using Autofac;
using Baka.ContactSplitter.Framework;
using Baka.ContactSplitter.Model;
using Baka.ContactSplitter.Services.Interfaces;
using Baka.ContactSplitter.View;
using Baka.ContactSplitter.ViewModel;

namespace Baka.ContactSplitter.Controller
{
    /// <summary>
    /// Class which is used as the controller for the MainWindowViewModel and the MainWindow.
    /// This controller combines these two and defines the logic for methods of the MainWindowViewModel.
    /// </summary>
    public class MainWindowController: BaseWindowController<MainWindow, MainWindowViewModel>
    {
        private IParserService ParserService { get; }

        private ILetterSalutationService LetterSalutationService { get; }

        private ISalutationService SalutationService { get; }

        private App App { get; }

        public MainWindowController(MainWindow view, MainWindowViewModel viewModel, IParserService parserService, ILetterSalutationService letterSalutationService, ISalutationService salutationService, App app) : base(view, viewModel)
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

            if (letterSalutationService is null)
            {
                throw new ArgumentNullException(nameof(letterSalutationService));
            }

            LetterSalutationService = letterSalutationService;

            if (salutationService is null)
            {
                throw new ArgumentNullException(nameof(salutationService));
            }

            SalutationService = salutationService;

            if (app is null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            App = app;

            ViewModel.InputChanged += OnInputChanged;
            ViewModel.SelectedContactIndexChanged += OnSelectedContactIndexChanged;
        }

        public void ExecuteTitlesCommand(object o)
        {
            App.Container.Resolve<TitleController>().Show();
        }

        public void ExecuteSalutationsCommand(object o)
        {
            App.Container.Resolve<SalutationController>().Show();
        }

        public void ExecuteAddCommand(object o)
        {
            var parserResult = ParserService.ParseContact(ViewModel.Input);
            var newContact = parserResult.Model;
            newContact.FirstName = ViewModel.SelectedContactFirstName;
            newContact.LastName = ViewModel.SelectedContactLastName;
            ViewModel.Contacts.Add(newContact);
            ViewModel.Input = string.Empty;
            ResetPreview();
        }

        public bool CanExecuteAddCommand(object o)
        {
            //add can be executed, if the input can be parsed successfully
            return ViewModel.Input is not null && ParserService.ParseContact(ViewModel.Input) is not null &&
                   ParserService.ParseContact(ViewModel.Input).Successful;
        }

        public void ExecuteDeleteCommand(object o)
        {
            ViewModel.Contacts.RemoveAt(ViewModel.SelectedContactIndex);
        }

        public bool CanExecuteDeleteCommand(object o)
        {
            //delete can be executed, if a contact is selected (index != -1)
            return ViewModel.SelectedContactIndex != -1;
        }

        public void OnInputChanged(string input)
        {

            if (input == string.Empty)
            {
                ViewModel.ErrorMessage = string.Empty;
                return;
            }

            var parseResult = ParserService.ParseContact(input);
            if (parseResult.Successful)
            {
                ViewModel.SelectedContactSalutation = parseResult.Model.Salutation;
                ViewModel.SelectedContactTitles = parseResult.Model.Titles.Count > 0 ?
                    parseResult.Model.Titles.Aggregate((current, title) => $"{current} {title}") :
                    string.Empty;
                ViewModel.SelectedContactFirstName = parseResult.Model.FirstName;
                ViewModel.SelectedContactLastName = parseResult.Model.LastName;
                ViewModel.SelectedContactGender = SalutationService.GetGender(parseResult.Model.Salutation).ToGermanString();
                ViewModel.SelectedContactLetterSalutation =
                    LetterSalutationService.GenerateLetterSalutation(parseResult.Model);
                ViewModel.ErrorMessage = string.Empty;
            }
            else
            {
                ResetPreview();
                ViewModel.ErrorMessage = parseResult.ErrorMessages[0];
            }
        }

        public void OnSelectedContactIndexChanged(int selectedContactIndex)
        {
            if (selectedContactIndex >= 0 && selectedContactIndex < ViewModel.Contacts.Count)
            {
                ViewModel.SelectedContactSalutation = ViewModel.ViewModelContacts[selectedContactIndex].Salutation;
                ViewModel.SelectedContactTitles = ViewModel.ViewModelContacts[selectedContactIndex].Titles;
                ViewModel.SelectedContactFirstName = ViewModel.ViewModelContacts[selectedContactIndex].FirstName;
                ViewModel.SelectedContactLastName = ViewModel.ViewModelContacts[selectedContactIndex].LastName;
                ViewModel.SelectedContactGender = ViewModel.ViewModelContacts[selectedContactIndex].Gender;
                ViewModel.SelectedContactLetterSalutation =
                    LetterSalutationService.GenerateLetterSalutation(ViewModel.Contacts[selectedContactIndex]);
            }
            else
            {
                ResetPreview();
            }
        }

        private void ResetPreview()
        {
            ViewModel.SelectedContactSalutation = string.Empty;
            ViewModel.SelectedContactTitles = string.Empty;
            ViewModel.SelectedContactFirstName = string.Empty;
            ViewModel.SelectedContactLastName = string.Empty;
            ViewModel.SelectedContactGender = string.Empty;
            ViewModel.SelectedContactLetterSalutation = string.Empty;
            ViewModel.ErrorMessage = string.Empty;
        }
    }
}