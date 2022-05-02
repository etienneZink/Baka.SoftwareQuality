using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Baka.ContactSplitter.framework;
using Baka.ContactSplitter.frontendModel;
using Baka.ContactSplitter.model;
using Baka.ContactSplitter.services.interfaces;

namespace Baka.ContactSplitter.viewModel
{
    public class MainWindowViewModel : BaseViewModel
    {
        public delegate void InputChange(string input);

        public event InputChange InputChanged;

        public ISalutationService SalutationService { get; }

        public ICommand AddCommand { get; set; }
        public ICommand DeleteCommand { get; set; }

        public MainWindowViewModel(ISalutationService salutationService)
        {
            if (salutationService is null)
            {
                throw new ArgumentNullException(nameof(salutationService));
            }

            SalutationService = salutationService;
            Contacts.CollectionChanged += (sender, args) => OnPropertyChanged(nameof(ViewModelContacts));
        }

        private ObservableCollection<Contact> _Contacts;
        public ObservableCollection<Contact> Contacts => _Contacts ??= new ObservableCollection<Contact>();

        private string _input;
        public string Input
        {
            get => _input;
            set 
            { 
                SetField(ref _input, value);

                InputChanged?.Invoke(_input);
            }
        }

        private int _selectedContactIndex = -1;

        public int SelectedContactIndex
        {
            get => _selectedContactIndex;
            set => SetField(ref _selectedContactIndex, value);
        }

        public IEnumerable<ViewModelContact> ViewModelContacts
        {
            get
            {
                return Contacts.Select(contact =>
                {
                    var titles = contact.Titles.Count > 0 ? contact.Titles : new[] { string.Empty };

                    return new ViewModelContact
                    {
                        FirstName = contact.FirstName,
                        LastName = contact.LastName,
                        Salutation = contact.Salutation,
                        Titles = titles.Aggregate((current, title) => $"{ current } { title }"),
                        Gender = SalutationService.GetGender(contact.Salutation).ToString()
                    };
                });
            }
        }

        private string _ParsedSalutation;
        public string ParsedSalutation
        {
            get => _ParsedSalutation;
            set
            {
                SetField(ref _ParsedSalutation, value);

                OnPropertyChanged(nameof(ParsedContactDetailsVisible));
            }
        }

        private string _ParsedTitles;
        public string ParsedTitles
        {
            get => _ParsedTitles;
            set => SetField(ref _ParsedTitles, value);
        }

        private string _ParsedFirstName;
        public string ParsedFirstName
        {
            get => _ParsedFirstName;
            set => SetField(ref _ParsedFirstName, value);
        }

        private string _ParsedLastName;
        public string ParsedLastName
        {
            get => _ParsedLastName;
            set => SetField(ref _ParsedLastName, value);
        }

        private string _ParsedGender;
        public string ParsedGender
        {
            get => _ParsedGender;
            set => SetField(ref _ParsedGender, value);
        }

        public Visibility ParsedContactDetailsVisible => ParsedSalutation is null ? Visibility.Collapsed : Visibility.Visible;
    }
}