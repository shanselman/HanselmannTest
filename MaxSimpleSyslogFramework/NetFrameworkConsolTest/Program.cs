using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetFrameworkConsolTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Program p = new Program();
            p.Ini();
            //p.Test();
            //p.TestException();

            p = new Program();
            //p.TestWithNewInstans();
        }

        private void Ini()
        {
            MaxSimpleSyslogFramework.Syslog.Initialize("127.0.0.1", 514);
        }

        //public void Test()
        //{
        //    Ini();
        //    //log.Info("Test");
        //    Logger.Warn("Warning");
        //    Logger.Emergency("Emergency");
        //    Logger.Info("Info");
        //    Logger.Debug("Debug");
        //    //Console.Read();
        //}

        //public void TestWithNewInstans()
        //{
        //    Logger.Warn("Warning - New Instans");
        //}

        //public void TestException()
        //{
        //    Ini();
        //    try
        //    {
        //        throw new Exception("Some random exception");
        //    }
        //    catch (Exception e)
        //    {
        //        Logger.Error("Test with exception", e);
        //    }
        //}
    }
}
