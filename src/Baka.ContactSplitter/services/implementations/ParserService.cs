using Baka.ContactSplitter.model;
using Baka.ContactSplitter.services.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Baka.ContactSplitter.services.implementations
{
    public class ParserService : IParserService
    {
        public ParserService(ITitleService titleService, ISalutationService salutationService)
        {
            if (titleService is null)
            {
                throw new ArgumentNullException(nameof(titleService));
            }

            if (salutationService is null)
            {
                throw new ArgumentNullException(nameof(salutationService));
            }

            TitleService = titleService;
            SalutationService = salutationService;
        }

        protected ITitleService TitleService { get; }
        protected ISalutationService SalutationService { get; }

        protected const string Salutation = "Salutation";
        protected const string Titles = "Titles";
        protected const string FirstName = "FirstName";
        protected const string LastName = "LastName";

        protected virtual string NamePattern => $"^(?<{ Salutation }>(<PossibleSalutations> )?)(?<{ Titles }>(<PossibleTitles> )*)((?<{ FirstName }><FirstNamePattern>) (?<{ LastName }><LastNamePattern>)|(?<{ LastName }><LastNamePattern>), (?<{ FirstName }><FirstNamePattern>))$";

        public virtual ParseResult<Contact> ParseContact(string contactString)
        {
            var parseResult = new ParseResult<Contact>();

            if (contactString is null)
            {
                parseResult.ErrorMessages.Add($"{ nameof(contactString) } ist null!");

                return parseResult;
            }

            contactString = contactString.Trim();

            var possibleSalutations = SalutationService.GetSalutations().Aggregate((current, salutation) => current + salutation + "|");
            possibleSalutations = "(" + string.Concat(possibleSalutations.SkipLast(1)) + ")";

            var possibleTitles = TitleService.GetTitles().Aggregate((current, title) => current + title + "|");
            possibleTitles = "(" + string.Concat(possibleTitles.SkipLast(1)) + ")";

            var regex = NamePattern.Replace("<PossibleSalutations>", possibleSalutations);
            regex = regex.Replace("<PossibleTitles>", possibleTitles);
            regex = regex.Replace("<FirstNamePattern>", "[A-Z][a-z]*([ -][A-Z][a-z]*)*");
            regex = regex.Replace("<LastNamePattern>", @"([a-z]+ )*[A-Z][a-z]*([-][A-Z][a-z]*)?");

            var matchResult = Regex.Match(contactString, regex);

            if (!matchResult.Success)
            {
                foreach (var matchGroup in matchResult.Groups.Values.Where(group => !group.Success))
                {
                    parseResult.ErrorMessages.Add(matchGroup.Name switch
                    {
                        Salutation => "Die Anrede konnte nicht erfolgreich erkannt werden!",
                        Titles => "Die Titel konnten nicht erfolgreich erkannt werden!",
                        FirstName => "Der Vorname konnte nicht erfolgreich erkannt werden!",
                        LastName => "Der Nachname konnte nicht erfolgreich erkannt werden!",
                        _ => "Eine Eingabe konnte nicht erfolgreich erkannt werden!"
                    });
                }

                return parseResult;
            }

            var salutation = matchResult.Groups[Salutation].Value;
            var titles = matchResult.Groups[Titles].Value;
            var firstName = matchResult.Groups[FirstName].Value;
            var lastName = matchResult.Groups[LastName].Value;

            parseResult.Model = new Contact
            {
                Salutation = salutation,
                FirstName = firstName,
                LastName = lastName
            };

            var orderedTitlesList = TitleService.GetTitles().OrderByDescending(title => title.Length);

            foreach (var title in orderedTitlesList)
            {
                if (titles.Contains(title))
                {
                    titles = titles.Replace(title, string.Empty);

                    parseResult.Model.Titles.Add(title);
                }
            }

            return parseResult;
        }
    }
}
