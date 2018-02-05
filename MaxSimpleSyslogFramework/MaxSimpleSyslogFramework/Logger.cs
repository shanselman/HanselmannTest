using System;

namespace MaxSimpleSyslogFramework
{
    public class Logger
    {
    
        public Logger()
        {
        }

        internal static void SendLog(Severity severity, string message, Exception e = null)
        {
            message = string.Format(message);
            Syslog syslog = new Syslog();
            syslog.Send(severity,message,e);
        }

        public static void Emergency(string message)
        {
            SendLog(Severity.Emergency, message);
        }

        public static void Emergency(string message, Exception e)
        {
            SendLog(Severity.Emergency, message,e);
        }

        public static void Alert(string message)
        {
            SendLog(Severity.Alert, message);
        }

        public static void Critical(string message)
        {
            SendLog(Severity.Critical, message);
        }

        public static void Critical(string message, Exception e)
        {
            SendLog(Severity.Critical, message,e);
        }

        public static void Error(string message)
        {
            SendLog(Severity.Error, message);
        }

        public static void Error(string message, Exception e)
        {
            SendLog(Severity.Error, message,e);
        }

        public static void Warn(string message)
        {
            SendLog(Severity.Warn, message);
        }

        public static void Notice(string message)
        {
            SendLog(Severity.Notice, message);
        }

        public static void Info(string message)
        {
            SendLog(Severity.Info, message);
        }

        public static void Debug(string message)
        {
            SendLog(Severity.Debug, message);
        }
    }
}