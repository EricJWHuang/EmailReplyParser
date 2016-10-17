using System.Collections.Generic;
using System.Linq;
using EmailReplyParser.Lib.Extensions;
using EmailReplyParser.Lib.TextNormalizers;

namespace EmailReplyParser.Lib
{
    /// <summary>
    /// Email.
    /// </summary>
    public class Email
    {
        private string Text { get; set; }
        private readonly IEnumerable<ITextNormalizer> _normalizers;

        /// <summary>
        /// Create an email instance from a text.
        /// </summary>
        /// <param name="text">email body</param>
        public Email(string text)
            : this(text, new ITextNormalizer[] {new LineEndingsNormalizer(), new ReplyHeaderTextNormalizer(), new ReplyUnderscoresTextNormalizer()})
        {
        }

        /// <summary>
        /// Create an email instance from a text.
        /// </summary>
        /// <param name="text">email body</param>
        /// <param name="normalizers">a set of normalizers for tidying up the <paramref name="text">text</paramref></param>
        public Email(string text, IEnumerable<ITextNormalizer> normalizers)
        {
            Text = text;

            _normalizers = normalizers;
        }

        // An email is an array of fragments
        private readonly List<Fragment> _fragments = new List<Fragment>();

        // It points to the current Fragment.
        // If the matched line fits, it should be added to this Fragment.
        // Otherwise, finish it and start a new Fragment.
        private Fragment _currentFragment;

        // This determines if any 'visible' Fragment has been found.
        // Once any visible Fragment is found, stop looking for hidden ones.
        private bool _foundVisible;

        /// <summary>
        /// The reply after parsing the email.
        /// </summary>
        public string Reply
        {
            get
            {
                var visibleContent = _fragments
                    .Where(fragment => !fragment.IsHidden)
                    .Select(fragment => fragment.Content);

                return string.Join("\n", visibleContent).Trim();
            }
        }

        /// <summary>
        /// Parse email.
        /// </summary>
        public void Parse()
        {
            // Splits the given text into a list of Fragments.
            //  This is roughly done by reversing the text and parsing from the bottom to the top.
            //  This way we can check for 'On <date>, <author> wrote:' lines above quoted blocks.

            foreach (var normalizer in _normalizers)
            {
                Text = normalizer.Normalize(Text);
            }

            ParseEmail();
        }

        private void ParseEmail()
        {
            // The content is reversed initially due to the way we check for hidden fragments.
            Text = Text.Reverse();

            _foundVisible = false;
            _currentFragment = null;

            foreach (var line in Text.Split('\n'))
            {
                ParseEmailLine(line);
            }

            // Finish up the final fragment.
            FinishFragment();

            _currentFragment = null;

            // Now that parsing is done, reverse the order.
            _fragments.Reverse();
        }

        /// <summary>
        /// Parse a line.
        /// </summary>
        private void ParseEmailLine(string line)
        {
            // Scans the given line of text and figures out which fragment it belongs to.

            line = line.TrimEnd('\n');
            if (line.IsSignature())
            {
                line = line.Trim();
            }

            // Mark the current Fragment as a signature
            // if the current line is empty and the Fragment starts with a common signature indicator.
            if (_currentFragment != null
                && string.IsNullOrWhiteSpace(line))
            {
                if (_currentFragment.Lines.Last().IsSignature())
                {
                    _currentFragment.IsSignature = true;

                    FinishFragment();
                }
            }

            var isQuote = line.IsQuote();

            // If the line matches the current fragment, add it.
            // Note that a common reply header also counts as part of the quoted Fragment, even though it doesn't start with `>`.

            var isMatched = false;
            if (_currentFragment != null)
            {
                isMatched = (_currentFragment.IsQuote == isQuote ||
                             (_currentFragment.IsQuote && (line.IsQuoteHeader() || string.IsNullOrWhiteSpace(line))));
            }

            if (!isMatched)
            {
                FinishFragment();
                _currentFragment = new Fragment(isQuote, line);
            }
            else
            {
                _currentFragment.Lines.Add(line);
            }
        }

        /// <summary>
        /// Mark a fragment as finished.
        /// </summary>
        private void FinishFragment()
        {
            // Finishing a fragment will detect any attributes (hidden, signature, reply), and join each line into a string.

            if (_currentFragment != null)
            {
                _currentFragment.Finish();

                if (!_foundVisible)
                {
                    if (_currentFragment.IsQuote ||
                        _currentFragment.IsSignature ||
                        string.IsNullOrWhiteSpace(_currentFragment.Content))
                    {
                        _currentFragment.IsHidden = true;
                    }
                    else
                    {
                        _foundVisible = true;
                    }
                }

                _fragments.Add(_currentFragment);
            }

            _currentFragment = null;
        }
    }
}
