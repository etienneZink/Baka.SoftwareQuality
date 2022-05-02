using Baka.ContactSplitter.model;
using Baka.ContactSplitter.services.interfaces;
using System;
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

        protected virtual string ContactPattern => $@"^(?<{ Salutation }>(<PossibleSalutations>\s+)?)(?<{ Titles }>(<PossibleTitles>\s+)*)((?<{ FirstName }><FirstNamePattern>)\s+(?<{ LastName }><LastNamePattern>)|(?<{ LastName }><LastNamePattern>)\s*,\s+(?<{ FirstName }><FirstNamePattern>))$";

        public virtual ParseResult<Contact> ParseContact(string contactString)
        {
            var parseResult = new ParseResult<Contact>();

            if (contactString is null)
            {
                parseResult.ErrorMessages.Add($"{ nameof(contactString) } ist null!");

                return parseResult;
            }

            contactString = contactString.Trim();

            var possibleSalutations = SalutationService.GetSalutations();
            if (!possibleSalutations.Any()) possibleSalutations = new[] { string.Empty };
            var possibleSalutationsRegex = possibleSalutations.Aggregate((current, salutation) => current + "|" + salutation);
            possibleSalutationsRegex = "(" + string.Concat(possibleSalutationsRegex) + ")";

            var possibleTitles = TitleService.GetTitles();
            if (!possibleTitles.Any()) possibleTitles = new[] { string.Empty };
            var possibleTitlesRegex = possibleTitles.Aggregate((current, title) => current + "|" + title);
            possibleTitlesRegex = "(" + string.Concat(possibleTitlesRegex) + ")";

            var regex = ContactPattern.Replace("<PossibleSalutations>", possibleSalutationsRegex);
            regex = regex.Replace("<PossibleTitles>", possibleTitlesRegex);
            regex = regex.Replace("<FirstNamePattern>", @"[A-Z][a-z]*((\s+|\-)[A-Z][a-z]*)*");
            regex = regex.Replace("<LastNamePattern>", @"([a-z]+\s+)*[A-Z][a-z]*([-][A-Z][a-z]*)?");

            var matchResult = Regex.Match(contactString, regex);

            if (!matchResult.Success)
            {
                parseResult.ErrorMessages.Add("Die Eingabe konnte nicht erfolgreich eingelesen werden!");

                //foreach (var matchGroup in matchResult.Groups.Values.Where(group => !group.Success))
                //{
                //    parseResult.ErrorMessages.Add(matchGroup.Name switch
                //    {
                //        Salutation => "Die Anrede konnte nicht erfolgreich erkannt werden!",
                //        Titles => "Die Titel konnten nicht erfolgreich erkannt werden!",
                //        FirstName => "Der Vorname konnte nicht erfolgreich erkannt werden!",
                //        LastName => "Der Nachname konnte nicht erfolgreich erkannt werden!",
                //        _ => "Eine Eingabe konnte nicht erfolgreich erkannt werden!"
                //    });
                //}

                return parseResult;
            }

            var salutation = matchResult.Groups[Salutation].Value.Trim();
            var titles = matchResult.Groups[Titles].Value.Trim();
            var firstName = Regex.Split(matchResult.Groups[FirstName].Value, @"\s+")
                .Where(s => s != string.Empty)
                .Aggregate((current, firstName) => $"{current} {firstName}");
            var lastName = Regex.Split(matchResult.Groups[LastName].Value, @"\s+")
                .Where(s => s != string.Empty)
                .Aggregate((current, lastName) => $"{current} {lastName}");
            var gender = SalutationService.GetGender(salutation);

            parseResult.Model = new Contact
            {
                Salutation = salutation,
                FirstName = firstName,
                LastName = lastName,
                Gender = gender
            };

            var orderedTitlesList = TitleService
                .GetTitles()
                .OrderByDescending(title => title.Length)
                .ToList();
            
            while (titles!= string.Empty)
            {
                var longestMatch = orderedTitlesList
                    .First(title => titles.StartsWith(title));

                titles = titles
                    .Replace(longestMatch, string.Empty)
                    .Trim();
                parseResult.Model.Titles.Add(longestMatch);
            }
            
            return parseResult;
        }
    }
}
