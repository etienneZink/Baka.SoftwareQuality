using System.Collections.Generic;

namespace Baka.ContactSplitter.model
{
    /// <summary>
    /// Represents a contact which is split into its individual parts.
    /// The gender is not stored because it can be retrieved with ISalutationService.
    /// </summary>
    public class Contact
    {
        public string Salutation { get; init; }

        public string LastName { get; init; }

        public string FirstName { get; init; }

        /// <summary>
        /// A contact can have zero or many titles.
        /// </summary>
        public IEnumerable<string> Titles { get; init; }
    }
}