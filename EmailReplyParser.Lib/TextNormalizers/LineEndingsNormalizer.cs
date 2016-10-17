namespace EmailReplyParser.Lib.TextNormalizers
{
    /// <summary>
    /// Line endings normalizer.
    /// </summary>
    public class LineEndingsNormalizer : ITextNormalizer
    {
        public string Normalize(string text)
        {
            return text.Replace("\r\n", "\n");
        }
    }
}