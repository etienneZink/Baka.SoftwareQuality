using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Baka.ContactSplitter.FrontendModel;
using Baka.ContactSplitter.Model;

namespace Baka.ContactSplitter.ViewModel
{
    /// <summary>
    /// Class which is used as a viewModel for the SalutationWindow.
    /// </summary>
    public class SalutationWindowViewModel : BaseViewModel
    {

        public ICommand AddOrUpdateCommand { get; set; }
        public ICommand DeleteCommand { get; set; }

        public event Action<int> SelectedSalutationIndexChanged;

        public ObservableCollection<SalutationToGender> Salutations { get; } = new();

        // property which represents all possible genders
        private ObservableCollection<string> _genders;
        public ObservableCollection<string> Genders => _genders ??= new (Enum.GetValues<Gender>().Select(gender => gender.ToGermanString()));

        // property which represents the gender for the salutation to be added/updated or deleted
        private Gender _gender = Model.Gender.Neutral;
        public string Gender
        {
            get => _gender.ToGermanString();
            set => SetField(ref _gender, value.ToGenderFromGermanString());
        }

        // property which represents the salutation to be added/updated or deleted
        private string _salutation;
        public string Salutation
        {
            get => _salutation;
            set => SetField(ref _salutation, value);
        }

        // property which represents the selected salutation index 
        private int _selectedSalutationIndex = -1;
        public int SelectedSalutationIndex
        {
            get => _selectedSalutationIndex;
            set
            {
                SetField(ref _selectedSalutationIndex, value);

                SelectedSalutationIndexChanged?.Invoke(SelectedSalutationIndex);
            }
        }
    }
}