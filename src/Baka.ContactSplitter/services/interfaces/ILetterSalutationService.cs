using Baka.ContactSplitter.model;

namespace Baka.ContactSplitter.services.interfaces
{
    public interface ILetterSalutationService
    {
        /// <param name="contact">The contact a letter salutation should be generated for.</param>
        /// <returns>A string containing the correct letter salutation for the given contact.</returns>
        string GenerateLetterSalutation(Contact contact);
    }
}