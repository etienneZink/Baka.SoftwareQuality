namespace Baka.ContactSplitter.Model
{
    public static class GenderExtensions
    {
        public static string ToGermanString(this Gender gender)
        {
            return gender switch
            {
                Gender.Male => "Männlich",
                Gender.Female => "Weiblich",
                Gender.Neutral => "Divers",
                _ => "Unbekannt"
            };
        }

        public static Gender ToGenderFromGermanString(this string genderString)
        {
            return genderString switch
            {
                "Männlich" => Gender.Male,
                "Weiblich" => Gender.Female,
                "Divers" => Gender.Neutral,
                _ => Gender.Neutral
            };
        }
    }
}
