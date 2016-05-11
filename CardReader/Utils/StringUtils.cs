using System;
using System.Text.RegularExpressions;

namespace CardReader.Utils
{
    public static class StringUtils
    {
        public static string ParseMagneticStripeName(string magStripeName, out string firstName, out string lastName)
        {
            firstName = null;
            lastName = null;

            if (string.IsNullOrEmpty(magStripeName))
                return null;

            string workingString = magStripeName;

            // Flip the data around the "/", if one is present. Makes for a more sensible string to parse
            int indexOfSlash = workingString.IndexOf("/", StringComparison.Ordinal);
            if (indexOfSlash != -1)
            {
                string newFirstSegment = indexOfSlash + 1 < workingString.Length
                    ? workingString.Substring(indexOfSlash + 1)
                    : string.Empty;
                string newLastSegment = indexOfSlash != 0 ? workingString.Substring(0, indexOfSlash) : string.Empty;

                // Special case for newLastSegment: "O Donnell" etc...
                newLastSegment = Regex.Replace(newLastSegment, "(^|\\s+)o\\s+(.+)", "$1O'$2", RegexOptions.IgnoreCase);

                workingString = string.Format("{0} {1}", newFirstSegment, newLastSegment);
            }

            // Replace periods, commas and double quotes with spaces
            workingString = Regex.Replace(workingString, "[\\.,\"]", " ");

            // Drop known prefixes and suffixes, they could be on the ends or in the middle
            workingString = Regex.Replace(workingString, "(^|\\s)(mr|mrs|miss|ms|jr|sr|esq|ii|iii|iv)($|\\s)", " ", RegexOptions.IgnoreCase);

            // Change multiple spaces into single spaces
            workingString = Regex.Replace(workingString, "\\s\\s+", " ");

            // Trim
            workingString = workingString.Trim();

            // Adjust case
            workingString = workingString.ToLower();
            workingString = workingString.ToTitleCase();

            // Special case: adjusting the first letter after an apostrophe "O'Donnel"
            int indexofQuote = workingString.IndexOf('\'');
            if (indexofQuote > -1)
                workingString = CapitalizeCharacterAt(workingString, indexofQuote + 1);

            // Special case: adjusting the first letter after a "Mc"
            int indexofMc = workingString.IndexOf("mc", StringComparison.OrdinalIgnoreCase);
            if (indexofMc > -1)
                workingString = CapitalizeCharacterAt(workingString, indexofMc + 2);

            // Extract segments
            string[] segments = workingString.Split(' ');

            // Build firstName and lastName. All segments except the last one belong to the firstName, the last segment belongs to lastName.
            // Drop middle initials
            for (int i = 0; i < segments.Length; i++)
            {
                if (i == 0)
                {
                    firstName = segments[i];
                }
                if (i > 0 && i != segments.Length - 1)
                {
                    // Only add middle segements which are greater than 1 character, for example "Carly Rae Smith", "Rae" should be added to the first name.
                    // "Carly J Smith", the middle initial, "J" should be dropped
                    if (segments[i].Length > 1)
                        firstName += " " + segments[i];
                }
                // Only add something to the lastName if there is more than 1 segment
                if (segments.Length > 1 && i == segments.Length - 1)
                {
                    lastName += segments[i];
                }
            }

            return workingString;
        }

        public static string CapitalizeCharacterAt(string input, int indexToCapitalize)
        {
            if (input.Length <= indexToCapitalize)
                return input;
            else
            {
                char[] charArray = input.ToCharArray();

                charArray[indexToCapitalize] = char.ToUpper(charArray[indexToCapitalize]);

                return new string(charArray);
            }
        }

        private static string ToTitleCase(this string str)
        {
            string auxStr = str.ToLower();
            string[] auxArr = auxStr.Split(' ');
            string result = "";
            bool firstWord = true;
            foreach (string word in auxArr)
            {
                if (!firstWord)
                    result += " ";
                else
                    firstWord = false;

                result += word.Substring(0, 1).ToUpper() + word.Substring(1, word.Length - 1);
            }

            return result;
        }
    }
}
