using System;
using System.Collections.Generic;
using System.Text;

namespace ObligatorioDA2.Services
{
    public class LogInfo
    {
        private int id;
        private string messagge;
        private string logType;
        private DateTime date;
        private string username;

        public int Id { get => id; set => id = value; }
        public string Messagge { get => messagge; set => messagge = value; }
        public string LogType { get => logType; set => logType = value; }
        public DateTime Date { get => date; set => date = value; }
        public string Username { get => username; set => username = value; }

        public LogInfo() { }

        public LogInfo(string messagge, string type, DateTime date)
        {
            Messagge = messagge;
            LogType = type;
            Date = date;
        }
    }
}
