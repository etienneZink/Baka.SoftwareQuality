using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Baka.ContactSplitter.frontendModel;
using Baka.ContactSplitter.model;
using Baka.ContactSplitter.services.interfaces;

namespace Baka.ContactSplitter.viewModel
{
    /// <summary>
    /// Class which is used as a viewModel for the MainWindow.
    /// </summary>
    public class MainWindowViewModel : BaseViewModel
    {
        public ISalutationService SalutationService { get; }
        public ILetterSalutationService LetterSalutationService { get; }
        public IParserService ParserService { get; }

        public ICommand AddCommand { get; set; }
        public ICommand DeleteCommand { get; set; }

        public ICommand TitlesCommand { get; set; }
        public ICommand SalutationsCommand { get; set; }

        public MainWindowViewModel(ISalutationService salutationService,
            ILetterSalutationService letterSalutationService, IParserService parserService)
        {
            if (salutationService is null)
            {
                throw new ArgumentNullException(nameof(salutationService));
            }

            SalutationService = salutationService;

            if (letterSalutationService is null)
            {
                throw new ArgumentNullException(nameof(letterSalutationService));
            }

            LetterSalutationService = letterSalutationService;

            if (parserService is null)
            {
                throw new ArgumentNullException(nameof(parserService));
            }

            ParserService = parserService;
            Contacts.CollectionChanged += (sender, args) => OnPropertyChanged(nameof(ViewModelContacts));
        }

        // property which is used to store the saved Contact
        private ObservableCollection<Contact> _contacts;
        public ObservableCollection<Contact> Contacts => _contacts ??= new ObservableCollection<Contact>();

        // property which is used to display an error message
        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetField(ref _errorMessage, value);
        }

        // property which represents the input of an user
        private string _input;
        public string Input
        {
            get => _input;
            set
            {
                SetField(ref _input, value);
                if (Input == string.Empty)
                {
                    ErrorMessage = string.Empty;
                    return;
                }

                var parseResult = ParserService.ParseContact(Input);
                if (parseResult.Successful)
                {
                    SelectedContactSalutation = parseResult.Model.Salutation;
                    SelectedContactTitles =  parseResult.Model.Titles.Count > 0 ?
                        parseResult.Model.Titles.Aggregate((current, title) => $"{current} {title}"):
                        string.Empty;
                    SelectedContactFirstName = parseResult.Model.FirstName;
                    SelectedContactLastName = parseResult.Model.LastName;
                    SelectedContactGender = SalutationService.GetGender(parseResult.Model.Salutation).ToString();
                    SelectedContactLetterSalutation =
                        LetterSalutationService.GenerateLetterSalutation(parseResult.Model);
                    ErrorMessage = string.Empty;
                }
                else
                {
                    ResetPreview();
                    ErrorMessage = parseResult.ErrorMessages[0];
                }
            }
        }

        // property which represents the index of the selected contact in the list of ViewModeContact 
        private int _selectedContactIndex = -1;
        public int SelectedContactIndex
        {
            get => _selectedContactIndex;
            set
            {
                SetField(ref _selectedContactIndex, value);
                if (SelectedContactIndex >= 0 && SelectedContactIndex < Contacts.Count)
                {
                    SelectedContactSalutation = ViewModelContacts[SelectedContactIndex].Salutation;
                    SelectedContactTitles = ViewModelContacts[SelectedContactIndex].Titles;
                    SelectedContactFirstName = ViewModelContacts[SelectedContactIndex].FirstName;
                    SelectedContactLastName = ViewModelContacts[SelectedContactIndex].LastName;
                    SelectedContactGender = ViewModelContacts[SelectedContactIndex].Gender;
                    SelectedContactLetterSalutation =
                        LetterSalutationService.GenerateLetterSalutation(Contacts[SelectedContactIndex]);
                }
                else
                {
                    ResetPreview();
                }
            }
        }

        // property used in the preview for the salutation
        private string _selectedContactSalutation;
        public string SelectedContactSalutation
        {
            get => _selectedContactSalutation;
            set => SetField(ref _selectedContactSalutation, value);
        }

        // property used in the preview for the titles
        private string _selectedContactTitles;
        public string SelectedContactTitles
        {
            get => _selectedContactTitles;
            set => SetField(ref _selectedContactTitles, value);
        }

        // property used in the preview for the firstname
        private string _selectedContactFirstName;
        public string SelectedContactFirstName
        {
            get => _selectedContactFirstName;
            set => SetField(ref _selectedContactFirstName, value);
        }

        // property used in the preview for the lastname
        private string _selectedContactLastName;
        public string SelectedContactLastName
        {
            get => _selectedContactLastName;
            set => SetField(ref _selectedContactLastName, value);
        }

        // property used in the preview for the gender
        private string _selectedContactGender;
        public string SelectedContactGender
        {
            get => _selectedContactGender;
            set => SetField(ref _selectedContactGender, value);
        }

        // property used in the preview for the letter salutation
        private string _selectedContactLetterSalutation;
        public string SelectedContactLetterSalutation
        {
            get => _selectedContactLetterSalutation;
            set => SetField(ref _selectedContactLetterSalutation, value);
        }

        // property which is used to view the list of Contact (Contacts) as a list of ViewModelContact
        public ObservableCollection<ViewModelContact> ViewModelContacts
        {
            get
            {
                return new ObservableCollection<ViewModelContact>(Contacts.Select(contact =>
                {
                    var titles = contact.Titles.Count > 0 ? contact.Titles : new[] { string.Empty };

                    return new ViewModelContact
                    {
                        FirstName = contact.FirstName,
                        LastName = contact.LastName,
                        Salutation = contact.Salutation,
                        Titles = titles.Aggregate((current, title) => $"{current} {title}"),
                        Gender = SalutationService.GetGender(contact.Salutation).ToString()
                    };
                }));
            }
        }

        public void ResetPreview()
        {
            SelectedContactSalutation = string.Empty;
            SelectedContactTitles = string.Empty;
            SelectedContactFirstName = string.Empty;
            SelectedContactLastName = string.Empty;
            SelectedContactGender = string.Empty;
            SelectedContactLetterSalutation = string.Empty;
            ErrorMessage = string.Empty;
        }
    }
}