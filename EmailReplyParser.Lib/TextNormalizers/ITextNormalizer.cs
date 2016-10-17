namespace EmailReplyParser.Lib.TextNormalizers
{
    /// <summary>
    /// An interface to normalize or tidy up text.
    /// </summary>
    public interface ITextNormalizer
    {
        string Normalize(string text);
    }
}
