﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JSNLog.LogHandling;
using JSNLog.Tests.Logic;
using System.Xml;
using JSNLog.Infrastructure;
using System.Text;
using JSNLog.Exceptions;
using System.IO;

namespace JSNLog.Tests.UnitTests
{
    [TestClass]
    public class DemoTests
    {
        // All tests here write a partial with the complete demo html.
        // The partials are written to directory:
        private const string _demosDirectory = @"D:\Dev\JSNLog\jsnlog.website\WebSite\Views\Shared\Demos";

        // >>>>>>>>>>>>>>>>>>>>>>>
        // All files are removed from the Demos directory before the tests are run.
        // If you run 1 test, you'll get only 1 file.
        // Be sure to run ALL tests before publishing the web site.
        // <<<<<<<<<<<<<<<<<<<<<<<<

        [ClassInitialize]
        public static void ClassInitialize(TestContext a)
        {
            // Delete all files in Demos directory

            System.IO.DirectoryInfo demosDirectory = new DirectoryInfo(_demosDirectory);

            foreach (FileInfo file in demosDirectory.GetFiles("*.cshtml"))
            {
                file.Delete();
            }
        }

        // ---------------------------------------------------------------------------------

        [TestMethod]
        public void jsnlog1()
        {
            TestDemo(
                @"
<jsnlog serverSideMessageFormat=""Sent: % date, Brower: % userAgent - % message"" >
</jsnlog>",
                @"
new JsnlogConfiguration {
    serverSideMessageFormat=""Sent: % date, Brower: % userAgent - % message""
}",
                "jsnlog1");
        }

        [TestMethod]
        public void jsnlog2()
        {
            TestDemo(
                @"
<jsnlog serverSideLogger=""jslogger"">
</jsnlog>",
                @"
new JsnlogConfiguration {
    serverSideLogger=""jslogger""
}",
                "jsnlog2");
        }

        [TestMethod]
        public void jsnlog3()
        {
            TestDemo(
                @"
<jsnlog enabled=""false"">
</jsnlog>",
                @"
new JsnlogConfiguration {
    enabled=false
}",
                "jsnlog3");
        }

        // ---------------------------------------------------------------------------------

        [TestMethod]
        public void ajaxappender1()
        {
            TestDemo(
                @"
<jsnlog>
	<ajaxAppender 
		name=""appender1"" 
		storeInBufferLevel=""TRACE"" 
		level=""WARN"" 
		sendWithBufferLevel=""FATAL"" 
		bufferSize=""20""/>
	<logger appenders=""appender1""/>
</jsnlog>
",
                @"
new JsnlogConfiguration {
    ajaxAppenders=new List<AjaxAppender> {
        new AjaxAppender {
		    name=""appender1"", 
		    storeInBufferLevel=""TRACE"", 
		    level=""WARN"", 
		    sendWithBufferLevel=""FATAL"", 
		    bufferSize=20
        }
    },
    loggers=new List<Logger> {
        new Logger {
            appenders=""appender1""
        }
    }
}",
                "ajaxappender1");
        }

        // ---------------------------------------------------------------------------------

        [TestMethod]
        public void consoleappender1()
        {
            TestDemo(
                @"
<jsnlog>
	<!-- ""mylogger"" logs to just the console -->
	<consoleAppender name=""consoleAppender"" />
	<logger name=""mylogger"" appenders=""consoleAppender"" />
</jsnlog>
",
                @"
// ""mylogger"" logs to just the console
new JsnlogConfiguration {
    consoleAppenders=new List<ConsoleAppender> {
        new ConsoleAppender {
		    name=""consoleAppender""
        }
    },
    loggers=new List<Logger> {
        new Logger {
            name=""mylogger"",
            appenders=""consoleAppender""
        }
    }
}",
                "consoleappender1");
        }

