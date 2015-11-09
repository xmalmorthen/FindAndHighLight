using patterns.Utils.HTMLTOPDF;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace searcher.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index2(string text)
        {
            Searcher s = new Searcher();
            //new FileStream("", FileMode.);
            //return Json(s.find(System.IO.File.ReadAllBytes(Server.MapPath("~/Files/bienestar.pdf")), text, new FileStream(Server.MapPath("~/Files/bienestar2.pdf"), FileMode.Create, FileAccess.Write, FileShare.None)), JsonRequestBehavior.AllowGet);            
            return File(s.find(System.IO.File.ReadAllBytes(Server.MapPath("~/Files/bienestar.pdf")), text), "application/pdf");                       
            //byte[] file = System.IO.File.ReadAllBytes(Server.MapPath("~/Files/bienestar2.pdf"));
            //System.IO.File.Delete(Server.MapPath("~/Files/bienestar2.pdf"));
            //return File(file, "application/pdf");
        }

    }
}
