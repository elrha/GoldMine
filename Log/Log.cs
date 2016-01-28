using log4net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// note : Script 내부에서 바로 접근 할 수 있도록 가장 상위 NameSpace에 Log 등록
public class Log
{
    private static ILog iLog;

    public static ILog Logger { get { return Log.iLog; } }

    private static bool initialized = Log.Initialize();

    public static bool Initialize()
    {
        var fileLogConfig = string.Format(@"<configuration><configSections><section name=""log4net"" type=""log4net.Config.Log4NetConfigurationSectionHandler, log4net""/></configSections><log4net><appender name=""RollingFile"" type=""log4net.Appender.RollingFileAppender""><file value=""./Logs/{0}.log""/><appendToFile value=""true""/><rollingStyle value=""Composite""/><lockingModel type=""log4net.Appender.FileAppender+MinimalLock"" /><maxSizeRollBackups value=""14""/><maximumFileSize value=""15000KB""/><datePattern value=""-yyyy-MM-dd""/><staticLogFileName value=""true""/><layout type=""log4net.Layout.PatternLayout""><conversionPattern value=""%date [%thread] %-5level - %message%newline""/></layout></appender><appender name=""ConsoleAppender"" type=""log4net.Appender.ConsoleAppender""><layout type=""log4net.Layout.PatternLayout""><conversionPattern value=""%date [%thread] %-5level - %message%newline"" /></layout></appender><logger name=""RollingFile""><level value=""ALL""/><appender-ref ref=""RollingFile""/><appender-ref ref=""ConsoleAppender""/></logger></log4net></configuration>", Process.GetCurrentProcess().ProcessName);

        using (var configStream = new MemoryStream(Encoding.UTF8.GetBytes(fileLogConfig)))
        {
            log4net.Config.XmlConfigurator.Configure(configStream);
            iLog = log4net.LogManager.GetLogger("RollingFile");
        }

        return true;
    }
}
