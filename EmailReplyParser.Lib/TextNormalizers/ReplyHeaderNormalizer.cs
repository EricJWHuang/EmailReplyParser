using System.Text.RegularExpressions;

namespace EmailReplyParser.Lib.TextNormalizers
{
    /// <summary>
    /// Reply header normalizer.
    /// </summary>
    public class ReplyHeaderTextNormalizer : ITextNormalizer
    {
        public string Normalize(string text)
        {
            // Check for multi-line reply headers. Some clients break up
            // the "On DATE, NAME <EMAIL> wrote:" line into multiple lines.

            var replyHeaderRegex = new Regex(@"(?!On.*On\s.+?wrote:)(On\s(.+?)wrote:)", RegexOptions.Singleline);
            return replyHeaderRegex.Replace(text, (match) => match.Value.Replace("\n", " "));
        }
    }
}
