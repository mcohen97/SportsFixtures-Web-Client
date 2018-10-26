using System;
using System.Collections.Generic;
using System.Text;

namespace ObligatorioDA2.BusinessLogic
{
    public class LogInfo
    {
        private int id;
        private string message;
        private string logType;
        private DateTime date;
        private string username;

        public int Id { get => id; set => id = value; }
        public string Message { get => message; set => message = value; }
        public string LogType { get => logType; set => logType = value; }
        public DateTime Date { get => date; set => date = value; }
        public string Username { get => username; set => username = value; }

        public LogInfo() { }

        public LogInfo(string message, string type, DateTime date)
        {
            Message = message;
            LogType = type;
            Date = date;
        }

       // override object.Equals
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            LogInfo log = (LogInfo)obj;
            return id == log.id;
        }

    }
}
