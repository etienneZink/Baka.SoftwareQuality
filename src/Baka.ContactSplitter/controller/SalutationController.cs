using System.Linq;
using Baka.ContactSplitter.Framework;
using Baka.ContactSplitter.FrontendModel;
using Baka.ContactSplitter.Model;
using Baka.ContactSplitter.Services.Interfaces;
using Baka.ContactSplitter.View;
using Baka.ContactSplitter.ViewModel;

namespace Baka.ContactSplitter.Controller
{
    /// <summary>
    /// Class which is used as the controller for the SalutationWindowViewModel and the SalutationWindow.
    /// This controller combines these two and defines the logic for methods of the SalutationWindowViewModel.
    /// </summary>
    public class SalutationController: BaseWindowController<SalutationWindow, SalutationWindowViewModel>
    {
        private ISalutationService SalutationService { get; }

        public SalutationController(SalutationWindow view, SalutationWindowViewModel viewModel, ISalutationService salutationService): base(view, viewModel)
        {
            ViewModel.AddOrUpdateCommand = new RelayCommand(ExecuteAddOrUpdateCommand, CanExecuteAddOrUpdateCommand);
            ViewModel.DeleteCommand = new RelayCommand(ExecuteDeleteCommand, CanExecuteDeleteCommand);
            SalutationService = salutationService;
            LoadSalutationsToGenders();

            ViewModel.SelectedSalutationIndexChanged += OnSelectedSalutationIndexChanged;
        }

        public void ExecuteAddOrUpdateCommand(object o)
        {
            SalutationService.SaveOrUpdateSalutation(ViewModel.Salutation, ViewModel.Gender.ToGenderFromGermanString());
            LoadSalutationsToGenders();
            ViewModel.Salutation = string.Empty;
            ViewModel.Gender = Gender.Neutral.ToGermanString();
        }

        public bool CanExecuteAddOrUpdateCommand(object o)
        {
            //add can be executed, if the salutation text is not null
            return ViewModel.Salutation is not null;
        }

        public void ExecuteDeleteCommand(object o)
        {
            var stg = ViewModel.Salutations[ViewModel.SelectedSalutationIndex];
            ViewModel.Salutations.RemoveAt(ViewModel.SelectedSalutationIndex);
            SalutationService.DeleteSalutation(stg.Salutation);
            ViewModel.Salutation = string.Empty;
            ViewModel.Gender = Gender.Neutral.ToGermanString();
        }

        public bool CanExecuteDeleteCommand(object o)
        {
            //delete can be executed, if a salutation is selected (index != -1)
            return ViewModel.SelectedSalutationIndex != -1;
        }

        //fetches all salutations and genders from the SalutationService and adds them to the list of SalutationToGender
        private void LoadSalutationsToGenders()
        {
            ViewModel.Salutations.Clear();
            var titleToTitleSalutations = SalutationService
                .GetSalutations()
                .Select(s => new SalutationToGender
                {
                    Salutation = s,
                    GenderString = SalutationService.GetGender(s).ToGermanString()
                });

            foreach (var stg in titleToTitleSalutations)
            {
                ViewModel.Salutations.Add(stg);
            }
        }
        private void OnSelectedSalutationIndexChanged(int selectedSalutationIndex)
        {
            if (selectedSalutationIndex >= 0 && selectedSalutationIndex < ViewModel.Salutations.Count)
            {
                ViewModel.Salutation = ViewModel.Salutations[selectedSalutationIndex].Salutation;
                ViewModel.Gender = ViewModel.Salutations[selectedSalutationIndex].GenderString;
            }
            else
            {
                ViewModel.Salutation = string.Empty;
                ViewModel.Gender = Gender.Neutral.ToGermanString();
            }
        }
    }
}