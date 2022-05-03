using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Baka.ContactSplitter.frontendModel;
using Baka.ContactSplitter.model;

namespace Baka.ContactSplitter.viewModel
{
    /// <summary>
    /// Class which is used as a viewModel for the SalutationWindow.
    /// </summary>
    public class SalutationWindowViewModel : BaseViewModel
    {

        public ICommand AddOrUpdateCommand { get; set; }
        public ICommand DeleteCommand { get; set; }

        public ObservableCollection<SalutationToGender> Salutations { get; } = new();

        // property which represents all possible genders
        private ObservableCollection<Gender> _genders;
        public ObservableCollection<Gender> Genders => _genders ??= new (Enum.GetValues<Gender>());

        // property which represents the gender for the salutation to be added/updated or deleted
        private Gender _gender = Gender.Neutral;
        public Gender Gender
        {
            get => _gender;
            set => SetField(ref _gender, value);
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
                if (SelectedSalutationIndex >= 0 && SelectedSalutationIndex < Salutations.Count)
                {
                    Salutation = Salutations[_selectedSalutationIndex].Salutation;
                    Gender = Salutations[_selectedSalutationIndex].Gender;
                }
                else
                {
                    Salutation = string.Empty;
                    Gender = Gender.Neutral;
                }
            }
        }
    }
}