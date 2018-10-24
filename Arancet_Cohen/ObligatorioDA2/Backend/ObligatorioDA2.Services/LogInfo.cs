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

        public int Id { get; set; }
        public string Messagge { get; set; }
        public string LogType { get; set; }
        public DateTime Date { get; set; }

        public LogInfo() { }

        public LogInfo(string messagge, string type, DateTime date)
        {
            Messagge = messagge;
            LogType = type;
            Date = date;
        }
    }
}
