using Baka.ContactSplitter.model;

namespace Baka.ContactSplitter.services.interfaces
{
    public interface IParserService
    {
        /// <param name="contactString">The string which should be converted into a contact.</param>
        /// <returns>A parse result for the given contactString.</returns>
        ParseResult<Contact> ParseContact(string contactString);
    }
}