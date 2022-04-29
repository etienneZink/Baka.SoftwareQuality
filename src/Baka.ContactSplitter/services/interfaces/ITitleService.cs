using System.Collections.Generic;

namespace Baka.ContactSplitter.services.interfaces
{
    public interface ITitleService
    {
        /// <summary>
        /// Saves or updates the given title with the given titleSalutation.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="titleSalutation"></param>
        /// <returns>True, iff the title with the titleSalutation could be saved or updated.</returns>
        bool SaveOrUpdateTitle(string title, string titleSalutation);

        
        /// <returns>All titles stored.</returns>
        IEnumerable<string> GetTitles();

        /// <param name="title">The title to get the titleSalutation for.</param>
        /// <returns>The titleSalutation corresponding to the title.</returns>
        string GetTitleSalutation(string title);
    }
}