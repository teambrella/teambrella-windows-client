using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teambrella.Client.Services
{
    public class Logger
    {
        private const string FILENAMEFORMAT = "teambrella_{0}.log";

        private string _filePath = "";
        private Logger()
        {
            _filePath = Path.Combine(Path.GetTempPath(), "Teambrella");
            if (!Directory.Exists(_filePath))
                Directory.CreateDirectory(_filePath);
            _filePath = Path.Combine(_filePath, string.Format(FILENAMEFORMAT, DateTime.Now.ToString("yyyyMM")));
        }

        private void WriteString(string str)
        {
            lock (_lockObject)
            {
                if (File.Exists(_filePath))
                {
                    using (var fr = File.AppendText(_filePath))
                    {
                        fr.Write(str);
                    }
                }
                else
                {
                    using (var fr = File.CreateText(_filePath))
                    {
                        fr.Write(str);
                    }

                }
            }
        }


        private static Logger _logger;
        private static object _lockObject = new object();

        private static Logger GetLogger()
        {
            lock (_lockObject)
            {
                if (_logger == null)
                {
                    _logger = new Logger();
                }
            }
            return _logger;
        }

        public static void WriteMessage(string message)
        {
            var logger = GetLogger();

            string str = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + ": " + message + Environment.NewLine;

            logger.WriteString(str);
        }

        public static void WriteFormatMessage(string message, params object[] pars)
        {
            WriteMessage(string.Format(message, pars));
        }

        public static void WriteException(Exception ex, string message = null)
        {
            var logger = GetLogger();
            string str = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + ": " + (message ?? "Exception is thrown.") + Environment.NewLine + "   ";
#if DEBUG
            str += ex.ToString();
#else
            str += ex.Message;
#endif
            str += Environment.NewLine;

            logger.WriteString(str);
        }
    }
}
