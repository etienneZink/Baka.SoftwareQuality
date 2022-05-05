using System.Collections.Generic;
using Baka.ContactSplitter.Model;

namespace Baka.ContactSplitter.Services.Interfaces
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

        /// <param name="salutation"></param>
        /// <returns>True, iff the salutation could be removed.</returns>
        bool DeleteSalutation(string salutation);

        /// <returns>All salutations stored.</returns>
        IEnumerable<string> GetSalutations();

        /// <param name="salutation">The salutation to get the gender for.</param>
        /// <returns>The gender corresponding to the salutation.</returns>
        Gender GetGender(string salutation);
    }
}