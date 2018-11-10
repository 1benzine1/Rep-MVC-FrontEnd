using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyApplicationMVC.Models
{
    public class LogStringParts
    {
        public int Id { get; set; } //  поле в БД auto increment + 1
        public string Ip { get; set; }
        public string DateTime { get; set; }
        public string TimeZone { get; set; }
        public string TypeRequest { get; set; }
        public string FileDownload { get; set; }
        public int StatusCode { get; set; }
        public int SizeTransmitDate { get; set; }
    }
}