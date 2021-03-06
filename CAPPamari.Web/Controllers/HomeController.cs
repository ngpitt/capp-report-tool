﻿using System.IO;
using System.Web.Mvc;
using CAPPamari.Web.Helpers;

namespace CAPPamari.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Print(string username)
        {
            var document = PrintingHelper.PrintCappReport(username);
            if (document == null) return View("Error");
            using (var pdfStream = new MemoryStream())
            {
                document.SaveToStream(pdfStream);
                return File(pdfStream.ToArray(), "applicaion/pdf", "CAPP Report.pdf");
            }
        }
    }
}