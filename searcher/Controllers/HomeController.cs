using searcher.Utils.HTMLTOPDF;
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
            //return File(s.find(System.IO.File.ReadAllBytes(Server.MapPath("~/Files/bienestar.pdf")), text), "application/pdf");

            //searcher.Utils.HTMLTOPDF.Searcher.Respuesta resp = s.find(System.IO.File.ReadAllBytes(Server.MapPath("~/Files/bienestar.pdf")), text);
            searcher.Utils.HTMLTOPDF.Searcher.Respuesta resp = s._find(text, Server.MapPath("~/Files/bienestar.pdf"), Server.MapPath("~/Files/temp"));

            //byte[] file = resp.file;            
            //ViewBag.base64EncodedPdf = Convert.ToBase64String(file);
            return View(resp);
        }
        [HttpPost]
        public ActionResult Index2(FormCollection frm)
        {
            Searcher s = new Searcher();
            //new FileStream("", FileMode.);
            //return Json(s.find(System.IO.File.ReadAllBytes(Server.MapPath("~/Files/bienestar.pdf")), text, new FileStream(Server.MapPath("~/Files/bienestar2.pdf"), FileMode.Create, FileAccess.Write, FileShare.None)), JsonRequestBehavior.AllowGet);            
            //return File(s.find(System.IO.File.ReadAllBytes(Server.MapPath("~/Files/bienestar.pdf")), text), "application/pdf");

            searcher.Utils.HTMLTOPDF.Searcher.Respuesta resp = s.find(System.IO.File.ReadAllBytes(Server.MapPath("~/Files/bienestar.pdf")), frm["text"]);
            
            //byte[] file = resp.file;            
            //ViewBag.base64EncodedPdf = Convert.ToBase64String(file);
            return View(resp);
        }

        public ActionResult Index3(string text)
        {
            Searcher s = new Searcher();
            //new FileStream("", FileMode.);
            //return Json(s.find(System.IO.File.ReadAllBytes(Server.MapPath("~/Files/bienestar.pdf")), text, new FileStream(Server.MapPath("~/Files/bienestar2.pdf"), FileMode.Create, FileAccess.Write, FileShare.None)), JsonRequestBehavior.AllowGet);            
            //return File(s.find(System.IO.File.ReadAllBytes(Server.MapPath("~/Files/bienestar.pdf")), text), "application/pdf");
            return View();
        }

        public ActionResult viewer(string text)
        {
            return View();
        }

        public FileContentResult getFile(string type)
        {

            if(type.Equals("pdf", StringComparison.InvariantCultureIgnoreCase))
                return new FileContentResult(System.IO.File.ReadAllBytes(Server.MapPath("~/Files/bienestar.pdf")), "application/pdf");
            else
                return new FileContentResult(System.IO.File.ReadAllBytes(Server.MapPath("~/Files/doc.docx")), "application/octet-stream");
        }

        public ActionResult find(string text)
        {
            return View();
        }
    }
}
