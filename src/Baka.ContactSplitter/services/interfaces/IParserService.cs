using Baka.ContactSplitter.Model;

namespace Baka.ContactSplitter.Services.Interfaces
{
    public interface IParserService
    {
        /// <param name="contactString">The string which should be converted into a contact.</param>
        /// <returns>A parse result for the given contactString.</returns>
        ParseResult<Contact> ParseContact(string contactString);
    }
}