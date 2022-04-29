using System.Collections.Generic;
using Baka.ContactSplitter.model;

namespace Baka.ContactSplitter.services.interfaces
{
    public interface ISalutationService
    {
        /// <summary>
        /// Saves or updates the given salutation with the given gender.
        /// </summary>
        /// <param name="salutation"></param>
        /// <param name="gender"></param>
        /// <returns>True, iff the salutation with the gender could be saved or updated.</returns>
        bool SaveOrUpdateSalutation(string salutation, Gender gender);

        /// <returns>All salutations stored.</returns>
        IEnumerable<string> GetSalutations();

        /// <param name="salutation">The salutation to get the gender for.</param>
        /// <returns>The gender corresponding to the salutation.</returns>
        Gender GetGender(string salutation);
    }
}