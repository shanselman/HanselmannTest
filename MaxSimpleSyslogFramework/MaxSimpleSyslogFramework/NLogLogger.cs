using System;
using System.IO;

namespace MaxSimpleSyslogFramework
{
    public static class NLogLogger
    {
        private static bool _nlogCalc;

        private static bool _nLogEnabled;

        private static NLog.Logger logger = LogManager.GetCurrentClassLogger();
        public static void Log(Severity severity, string message, string className, Exception e = null)
        {
            if (IsNLogEnabled())
            {
                //NLog.Logger logger = LogManager.GetLogger(className);
                if (severity == Severity.Emergency && severity == Severity.Alert && severity == Severity.Critical)
                {
                    if (e != null)
                    {
                        logger.Fatal(e, message);
                    }
                    else
                    {
                        logger.Fatal(message);
                    }
                }

                if (severity == Severity.Error)
                {
                    if (e != null)
                    {
                        logger.Error(e, message);
                    }
                    else
                    {
                        logger.Error(message);
                    }
                }

                if (severity == Severity.Warn)
                {
                    logger.Warn(message);

                }

                if (severity == Severity.Info)
                {
                    logger.Info(message);
                }

                if (severity == Severity.Debug)
                {
                    logger.Debug(message);
                }

                if (severity == Severity.Notice)
                {
                    logger.Trace(message);
                }
            }
        }


        private static bool IsNLogEnabled()
        {
            if (!_nlogCalc)
            {
                _nlogCalc = true;
                var applicationPath = ToApplicationPath("NLog.config").Replace("file:\\",string.Empty);
                var exists = File.Exists(applicationPath);
                if(exists)
                    _nLogEnabled = true;
            }
            return _nLogEnabled;
        }

        private static string ToApplicationPath(string fileName)
        {
            var exePath = Path.GetDirectoryName(System.Reflection
                .Assembly.GetExecutingAssembly().CodeBase);
            return Path.Combine(exePath, fileName);
        }

    }
}
