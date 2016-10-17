using System.Text.RegularExpressions;

namespace EmailReplyParser.Lib.TextNormalizers
{
    /// <summary>
    /// Reply underscores normaliser.
    /// </summary>
    public class ReplyUnderscoresTextNormalizer : ITextNormalizer
    {
        public string Normalize(string text)
        {
            // Some users may reply directly above a line of underscores.
            // In order to ensure that these fragments are split correctly,
            // make sure that all lines of underscores are preceded by
            // at least two newline characters.

            var underscoreRegex = new Regex(@"([^\n])(?=\n_{7}_+)$", RegexOptions.Multiline);
            return underscoreRegex.Replace(text, (match) => match.Value + "\n", 1);
        }
    }
}
