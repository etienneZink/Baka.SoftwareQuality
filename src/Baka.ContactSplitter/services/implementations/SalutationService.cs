using System.Collections.Generic;
using System.IO;
using Baka.ContactSplitter.model;
using Baka.ContactSplitter.services.interfaces;
using Newtonsoft.Json;

namespace Baka.ContactSplitter.services.implementations
{
    public class SalutationService : ISalutationService
    {
        private string SalutationJsonPath => "resources/Salutations.json";

        private Dictionary<string, Gender> SalutationsToGender { get; set; }

        public SalutationService() => SalutationsToGender = LoadSalutationJson();

        public bool SaveOrUpdateSalutation(string salutation, Gender gender)
        {
            if (SalutationsToGender.ContainsKey(salutation)) SalutationsToGender[salutation] = gender;
            else SalutationsToGender.Add(salutation, gender);

            if (!WriteSalutationJson()) return false;
            SalutationsToGender = LoadSalutationJson();
            return true;
        }

        public bool DeleteSalutation(string salutation)
        {
            if (!SalutationsToGender.ContainsKey(salutation)) return true;
            SalutationsToGender.Remove(salutation);

            if (!WriteSalutationJson()) return false;
            SalutationsToGender = LoadSalutationJson();
            return true;
        }

        public IEnumerable<string> GetSalutations() => SalutationsToGender.Keys;

        public Gender GetGender(string salutation) => salutation is not null && SalutationsToGender.ContainsKey(salutation) ?  SalutationsToGender[salutation]: Gender.Neutral;

        /// <summary>
        /// Tries to read the salutations from the JSON in SalutationJsonPath.
        /// </summary>
        /// <returns>The dictionary parsed from SalutationJsonPath or a empty dictionary.</returns>
        private Dictionary<string, Gender> LoadSalutationJson()
        {
            try
            {
                using var streamReader = new StreamReader(SalutationJsonPath);
                var json = streamReader.ReadToEnd();
                return JsonConvert.DeserializeObject<Dictionary<string, Gender>>(json);
            }
            catch
            {
                return new Dictionary<string, Gender>();
            }
        }

        /// <returns>True, iff SalutationsToGender could be written into SalutationJsonPath as JSON.</returns>
        private bool WriteSalutationJson()
        {
            try
            {
                using var streamWriter = new StreamWriter(SalutationJsonPath, false);
                streamWriter.Write(JsonConvert.SerializeObject(SalutationsToGender));
                streamWriter.Flush();
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}