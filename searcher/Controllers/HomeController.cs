using searcher.Models;
using searcher.Utils;
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
            if (!string.IsNullOrEmpty(text))
            {
                Searcher s = new Searcher();
                
                searcher.Utils.HTMLTOPDF.Searcher.Respuesta resp = s.find(System.IO.File.ReadAllBytes(Server.MapPath("~/Files/bienestar.pdf")), text);
                if (System.IO.File.Exists(Server.MapPath("~/Files/bienestarResult.pdf")))
                    System.IO.File.Delete(Server.MapPath("~/Files/bienestarResult.pdf"));
                System.IO.File.WriteAllBytes(Server.MapPath("~/Files/bienestarResult.pdf"), resp.file);
                return View(resp);
            }
            else{
                ViewBag.text = "";
                return View(new searcher.Utils.HTMLTOPDF.Searcher.Respuesta());
            }
        }
        
        [HttpPost]
        public ActionResult Index2(FormCollection frm)
        {
            if (!string.IsNullOrEmpty(frm["text"]))
            {
                string folder = string.Empty;
                if (Session["carpeta"] != null)
                    folder = Session["carpeta"] as string;

                List<string> paths = new List<string>();
                string dirFinal = String.Format("{2}{0}/{1}", folder, "result", "~/Files/PDFFiles/");
                
                Ftp ftp = Ftp.GetFTP();
                string ex = ftp.ExisteDirectorio(String.Format("{0}/{1}", folder, "result"));
                List<string> pathsName = ftp.ListFilesInDir(folder);
                Searcher s = new Searcher();

                foreach (string p in pathsName)
                {
                    if (!p.Equals(String.Format("{0}/{1}", folder, "result") ))
                    paths.Add(Server.MapPath(String.Format("{0}{1}", "~/Files/PDFFiles/", p )));
                }

                //searcher.Utils.HTMLTOPDF.Searcher.Respuesta resp = s.find(System.IO.File.ReadAllBytes(Server.MapPath("~/Files/bienestar.pdf")), frm["text"]);
                searcher.Utils.HTMLTOPDF.Searcher.Respuesta resp = s._find(frm["text"], paths, Server.MapPath(dirFinal));
                
                /*if (System.IO.File.Exists(Server.MapPath("~/Files/bienestarResult.pdf")))
                    System.IO.File.Delete(Server.MapPath("~/Files/bienestarResult.pdf"));
                System.IO.File.WriteAllBytes(Server.MapPath("~/Files/bienestarResult.pdf"), resp.file);*/
                ViewBag.text = frm["text"];
                return View(resp);
            }
            ViewBag.text = frm["text"];
            return View(new searcher.Utils.HTMLTOPDF.Searcher.Respuesta());
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
                return new FileContentResult(System.IO.File.ReadAllBytes(Server.MapPath("~/Files/bienestarResult.pdf")), "application/pdf");
            else
                return new FileContentResult(System.IO.File.ReadAllBytes(Server.MapPath("~/Files/doc.docx")), "application/octet-stream");
        }

        [HttpGet]
        public ActionResult filesToSearch()
        {
            if (Session["carpeta"] == null)
            {
                string folderName = Guid.NewGuid().ToString();
                Session["carpeta"] = folderName;
            }
            return View();
        }

        [HttpPost]
        public JsonResult uploadFiles()
        {
            //HttpPostedFileBase item = Request.Files[0];
            var r = new List<files>();
            int i = 0; 
            foreach (string files in Request.Files)
            {
                var statuses = new List<files>();
                var item = Request.Files[i];

                if (item.ContentLength == 0)
                    return Json(new { Exito = false, Mensaje = "El archivo esta vacio" });
                string extension = System.IO.Path.GetExtension(item.FileName).ToLower();
                if (!(extension == ".pdf" || extension == ".doc" || extension == ".docx"))
                    return Json(new { Exito = false, Mensaje = "Sólo se aceptan archivos en formato PDF, DOCX Y DOC" });
                
                Ftp ftp = Ftp.GetFTP();
                ftp.ExisteDirectorio(Session["carpeta"] as string);
                string ruta = string.Format("{0}/{1}", Session["carpeta"], item.FileName);
                if (ftp.ExisteArchivo(ruta))
                    ftp.EliminarArchivo(ruta);
                if (!ftp.UploadFTP(item.InputStream, ruta))
                    return Json(new { Exito = false, Mensaje = "Ocurrió un error al subir la evidencia" });

                statuses.Add(new files()
                {
                    name = item.FileName,
                    size = item.ContentLength,
                    type = item.ContentType,
                    url = item.FileName,
                    delete_url = item.FileName,
                    //thumbnail_url = @"data:image/png;base64," + EncodeFile(file.FileName),
                    delete_type = "GET",                    
                });
                JsonResult result = Json(statuses);
                result.ContentType = "text/plain";
                return result;                
            }            
            return Json(r);
        }

        public ActionResult find(string text)
        {
            return View();
        }
    }
}
