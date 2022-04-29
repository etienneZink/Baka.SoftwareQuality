using System;
using System.Collections.Generic;
using System.IO;
using Baka.ContactSplitter.services.interfaces;
using Newtonsoft.Json;

namespace Baka.ContactSplitter.services.implementations
{
    /// <summary>
    /// Implementation of the service interface ITitleService.
    /// </summary>
    public class TitleService: ITitleService
    {
        private string TitleJsonPath => "resources/Titles.json";

        private Dictionary<string, string> TitleToTitleSalutation { get; set; }

        public TitleService() => TitleToTitleSalutation = LoadTitleJson();

        public bool SaveOrUpdateTitle(string title, string titleSalutation)
        {
            
            if (TitleToTitleSalutation.ContainsKey(title)) TitleToTitleSalutation[title] = titleSalutation;
            else TitleToTitleSalutation.Add(title, titleSalutation);

            if (!WriteTitleJson()) return false;
            TitleToTitleSalutation = LoadTitleJson();
            return true;
        }

        public bool DeleteTitle(string title)
        {
            if (!TitleToTitleSalutation.ContainsKey(title)) return true;
            TitleToTitleSalutation.Remove(title);

            if (!WriteTitleJson()) return false;
            TitleToTitleSalutation = LoadTitleJson();
            return true;
        }

        public IEnumerable<string> GetTitles() => TitleToTitleSalutation.Keys;

        public string GetTitleSalutation(string title) => TitleToTitleSalutation.ContainsKey(title) ? TitleToTitleSalutation[title] : string.Empty;

        /// <summary>
        /// Tries to read the titles from the JSON in TitleJsonPath.
        /// </summary>
        /// <returns>The dictionary parsed from TitleJsonPath or a empty dictionary.</returns>
        private Dictionary<string, string> LoadTitleJson()
        {
            try
            {
                using var streamReader = new StreamReader(TitleJsonPath);
                var json = streamReader.ReadToEnd();
                return JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            }
            catch 
            {
                return new Dictionary<string, string>();
            }
        }

        /// <returns>True, iff TitleToTitleSalutation could be written into TitleJsonPath as JSON.</returns>
        private bool WriteTitleJson()
        {
            try
            {
                using var streamWriter = new StreamWriter(TitleJsonPath, false);
                streamWriter.Write(JsonConvert.SerializeObject(TitleToTitleSalutation));
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