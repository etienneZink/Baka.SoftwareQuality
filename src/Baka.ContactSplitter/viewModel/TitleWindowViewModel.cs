using System.Collections.ObjectModel;
using System.Windows.Input;
using Baka.ContactSplitter.frontendModel;

namespace Baka.ContactSplitter.viewModel
{
    /// <summary>
    /// Class which is used as a viewModel for the TitleWindow.
    /// </summary>
    public class TitleWindowViewModel : BaseViewModel
    {

        public ICommand AddOrUpdateCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        
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