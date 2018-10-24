using System;
using System.Collections.Generic;
using System.Text;

namespace ObligatorioDA2.Data.DataAccess.Entities
{
    public class LogInfoEntity
    {
        public int Id { get; set; }
        public string Messagge { get; set; }
        public string LogType { get; set; }
        public DateTime Date { get; set; }
        public string Username { get; set; }

        public LogInfoEntity() { }

        public LogInfoEntity(string messagge, string type, DateTime date, string username)
        {
            Id = 0;
            Messagge = messagge;
            LogType = type;
            Date = date;
            Username = username;
        }
    }
}
