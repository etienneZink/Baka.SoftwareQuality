using System.Linq;
using Baka.ContactSplitter.framework;
using Baka.ContactSplitter.frontendModel;
using Baka.ContactSplitter.model;
using Baka.ContactSplitter.services.interfaces;
using Baka.ContactSplitter.view;
using Baka.ContactSplitter.viewModel;

namespace Baka.ContactSplitter.controller
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
        }

        public void ExecuteAddOrUpdateCommand(object o)
        {
            SalutationService.SaveOrUpdateSalutation(ViewModel.Salutation, ViewModel.Gender);
            LoadSalutationsToGenders();
            ViewModel.Salutation = string.Empty;
            ViewModel.Gender = Gender.Neutral;
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
            ViewModel.Gender = Gender.Neutral;
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
                    Gender = SalutationService.GetGender(s)
                });

            foreach (var stg in titleToTitleSalutations)
            {
                ViewModel.Salutations.Add(stg);
            }
        }
    }
}