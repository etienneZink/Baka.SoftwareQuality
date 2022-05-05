using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

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
        
        /// <summary>
        /// A contact can have zero or many titles.
        /// </summary>
        public IList<string> Titles { get; init; }

        public Contact(): this(new List<string>()) { }

        public Contact([NotNull] IList<string> titles)
        {
            Titles = titles;
        }
    }
}