using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace CodeKingdom.Loggers
{
    public class FileLogger
    {
        private static FileLogger instance = null;

        public static FileLogger Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new FileLogger();
                }
                return instance;
            }
        }

        public void LogException(Exception ex)
        {
            string logFile = ConfigurationManager.AppSettings["LogFile"];
            string logFileDirectory = ConfigurationManager.AppSettings["LogFileDirectory"];

            string logFilePath = logFileDirectory + logFile;

            string message = string.Format("{0} was thrown on the {1}.{4}For: {2}{3}{4}",
                ex.Message, DateTime.Now, ex.Source, ex.StackTrace, Environment.NewLine);

            if (!Directory.Exists(logFilePath))
            {
                Directory.CreateDirectory(logFileDirectory);
            }

            using (StreamWriter writer = new StreamWriter(logFilePath, true, Encoding.Default))
            {
                writer.Write(message);
            }
        }
            
    }
}