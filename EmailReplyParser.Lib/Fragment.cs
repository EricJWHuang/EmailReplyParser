using System.Collections.Generic;
using EmailReplyParser.Lib.Extensions;

namespace EmailReplyParser.Lib
{
    /// <summary>
    /// Represents a group of lines in the email sharing common attributes.
    /// </summary>
    public class Fragment
    {
        // An fragment is an array of lines
        public List<string> Lines { get; set; }

        // A set of common attributes for the fragment.
        public bool IsQuote { get; set; }
        public bool IsSignature { get; set; }
        public bool IsHidden { get; set; }

        public Fragment(bool isQuote, string firstLine)
        {
            IsSignature = false;
            IsHidden = false;
            IsQuote = isQuote;

            Lines = new List<string>();

            if (firstLine != null)
            {
                Lines.Add(firstLine);
            }
        }

        public void Finish()
        {
            Content = string.Join("\n", Lines);
            Content = Content.Reverse();

            Lines.Clear();
        }

        public string Content { get; private set; }
    }
}