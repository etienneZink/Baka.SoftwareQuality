using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Baka.ContactSplitter.frontendModel;
using Baka.ContactSplitter.services.interfaces;
using JetBrains.Annotations;

namespace Baka.ContactSplitter.viewModel
{
    public class TitleWindowViewModel : INotifyPropertyChanged
    {
        

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand AddOrUpdateCommand { get; set; }
        public ICommand DeleteCommand { get; set; }

        public ITitleService TitleService { get; set; }


        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ObservableCollection<TitleToTitleSalutation> Titles { get; set; } = new();

        private string _titleSalutation;

        public string TitleSalutation
        {
            get => _titleSalutation;
            set
            {
                if (_titleSalutation == value) return;
                _titleSalutation = value;
                OnPropertyChanged(nameof(TitleSalutation));
            }
        }

        private string _title;

        public string Title
        {
            get => _title;
            set
            {
                if (_title == value) return;
                _title = value;
                OnPropertyChanged(nameof(Title));
            }
        }


        private int _selectedTitleIndex = -1;

        public int SelectedTitleIndex
        {
            get => _selectedTitleIndex;
            set
            {
                if (_selectedTitleIndex == value) return;
                _selectedTitleIndex = value;
                OnPropertyChanged(nameof(SelectedTitleIndex));
                if (SelectedTitleIndex >= 0 && SelectedTitleIndex < Titles.Count)
                {
                    Title = Titles[_selectedTitleIndex].Title;
                    TitleSalutation = Titles[_selectedTitleIndex].TitleSalutation;
                }
                else
                {
                    Title = string.Empty;
                    TitleSalutation = string.Empty;
                }
            }
        }
    }
}