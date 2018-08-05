using NLog.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Linq.Enumerable;

namespace nlog_test
{
    class Program
    {
        static void Main(string[] args)
        {
           
            var logger = NLog.LogManager.GetCurrentClassLogger();
            NLog.Targets.FileTarget.FileCompressor = new GZipArchiveFileCompressor();
            foreach (var index in Range(1, 1000))
            { 
                logger.Info("Hello Worldddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddd");
            }
        }
    }
}
