using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyApplicationMVC.Models;

namespace MyApplicationMVC.Controllers
{
    public class HomeController : Controller
    {
        LogStringParts logStringParts = new LogStringParts();


        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        //private bool isValidContentType(string contentType)
        //{
        //    return contentType.Equals("string");
        //}

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase upload)
        {
            Tools tools = new Tools();

            if (upload == null /*|| (!isValidContentType(upload.ContentType))*/)
            {
                ViewBag.Check = "error";

                return View("Parser");
            }

            if (upload.ContentLength > 0)
            {

                var fileName = Path.GetFileName(upload.FileName);
                var path = Path.Combine(Server.MapPath("LogFile/"), fileName);
                upload.SaveAs(path);
                List<LogStringParts> buffer = tools.ReadFile(path);

                ViewBag.Strings = buffer;
            }
            return View("Parser");
        }

        [HttpGet]
        public ActionResult Parser()
        {
            ViewBag.Message = "Parser page.";

            return View();
        }

        public ActionResult IpInfo(string ip)
        {
            Tools tools = new Tools();
            object buffer = tools.ShowIpInfo(ip);
            ViewBag.Ip = ip;

            return View(buffer);
        }
    }
}