        [TestMethod]
        public void consoleappender2()
        {
            TestDemo(
                @"
<jsnlog>
	<!-- ""mylogger"" logs to both the server and the console -->
    <consoleAppender name=""consoleAppender"" />
    <ajaxAppender name=""ajaxAppender"" />
	<logger name=""mylogger"" appenders=""ajaxAppender;consoleAppender"" />
</jsnlog>
",
                @"
// ""mylogger"" logs to both the server and the console
new JsnlogConfiguration {
    consoleAppenders=new List<ConsoleAppender> {
        new ConsoleAppender {
		    name=""consoleAppender""
        }
    },
    ajaxAppenders=new List<AjaxAppender> {
        new AjaxAppender {
		    name=""ajaxAppender""
        }
    },
    loggers=new List<Logger> {
        new Logger {
            name=""mylogger"",
            appenders=""ajaxAppender;consoleAppender""
        }
    }
}",
                "consoleappender2");
        }

        [TestMethod]
        public void consoleappender3()
        {
            TestDemo(
                @"
<jsnlog>
	<!-- Debugging: all loggers log to both the server and the console -->
    <consoleAppender name=""consoleAppender"" />
    <ajaxAppender name=""ajaxAppender"" />
	<logger appenders=""ajaxAppender;consoleAppender"" />
</jsnlog>
",
                @"
// Debugging: all loggers log to both the server and the console
new JsnlogConfiguration {
    consoleAppenders=new List<ConsoleAppender> {
        new ConsoleAppender {
		    name=""consoleAppender""
        }
    },
    ajaxAppenders=new List<AjaxAppender> {
        new AjaxAppender {
		    name=""ajaxAppender""
        }
    },
    loggers=new List<Logger> {
        new Logger {
            appenders=""ajaxAppender;consoleAppender""
        }
    }
}",
                "consoleappender3");
        }

        [TestMethod]
        public void consoleappender4()
        {
            TestDemo(
                @"
<jsnlog>
	<!-- Production: loggers log to the server only -->
    <ajaxAppender name=""ajaxAppender"" />
	<logger appenders=""ajaxAppender;consoleAppender"" />
</jsnlog>
",
                @"
// Production: loggers log to the server only
new JsnlogConfiguration {
    ajaxAppenders=new List<AjaxAppender> {
        new AjaxAppender {
		    name=""ajaxAppender""
        }
    },
    loggers=new List<Logger> {
        new Logger {
            appenders=""ajaxAppender;consoleAppender""
        }
    }
}",
                "consoleappender4");
        }

        // ---------------------------------------------------------------------------------

        [TestMethod]
        public void logger1()
        {
            TestDemo(
                @"
<jsnlog>
	<logger name=""a.b"" level=""3000"" />
</jsnlog>
",
                @"
new JsnlogConfiguration {
    loggers=new List<Logger> {
        new Logger {
            name=""a.b"",
			level=""3000""
        }
    }
}",
                "logger1");
        }

        [TestMethod]
        public void logger2()
        {
            TestDemo(
                @"
<jsnlog>
	<logger level=""3000"" />
</jsnlog>
",
                @"
new JsnlogConfiguration {
    loggers=new List<Logger> {
        new Logger {
			level=""3000""
        }
    }
}",
                "logger2");
        }

        [TestMethod]
        public void logger3()
        {
            TestDemo(
                @"
<jsnlog>
	<logger name=""a.b"" level=""INFO"" />
</jsnlog>
",
                @"
new JsnlogConfiguration {
    loggers=new List<Logger> {
        new Logger {
            name=""a.b"",
			level=""INFO""
        }
    }
}",
                "logger3");
        }

        [TestMethod]
        public void logger4()
        {
            TestDemo(
                @"
<jsnlog>
	<logger name=""a.b"" userAgentRegex=""MSIE 7|MSIE 8"" level=""4000"" />
</jsnlog>
",
                @"
new JsnlogConfiguration {
    loggers=new List<Logger> {
        new Logger {
            name=""a.b"",
            userAgentRegex=""MSIE 7|MSIE 8"",
			level=""4000""
        }
    }
}",
                "logger4");
        }

        [TestMethod]
        public void logger5()
        {
            TestDemo(
                @"
<jsnlog>
    <ajaxAppender name=""appender"" />
	<logger name=""a.b"" appenders=""appender"" />
</jsnlog>
",
                @"
// Production: loggers log to the server only
new JsnlogConfiguration {
    ajaxAppenders=new List<AjaxAppender> {
        new AjaxAppender {
		    name=""appender""
        }
    },
    loggers=new List<Logger> {
        new Logger {
            name=""a.b"",
            appenders=""appender""
        }
    }
}",
                "logger5");
        }

