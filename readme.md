# Email Reply Parser

This is a .NET clone of the very simple but super useful [github email reply parser] (https://github.com/github/email_reply_parser)
- Medium: https://medium.com/@ericjwhuang/email-reply-parser-there-you-go-net-developer-7ce756366f3d

### How to use it
- Install the package from [Nuget](https://www.nuget.org/packages/EmailReplyParser.NET)
- Parse the original reply using
```c#
var parser = new EmailReplyParser.Lib.Parser();
var reply = parser.ParseReply("I get proper rendering as well.\r\n\r\nSent from a magnificent torch of pixels\r\n\r\nOn Dec 16, 2011, at 12:47 PM, Corey Donohoe\r\n<reply@reply.github.com>\r\nwrote:\r\n\r\n> Was this caching related or fixed already?  I get proper rendering here.\r\n>\r\n> ![](https://img.skitch.com/20111216-m9munqjsy112yqap5cjee5wr6c.jpg)\r\n>\r\n> ---\r\n> Reply to this email directly or view it on GitHub:\r\n> https://github.com/github/github/issues/2278#issuecomment-3182418");
```