using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.IO;

namespace MyApplicationMVC.Models
{
    public class Tools
    {
        public List<LogStringParts> RegexMethod(string pattern, string line)
        {
            LogStringParts logStrPart = new LogStringParts();
            List<LogStringParts> listLogParts = new List<LogStringParts>();
            Regex regex = new Regex(pattern);
            Match match = regex.Match(line);
            string strReplace = null;
            string[] replaceableChars = new string[6] { "- ", "\"", "[", "]", "HTTP/1.1", "HTTP/1.0" };

            while (match.Success)
            {
                strReplace = match.Value;
              
                for (int i = 0; i < replaceableChars.Length; i++)
                {
                    strReplace = strReplace.Replace(replaceableChars[i], "");
                }

                string[] words = strReplace.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                listLogParts.Add(new LogStringParts
                {
                    Ip = words[0],
                    DateTime = words[1],
                    TimeZone = words[2],
                    TypeRequest = words[3],
                    FileDownload = words[4],
                    StatusCode = Convert.ToInt32(words[5]),
                    SizeTransmitDate = Convert.ToInt32(words[6])
                });

                match = match.NextMatch();
            }

            return listLogParts;
        }
        // Добавить прокси для обхода блокировки лимита запросов на проверку ip.
        public void ShowIpInfo(string ipInfo)
        {
            HttpClient client = new HttpClient();
            var json = client.GetStringAsync("http://ip-api.com/json/" + ipInfo +
                                                "?fields=status,message,country,city,org").Result;
            var ipinf = JsonConvert.DeserializeObject<IpInfo>(json);

            Console.WriteLine("\n Status request check ip: " + ipInfo + " " + ipinf.status + "\n Country: " + ipinf.country + "\n City: " +
                                     ipinf.city + "\n Provider: " + ipinf.org + "\n");
        }

        public List<LogStringParts> ReadFile(string pathLogFile)
        {
            List<LogStringParts> result = null;
                using (StreamReader sr = new StreamReader(pathLogFile, System.Text.Encoding.Default))
                {
                    Tools tools = new Tools();
                    int check = 0;
                    string pattern = @"^((\d{1,3}\.){3}\d{1,3}).+\[(.+)].+?\""(\w+)\s+(\S+).+?\"".+?(\d{3}).+?(\d+)";
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (check == 3)
                            break;

                       result = tools.RegexMethod(pattern, line);

                        check++;
                    }
                }
            if(result == null) result = new List<LogStringParts>(1);
            return result;
        }
    }
}