        [TestMethod]
        public void logger6()
        {
            TestDemo(
                @"
<jsnlog>
    <ajaxAppender name=""appender1"" />
    <ajaxAppender name=""appender2"" />
	<logger name=""a.b"" appenders=""apender1;appender2"" />
</jsnlog>
",
                @"
// Production: loggers log to the server only
new JsnlogConfiguration {
    ajaxAppenders=new List<AjaxAppender> {
        new AjaxAppender {
		    name=""appender1""
        },
        new AjaxAppender {
		    name=""appender2""
        }
    },
    loggers=new List<Logger> {
        new Logger {
            name=""a.b"",
            appenders=""apender1;appender2""
        }
    }
}",
                "logger6");
        }

        [TestMethod]
        public void logger7()
        {
            TestDemo(
                @"
<jsnlog>
	<logger name=""a"">
		<onceOnly regex=""Parameter x too high - x ="" />
	</logger>
</jsnlog>
",
                @"
new JsnlogConfiguration {
    loggers=new List<Logger> {
        new Logger {
            name=""a"",
			onceOnlies = new List<OnceOnlyOptions> {
				new OnceOnlyOptions {
					regex=""Parameter x too high - x =""
				}
            }
        }
    }
}",
                "logger7");
        }

        [TestMethod]
        public void logger8()
        {
            TestDemo(
                @"
<jsnlog>
	<logger name=""a"">
		<onceOnly regex=""Parameter x too high - x ="" />
        <onceOnly regex=""x = \d+ and y = \d+"" />
	</logger>
</jsnlog>
",
                @"
new JsnlogConfiguration {
    loggers=new List<Logger> {
        new Logger {
            name=""a"",
			onceOnlies = new List<OnceOnlyOptions> {
				new OnceOnlyOptions {
					regex=""Parameter x too high - x =""
				},
				new OnceOnlyOptions {
					regex=""x = \\d+ and y = \\d+""
				}
            }
        }
    }
}",
                "logger8");
        }

        [TestMethod]
        public void logger9()
        {
            TestDemo(
                @"
<jsnlog>
	<logger name=""a.b"">
		<onceOnly />
	</logger>
</jsnlog>
",
                @"
new JsnlogConfiguration {
    loggers=new List<Logger> {
        new Logger {
            name=""a.b"",
			onceOnlies = new List<OnceOnlyOptions> {
				new OnceOnlyOptions {
				}
            }
        }
    }
}",
                "logger9");
        }

        // ---------------------------------------------------------------------------------

        [TestMethod]
        public void consolelog1()
        {
            TestDemo(
                @"
<!-- Debug version of web.config -->
<jsnlog>
	<!-- Create console appender -->
	<consoleAppender name=""consoleAppender"" />

    <!-- Send all log messages to console appender -->
	<logger appenders=""consoleAppender"" />
</jsnlog>
",
                @"
// Debug version of web.config
new JsnlogConfiguration {

    // Create console appender
    consoleAppenders=new List<ConsoleAppender> {
        new ConsoleAppender {
		    name=""consoleAppender""
        }
    },

    // Send all log messages to console appender
    loggers=new List<Logger> {
        new Logger {
            appenders=""consoleAppender""
        }
    }
}",
                "consolelog1");
        }

        [TestMethod]
        public void consolelog2()
        {
            TestDemo(
                @"
<!-- Release version of web.config -->
<jsnlog>
    <!-- Stop all messages with severity lower than fatal -->
	<logger level=""FATAL"" />
</jsnlog>
",
                @"
// Release version of web.config
new JsnlogConfiguration {

    // Stop all messages with severity lower than fatal
    loggers=new List<Logger> {
        new Logger {
            level=""FATAL""
        }
    }
}",
                "consolelog2");
        }

