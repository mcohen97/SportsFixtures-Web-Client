
using System;
using System.ComponentModel.DataAnnotations;


namespace ObligatorioDA2.WebAPI.Models
{
    public class LogModelOut
    {
        public int Id { get; set; }
        public string LogType { get; set; }
        public string Message { get; set; }
        public string Useranme { get; set; }
        public DateTime Date { get; set; }
    }
}
