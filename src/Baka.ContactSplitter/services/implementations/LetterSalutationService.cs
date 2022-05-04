using System.Linq;
using Baka.ContactSplitter.Model;
using Baka.ContactSplitter.Services.Interfaces;

namespace Baka.ContactSplitter.Services.Implementations
{
    public class LetterSalutationService : ILetterSalutationService
    {
        private ITitleService TitleService { get; }

        public LetterSalutationService(ITitleService titleService)
        {
            TitleService = titleService;
        }

        public string GenerateLetterSalutation(Contact contact)
        {
            if (contact is null) return null;

            // if no salutation is present, the default english standard letterSalutation is used
            if (contact.Salutation is null || contact.Salutation == string.Empty) return "Dear Sir or Madam";

            // if the salutation is known as german, the german salutation prefix is used.
            // else the english salutation prefix is used
            var prefix = contact.Salutation switch
            {
                "Frau" => "Sehr geehrte",
                "Herr" => "Sehr geehrter",
                _ => "Dear"
            };

            // maps all titles to their titleSalutations
            var titleSalutations = contact
                .Titles
                .Select(t => TitleService.GetTitleSalutation(t));

            // if language is not german and contact has at least one title you don't mention the contact salutation
            var salutations = titleSalutations.Count() != 0 && prefix == "Dear" ? string.Empty : contact.Salutation;

            // adds all titleSalutations to the salutation-section of the letterSalutation
            if (salutations != string.Empty)
            {
                salutations = titleSalutations.Aggregate(salutations, (current, titleSalutation) => current + $" {titleSalutation}");
            }
            else
            {
                salutations = (titleSalutations.Count() > 0 ? titleSalutations : new[] { string.Empty }).Aggregate((current, titleSalutation) => current + $" {titleSalutation}");
            }

            return $"{prefix} {salutations} {contact.FirstName} {contact.LastName}";
        }
    }
}