using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Baka.ContactSplitter.FrontendModel;

namespace Baka.ContactSplitter.ViewModel
{
    /// <summary>
    /// Class which is used as a viewModel for the TitleWindow.
    /// </summary>
    public class TitleWindowViewModel : BaseViewModel
    {

        public ICommand AddOrUpdateCommand { get; set; }
        public ICommand DeleteCommand { get; set; }

        public event Action<int> SelectedTitleIndexChanged;
        
        public ObservableCollection<TitleToTitleSalutation> Titles { get; set; } = new();

        // property which represents the titleSalutation for the title to be added/updated or deleted
        private string _titleSalutation;
        public string TitleSalutation
        {
            get => _titleSalutation;
            set => SetField(ref _titleSalutation, value);
        }

        // property which represents the title to be added/updated or deleted
        private string _title;
        public string Title
        {
            get => _title;
            set => SetField(ref _title, value);
        }

        // property which represents the selected title index 
        private int _selectedTitleIndex = -1;
        public int SelectedTitleIndex
        {
            get => _selectedTitleIndex;
            set
            {
                SetField(ref _selectedTitleIndex, value);

                SelectedTitleIndexChanged?.Invoke(SelectedTitleIndex);
            }
        }
    }
}