        [TestMethod]
        public void consolelog3()
        {
            TestDemo(
                @"
<!-- Debug version of web.config -->
<jsnlog>
	<!-- Create console appender and AJAX appender -->
    <consoleAppender name=""consoleAppender"" />
    <ajaxAppender name=""ajaxAppender"" />

    <!-- Send all log messages to both -->
	<logger appenders=""ajaxAppender;consoleAppender"" />
</jsnlog>
",
                @"
// Debug version of web.config
new JsnlogConfiguration {

    // Create console appender and AJAX appender
    consoleAppenders=new List<ConsoleAppender> {
        new ConsoleAppender {
		    name=""consoleAppender""
        }
    },
    ajaxAppenders=new List<AjaxAppender> {
        new AjaxAppender {
		    name=""ajaxAppender""
        }
    },

    // Send all log messages to both
    loggers=new List<Logger> {
        new Logger {
            appenders=""ajaxAppender;consoleAppender""
        }
    }
}",
                "consolelog3");
        }

        // ---------------------------------------------------------------------------------

        [TestMethod]
        public void cors1()
        {
            TestDemo(
                @"
<jsnlog corsAllowedOriginsRegex=""^https?:\/\/([a-z0-9]+[.])*(my-abc-domain[.]com|my-xyz-domain[.]com)$"" >
</jsnlog>",
                @"
new JsnlogConfiguration {
    corsAllowedOriginsRegex=""^https?:\\/\\/([a-z0-9]+[.])*(my-abc-domain[.]com|my-xyz-domain[.]com)$""
}",
                "cors1");
        }

        // ---------------------------------------------------------------------------------

        [TestMethod]
        public void exceptions1()
        {
            TestDemo(
                @"
<jsnlog>
	<ajaxAppender 
		name=""appender1"" 
		storeInBufferLevel=""DEBUG"" 
		level=""FATAL"" 
		sendWithBufferLevel=""FATAL"" 
		bufferSize=""20""/>
	<logger appenders=""appender1""/>
</jsnlog>
",
                @"
new JsnlogConfiguration {
    ajaxAppenders=new List<AjaxAppender> {
        new AjaxAppender {
		    name=""appender1"", 
		    storeInBufferLevel=""DEBUG"", 
		    level=""FATAL"", 
		    sendWithBufferLevel=""FATAL"", 
		    bufferSize=20
        }
    },
    loggers=new List<Logger> {
        new Logger {
            appenders=""appender1""
        }
    }
}",
                "exceptions1");
        }

        // ---------------------------------------------------------------------------------

        [TestMethod]
        public void requestids1()
        {
            TestDemo(
                @"
<jsnlog serverSideMessageFormat="""+ _newCodeStart + @"%requestId" + _newCodeEnd + @" | %logger | %level | %message"">
</jsnlog>",
                @"
new JsnlogConfiguration {
    serverSideMessageFormat=""" + _newCodeStart + @"%requestId" + _newCodeEnd + @" | %logger | %level | %message""
}",
                "requestids1");
        }

        [TestMethod]
        public void requestids2()
        {
            TestDemo(
                @"
<jsnlog serverSideMessageFormat=
    ""{ " + _newCodeStart + @"'requestId': '%requestId'," + _newCodeEnd + @" 'clientdate': '%date', 'url': '%url', 'logmessage': %jsonmessage }"">
</jsnlog>",
                @"
new JsnlogConfiguration {
    serverSideMessageFormat=
        ""{ " + _newCodeStart + @"'requestId': '%requestId'," + _newCodeEnd + @" 'clientdate': '%date', 'url': '%url', 'logmessage': %jsonmessage }""
}",
                "requestids2");
        }

        // ---------------------------------------------------------------------------------

        [TestMethod]
        public void loadingjsfile1()
        {
            TestDemo(
                @"
<jsnlog productionLibraryPath=""" + _newCodeStart + @"https://cdnjs.cloudflare.com/ajax/libs/jsnlog/2.12.1/jsnlog.min.js" + _newCodeEnd + @""">
</jsnlog>",
                @"
new JsnlogConfiguration {
    productionLibraryPath=""" + _newCodeStart + @"https://cdnjs.cloudflare.com/ajax/libs/jsnlog/2.12.1/jsnlog.min.js" + _newCodeEnd + @"""
}",
                "loadingjsfile1");
        }

