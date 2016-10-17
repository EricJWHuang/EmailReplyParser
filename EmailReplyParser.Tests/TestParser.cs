using System.IO;
using System.Reflection;
using NUnit.Framework;

namespace EmailReplyParser.Tests
{
    [TestFixture]
    public class TestParser
    {
        private string LoadFile(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        [Test][TestCase("correct_sig.txt")]
        [TestCase("email_1_1.txt")]
        [TestCase("email_1_2.txt")]
        [TestCase("email_1_3.txt")]
        [TestCase("email_1_4.txt")]
        [TestCase("email_1_5.txt")]
        [TestCase("email_1_6.txt")]
        [TestCase("email_1_7.txt")]
        [TestCase("email_1_8.txt")]
        [TestCase("email_2_1.txt")]
        [TestCase("email_2_2.txt")]
        [TestCase("email_BlackBerry.txt")]
        [TestCase("email_bullets.txt")]
        [TestCase("email_iPhone.txt")]
        [TestCase("email_multi_word_sent_from_my_mobile_device.txt")]
        [TestCase("email_one_is_not_on.txt")]
        [TestCase("email_sent_from_my_not_signature.txt")]
        [TestCase("email_sig_delimiter_in_middle_of_line.txt")]
        [TestCase("greedy_on.txt")]
        [TestCase("pathological.txt")]
        public void VerifyParsedReply(string fileName)
        {
            var email = LoadFile(string.Format("EmailReplyParser.Tests.TestEmails.{0}", fileName));
            var expectedReply = LoadFile(string.Format("EmailReplyParser.Tests.TestEmailResults.{0}", fileName)).Replace("\r\n", "\n");

            var parser = new Lib.Parser();
            var reply = parser.ParseReply(email);

            Assert.AreEqual(expectedReply, reply);
        }
    }
}