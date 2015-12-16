using NLog;
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
        Logger logger = LogManager.GetCurrentClassLogger();
        //
        // GET: /Home/

        public ActionResult Index2(string text)
        {
            /*
            logger.Trace("Sample trace message");
            logger.Debug("Sample debug message");            
            logger.Warn("Sample warning message");
            logger.Error("Sample error message");
            logger.Fatal("Sample fatal error message");
            */

            logger.Info("********************************************************************");
            logger.Info("Inicio de la ejecución del proceso de búsqueda");

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
                logger.Info("Post de la búsqueda texto a buscar '{0}'", frm["text"]);
                string folder = string.Empty;
                if (Session["carpeta"] != null)
                {
                    folder = Session["carpeta"] as string;
                    logger.Info("ruta de localización de los archivos {0}", folder);
                }

                List<string> paths = new List<string>();
                string dirFinal = String.Format("{2}{0}/{1}", folder, "result", "~/Files/PDFFiles/");
                
                Ftp ftp = Ftp.GetFTP();
                logger.Info("FTP intanciado");
                string ex = ftp.ExisteDirectorio(String.Format("{0}/{1}", folder, "result"));
                logger.Info("ruta de localización de los archivos {0} creada en el ftp", folder);                
                List<string> pathsName = ftp.ListFilesInDir(folder);
                logger.Info("Extracción de archivos (t={0}) en el folder", paths.Count);
                
                Searcher s = new Searcher();
                foreach (string p in pathsName)
                {
                    if (!p.Equals(String.Format("{0}/{1}", folder, "result") ))
                    paths.Add(Server.MapPath(String.Format("{0}{1}", "~/Files/PDFFiles/", p )));
                }
                logger.Info("creación de las rutas de origen y destino de los archivos encontrados");
                
                searcher.Utils.HTMLTOPDF.Searcher.Respuesta resp = s._find(frm["text"], paths, Server.MapPath(dirFinal));
                
                logger.Info("Fin de la ejecución del proceso de búsqueda");
                logger.Info("********************************************************************");

                ViewBag.text = frm["text"];
                return View(resp);
            }
            logger.Info("Post de la búsqueda texto a buscar vacío");
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

        public FileContentResult getFile(string nameFile)
        {
            string folder = string.Empty;
            if (Session["carpeta"] != null)
                folder = Session["carpeta"] as string;

            string dirFinal = String.Format("{2}{0}/{1}/{3}", folder, "result", "~/Files/PDFFiles/", nameFile);
            return new FileContentResult(System.IO.File.ReadAllBytes(Server.MapPath(dirFinal)), "application/pdf");                
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
