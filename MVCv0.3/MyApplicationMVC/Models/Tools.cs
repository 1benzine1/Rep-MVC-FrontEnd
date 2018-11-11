using System;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.IO;

namespace MyApplicationMVC.Models
{
    public class Tools
    {
        LogStringParts logStrPart = new LogStringParts();
        List<LogStringParts> listLogParts = new List<LogStringParts>();
        int id = 1;

        public string[] RegexMethod(string line)
        {
            string[] replaceableChars = new string[6] { "- ", "\"", "[", "]", "HTTP/1.1", "HTTP/1.0" };
            string pattern = @"^((\d{1,3}\.){3}\d{1,3}).+\[(.+)].+?\""(\w+)\s+(\S+).+?\"".+?(\d{3}).+?(\d+)";
            string strReplace = null;

            Regex regex = new Regex(pattern);
            Match match = regex.Match(line);

            while (match.Success)
            {
                strReplace = match.Value;
              
                for (int i = 0; i < replaceableChars.Length; i++)
                {
                    strReplace = strReplace.Replace(replaceableChars[i], "");
                }

                string[] words = strReplace.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                match = match.NextMatch();

                return words;
            }
            return null;

        }
        // Добавить прокси для обхода блокировки лимита запросов на проверку ip.
        public object ShowIpInfo(string ip)
        {
                HttpClient client = new HttpClient();
                var json = client.GetStringAsync("http://ip-api.com/json/" + ip +
                                                    "?fields=status,message,country,city,org").Result;
                var ipinf = JsonConvert.DeserializeObject<IpInfo>(json);
                ipinf.ip = ip;

                return ipinf;
        }

        public List<LogStringParts> ReadFile(string pathLogFile)
        {
            string[] arrWords;

            using (StreamReader sr = new StreamReader(pathLogFile, System.Text.Encoding.Default))
                {
                    Tools tools = new Tools();
                    //int check = 0;
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        //if (check == 3)
                        //    break;
                   arrWords = tools.RegexMethod(line);

                    listLogParts.Add(new LogStringParts()
                    {
                        Id = id,
                        Ip = arrWords[0],
                        DateTime = arrWords[1],
                        TimeZone = arrWords[2],
                        TypeRequest = arrWords[3],
                        FileDownload = arrWords[4],
                        StatusCode = Convert.ToInt32(arrWords[5]),
                        SizeTransmitDate = Convert.ToInt32(arrWords[6])
                    });

                    id++;
                     // check++;
                    }
                }
            return listLogParts;
        }
    }
}