        [TestMethod]
        public void loadingjsfile2()
        {
            TestDemo(
                @"
<jsnlog " + _strikeThroughStart + @"productionLibraryPath=""~/Scripts/jsnlog.min.js""" + _strikeThroughEnd + @">
</jsnlog>",
                @"
new JsnlogConfiguration {
    " + _strikeThroughStart + @"productionLibraryPath=""~/Scripts/jsnlog.min.js""" + _strikeThroughEnd + @"
}",
                "loadingjsfile2");
        }


        // ---------------------------------------------------------------------------------

        private const string _strikeThroughStart = "(S}";
        private const string _strikeThroughEnd = "{S)";
        private const string _newCodeStart = "(N}";
        private const string _newCodeEnd = "{N)";

        private const string _jsnlogUrl = "/Documentation/Configuration/JSNLog";
        private const string _setJsnlogConfigurationUrl = "/Documentation/JavascriptLogging/SetJsnlogConfiguration";

        /// <summary>
        /// Ensures that the xml will be serialised by JSNLog to the code in csharp.
        /// Also writes HTML to d:\temp\demos.html with premade html for example tabs.
        /// 
        /// In that HTML, you may want to apply strikethrough or the "new code" style.
        /// To do that, use these meta tags:
        /// 
        /// <![CDATA[
        /// (S}...striked through text ...{S)"/>
        /// (N}...striked through text ...{N)"/>
        /// ]]>
        /// </summary>
        /// <param name="configXml"></param>
        /// <param name="csharp"></param>
        public void TestDemo(string configXml, string csharp, string demoId)
        {
            // Testing to ensure xml and code are the same

            XmlElement xe = TestUtils.ConfigToXe(CodeWithoutMeta(configXml));
            var jsnlogConfigurationFromXml = XmlHelpers.DeserialiseXml<JsnlogConfiguration>(xe);

            JsnlogConfiguration jsnlogConfigurationFromCode = (JsnlogConfiguration)TestUtils.Eval(CodeWithoutMeta(csharp));

            TestUtils.EnsureEqualJsnlogConfiguration(jsnlogConfigurationFromXml, jsnlogConfigurationFromCode);

            // Write partial

            var sb = new StringBuilder();
            sb.AppendLine(string.Format("@* GENERATED CODE - by class DemoTests in JSNLog.Tests project. Demo {0}. *@", demoId));

            sb.AppendLine(@"<div class=""commontabs""><div data-tab=""Web.config"">");
            sb.AppendLine(@"");
            sb.AppendLine(string.Format(@"<pre>{0}</pre>", CodeToHtml(configXml)));
            sb.AppendLine(@"");
            sb.AppendLine(@"</div><div data-tab=""JsnlogConfiguration"">");
            sb.AppendLine(@"");
            sb.AppendLine(string.Format(@"<pre>JavascriptLogging.{0}({1});</pre>",
                LinkedText("SetJsnlogConfiguration", _setJsnlogConfigurationUrl),
                CodeToHtml(csharp)));
            sb.AppendLine(@"");
            sb.AppendLine(@"</div></div>");

            string path = Path.Combine(_demosDirectory, string.Format("_{0}.cshtml", demoId));
            string content = sb.ToString();

            bool fileExists = File.Exists(path);
            Assert.IsFalse(fileExists, string.Format("{0} already exists", path));

            System.IO.File.WriteAllText(path, content);
        }

        private string CodeWithoutMeta(string code)
        {
            return code.Replace(_strikeThroughStart, "").Replace(_newCodeStart, "").Replace(_strikeThroughEnd, "").Replace(_newCodeEnd, "");
        }

        private string LinkedText(string text, string url)
        {
            return string.Format("<a href='{0}'>{1}</a>", url, text);
        }

        private string CodeToHtml(string code)
        {
            return HttpUtility.HtmlEncode(code.Trim().Replace("\t", "    "))
                .Replace(_strikeThroughStart, "<del>").Replace(_newCodeStart, "<span class='addedcode'>")
                .Replace(_strikeThroughEnd, "</del>").Replace(_newCodeEnd, "</span>")
                .Replace("JsnlogConfiguration", LinkedText("JsnlogConfiguration", _jsnlogUrl))
                .Replace("&lt;jsnlog", "&lt;" + LinkedText("jsnlog", _jsnlogUrl));
        }

    }
}
