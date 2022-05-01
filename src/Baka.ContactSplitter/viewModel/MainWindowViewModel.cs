using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Baka.ContactSplitter.framework;
using Baka.ContactSplitter.model;
using Baka.ContactSplitter.services.interfaces;
using JetBrains.Annotations;

namespace Baka.ContactSplitter.viewModel
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private IParserService ParserService { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand AddCommand => new RelayCommand(ExecuteAddCommand, CanExecuteAddCommand);
        public ICommand DeleteCommand => new RelayCommand(ExecuteDeleteCommand, CanExecuteDeleteCommand);

        public MainWindowViewModel(IParserService parserService)
        {
            ParserService = parserService;
        }


        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ObservableCollection<Contact> Contacts { get; set; } = new();

        private string _input;
        public string Input
        {
            get => _input;
            set
            {
                if (_input == value) return;
                OnPropertyChanged(nameof(Input));
                _input = value;
            }
        }

        private int _selectedContactIndex = -1;

        public int SelectedContactIndex
        {
            get => _selectedContactIndex;
            set
            {
                if (_selectedContactIndex == value) return;
                _selectedContactIndex = value;
                OnPropertyChanged(nameof(SelectedContactIndex));
                //ToDo
            }
        }

        public void ExecuteAddCommand(object o)
        {
            var parseResult = ParserService.ParseContact(Input);
            Contacts.Add(parseResult.Model);
        }

        public bool CanExecuteAddCommand(object o)
        {
            return Input is not null && ParserService.ParseContact(Input) is not null &&
                   ParserService.ParseContact(Input).Successful;
        }

        public void ExecuteDeleteCommand(object o)
        {
            Contacts.RemoveAt(SelectedContactIndex);
        }

        public bool CanExecuteDeleteCommand(object o)
        {
            return SelectedContactIndex != -1;
        }
    }
}