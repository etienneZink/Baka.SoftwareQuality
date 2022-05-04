using Baka.ContactSplitter.Model;

namespace Baka.ContactSplitter.Services.Interfaces
{
    public interface ILetterSalutationService
    {
        /// <param name="contact">The contact a letter salutation should be generated for.</param>
        /// <returns>A string containing the correct letter salutation for the given contact.</returns>
        string GenerateLetterSalutation(Contact contact);
    }
}