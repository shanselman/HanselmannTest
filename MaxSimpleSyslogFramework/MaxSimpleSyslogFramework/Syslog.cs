using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;

namespace MaxSimpleSyslogFramework
{
    public class Syslog
    {
        private static readonly UdpClient UdpClient = new UdpClient();
        private static Facility _facility;
        private static string _hostName;
        private static int _port;
        private static string _appName;
        private static readonly int VERSION = 1;

        public static void Initialize(string hostName, int port, string appName = "", Facility facility = Facility.User)
        {
            _hostName = hostName;
            _port = port;
            _facility = facility;
            _appName = appName;
        }

        internal void Send(Severity severity, string message, Exception e)
        {
            if (string.IsNullOrWhiteSpace(_hostName) || _port == 0)
                return;
            var constructMessage = ConstructMessage(severity, _facility, message, _appName,e);
            UdpClient.SendAsync(constructMessage, constructMessage.Length, _hostName, _port);
            NLogLogger.Log(severity,message,ClassNameAndMethod(false),e);
        }

        private byte[] ConstructMessage(Severity level, Facility facility, string message, string tag, Exception e)
        {
            int prival = ((int)facility) * 8 + ((int)level);
            string pri = string.Format("<{0}>", prival);
            string timestamp =
                new DateTimeOffset(DateTime.Now, TimeZoneInfo.Local.GetUtcOffset(DateTime.Now)).ToString("yyyy-MM-ddTHH:mm:ss");

            string hostname = string.Empty;
            //found that this code doesn't work on all machines; enclosed in a try block
            try
            {
                hostname = Dns.GetHostEntry(Environment.UserDomainName).HostName;
            }
            catch
            {
                hostname = "unknown";
            }

            if (string.IsNullOrEmpty(tag))
            {
                tag = ClassNameAndMethod(true);
            }
            else
            {
                message = ClassNameAndMethod(false) + message;
            }

            if (e != null)
            {
                message = $"[{message}]{Environment.NewLine} {Environment.NewLine} {e}";
            }

            string header = $"{pri}{VERSION} {timestamp} {hostname} {tag}";

            List<byte> syslogMsg = new List<byte>();
            syslogMsg.AddRange(System.Text.Encoding.ASCII.GetBytes(header));
            if (message != "")
            {
                message = message.Replace($"\r\n", Environment.NewLine);
                syslogMsg.AddRange(Encoding.ASCII.GetBytes(" "));
                syslogMsg.AddRange(Encoding.UTF8.GetBytes(message));
            }

            var array = syslogMsg.ToArray();
            return array;
        }

        private string ClassNameAndMethod(bool isTag)
        {
            var assemblyName =Assembly.GetExecutingAssembly().FullName;

            StackTrace st = new StackTrace();
            for (int i = 0; i < st.FrameCount; i++)
            {
                // Note that high up the call stack, there is only
                // one stack frame.
                StackFrame sf = st.GetFrame(i);
                var module = sf.GetMethod().Module.Name;
                if(module == null)
                    continue;

                module = module.Replace(".dll", string.Empty);

                if (assemblyName.Contains(module))
                    continue;

                var className = sf.GetMethod().DeclaringType.FullName;
                var methodName = sf.GetMethod().Name;
                if (!isTag)
                {
                    return $"[{className}.{methodName}] - ";
                }

                return $"{className}.{methodName} ";
            }

            return String.Empty;
        }


    }

}