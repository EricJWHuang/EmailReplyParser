using System.Text.RegularExpressions;

namespace EmailReplyParser.Lib.Extensions
{
    /// <summary>
    /// A set of extension methods on string line.
    /// </summary>
    public static class LineExtensions
    {
        /// <summary>
        /// Indicate if the <paramref name="line">line</paramref> is signature.
        /// </summary>
        public static bool IsSignature(this string line)
        {
            // "Send from my ..." or "From: " is considered as signature

            const string signatureRegex = @"(?m)(--\s*$|__\s*$|\w-$)|(^(\w+\s*){1,3} ym morf tneS$)";
            return ((new Regex(signatureRegex)).Matches(line).Count > 0);
        }

        /// <summary>
        /// Indicate if the <paramref name="line">line</paramref> is quote.
        /// </summary>
        public static bool IsQuote(this string line)
        {
            // "> " is considered as quote

            const string quoteRegex = @"(>+)$";
            return ((new Regex(quoteRegex)).Matches(line).Count > 0);
        }

        /// <summary>
        /// Indicate if the <paramref name="line">line</paramref> is quote header.
        /// </summary>
        public static bool IsQuoteHeader(this string line)
        {
            const string quoteHeaderRegex = @"^:etorw.*nO\s*(>{1})?$";
            return ((new Regex(quoteHeaderRegex)).Matches(line).Count > 0);
        }
    }
}
