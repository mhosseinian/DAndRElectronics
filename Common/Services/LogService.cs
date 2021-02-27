using System;
using System.Diagnostics;
using System.IO;

namespace Common.Services
{
    public class LogService : ILogService
    {
        private readonly object _lockObj = new object();
        private readonly string _filename;

        #region Contructors

        public LogService()
        {
            var localDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),"DAndR");
                
            if (!Directory.Exists(localDir))
            {
                Directory.CreateDirectory(localDir);
            }

            _filename = Path.Combine(localDir,  $"{System.AppDomain.CurrentDomain.FriendlyName}.log");
        }

        #endregion

        public void Info(string message)
        {
            Write(message,TraceEventType.Information );
        }

        public void Warning(string message)
        {
            Write(message,TraceEventType.Warning );
        }

        public void Error(string message)
        {
            Write(message, TraceEventType.Error);
        }

        public void Error(string message, Exception ex)
        {
            Write(message, TraceEventType.Error);
            Write($"The exception message : {ex.Message}", TraceEventType.Error);
        }

        private void Write(string message, TraceEventType eventType)
        {
            lock (_lockObj)
            {
                var msg = $"{DateTime.UtcNow},{eventType}:{message}";
                using var streamWriter = new StreamWriter(_filename, true);
                streamWriter.WriteLine(msg);
                streamWriter.Close();
            }

        }
    }
}