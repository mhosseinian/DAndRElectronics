using System;

namespace Common.Services
{
    public interface ILogService
    {
        void Info(string message);
        void Warning(string message);
        void Error(string message);
        void Error(string message, Exception ex);
    }
}