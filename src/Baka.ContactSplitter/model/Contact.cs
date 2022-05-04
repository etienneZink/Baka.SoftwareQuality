using System.Collections.Generic;

namespace Baka.ContactSplitter.Model
{
    /// <summary>
    /// Represents a contact which is split into its individual parts.
    /// </summary>
    public class Contact
    {
        public string Salutation { get; init; }

        public string LastName { get; set; }

        public string FirstName { get; set; }
        
        private IList<string> _Titles;
        /// <summary>
        /// A contact can have zero or many titles.
        /// </summary>
        public IList<string> Titles => _Titles ??= new List<string>();
    }
}