using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Baka.ContactSplitter.frontendModel;
using Baka.ContactSplitter.model;

namespace Baka.ContactSplitter.viewModel
{
    public class SalutationWindowViewModel : BaseViewModel
    {

        public ICommand AddOrUpdateCommand { get; set; }
        public ICommand DeleteCommand { get; set; }

        public ObservableCollection<SalutationToGender> Salutations { get; } = new();


        private ObservableCollection<Gender> _genders;

        public ObservableCollection<Gender> Genders => _genders ??= new (Enum.GetValues<Gender>());

        private Gender _gender = Gender.Neutral;

        public Gender Gender
        {
            get => _gender;
            set => SetField(ref _gender, value);
        }

        private string _salutation;

        public string Salutation
        {
            get => _salutation;
            set => SetField(ref _salutation, value);
        }


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