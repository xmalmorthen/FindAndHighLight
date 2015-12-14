using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace searcher.Utils
{
    public class Ftp
    {
        private Uri urlServer;
        private String userFtp;
        private String passFtp;
        public String serverUrlString { get; set; }
        public Boolean error { get; set; }
        public String mensaje { get; set; }

        public Ftp()
        { }

        public Ftp(Uri serverUrl, String ftpUser, String ftpPass)
        {
            urlServer = serverUrl;
            userFtp = ftpUser;
            passFtp = ftpPass;
        }

        public static Ftp GetFTP()
        {
            Uri url = new Uri(ConfigurationManager.AppSettings.Get("urlFtp"));
            string usuario = ConfigurationManager.AppSettings.Get("ftpUser");
            string password = ConfigurationManager.AppSettings.Get("ftpPass");
            return new Ftp(url, usuario, password) { serverUrlString = ConfigurationManager.AppSettings.Get("urlFtp") };
        }

        public bool UploadFTP(HttpPostedFile archivo, String nombreArchivo)
        {
            byte[] array = new byte[archivo.InputStream.Length];

            //error = "1";
            try
            {
                archivo.InputStream.Read(array, 0, (int)archivo.InputStream.Length);
                using (MemoryStream fs = new MemoryStream(array))
                {
                    //string url = RemotePath + "//" + archivo.FileName;
                    //error = "2";
                    String url = urlServer.ToString() + '/' + nombreArchivo;
                    // Creo el objeto ftp
                    FtpWebRequest ftp = (FtpWebRequest)FtpWebRequest.Create(url);

                    // Fijo las credenciales, usuario y contraseña
                    ftp.Credentials = new NetworkCredential(userFtp, userFtp);

                    // Le digo que no mantenga la conexión activa al terminar.
                    ftp.KeepAlive = false;

                    // Indicamos que la operación es subir un archivo...
                    ftp.Method = WebRequestMethods.Ftp.UploadFile;

                    // … en modo binario … (podria ser como ASCII)
                    ftp.UseBinary = true;

                    // Indicamos la longitud total de lo que vamos a enviar.
                    ftp.ContentLength = fs.Length;

                    //Indicamos que sea pasivo
                    ftp.UsePassive = true;

                    // Desactivo cualquier posible proxy http.
                    // Ojo pues de saltar este paso podría usar 
                    // un proxy configurado en iexplorer
                    ftp.Proxy = null;

                    // Pongo el stream al inicio
                    fs.Position = 0;

                    // Configuro el buffer a 2 KBytes
                    int buffLength = 2048;
                    byte[] buff = new byte[buffLength];
                    int contentLen;

                    if (ExisteArchivo(nombreArchivo))
                    {
                        this.mensaje = "El Archivo Ya Existe";
                        return false;
                    }

                    // obtener el stream del socket sobre el que se va a escribir.
                    //error = "3";
                    using (Stream strm = ftp.GetRequestStream())
                    {
                        // Leer del buffer 2kb cada vez
                        contentLen = fs.Read(buff, 0, buffLength);

                        //error = "4";
                        // mientras haya datos en el buffer ….
                        while (contentLen != 0)
                        {
                            // escribir en el stream de conexión
                            //el contenido del stream del fichero
                            strm.Write(buff, 0, contentLen);
                            contentLen = fs.Read(buff, 0, buffLength);
                            //error = "5";
                        }
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                error = true;
                mensaje = e.Message;
                return false;
            }

        }

        public bool UploadFTP(Stream stream, String nombreArchivo)
        {
            byte[] array = new byte[stream.Length];

            //error = "1";

            try
            {
                stream.Read(array, 0, (int)stream.Length);

                using (MemoryStream fs = new MemoryStream(array))
                {
                    //string url = RemotePath + "//" + archivo.FileName;
                    //error = "2";
                    string url = urlServer.ToString() + nombreArchivo;
                    // Creo el objeto ftp
                    FtpWebRequest ftp = (FtpWebRequest)FtpWebRequest.Create(url);

                    // Fijo las credenciales, usuario y contraseña
                    ftp.Credentials = new NetworkCredential(userFtp, passFtp);

                    // Le digo que no mantenga la conexión activa al terminar.
                    ftp.KeepAlive = false;

                    // Indicamos que la operación es subir un archivo...
                    ftp.Method = WebRequestMethods.Ftp.UploadFile;

                    // … en modo binario … (podria ser como ASCII)
                    ftp.UseBinary = true;

                    // Indicamos la longitud total de lo que vamos a enviar.
                    ftp.ContentLength = fs.Length;

                    //Indicamos que sea pasivo
                    ftp.UsePassive = true;

                    // Desactivo cualquier posible proxy http.
                    // Ojo pues de saltar este paso podría usar 
                    // un proxy configurado en iexplorer
                    ftp.Proxy = null;

                    // Pongo el stream al inicio
                    fs.Position = 0;

                    // Configuro el buffer a 2 KBytes
                    int buffLength = 2048;
                    byte[] buff = new byte[buffLength];
                    int contentLen;


                    //string errInt = "";
                    //if (ExisteArchivo(nombreArchivo))
                    //{
                    //    this.EliminarArchivo(nombreArchivo);
                    //    //this.mensaje = "El Archivo Ya Existe";
                    //    return true;
                    //}

                    // obtener el stream del socket sobre el que se va a escribir.
                    //error = "3";
                    using (Stream strm = ftp.GetRequestStream())
                    {
                        // Leer del buffer 2kb cada vez
                        contentLen = fs.Read(buff, 0, buffLength);

                        //error = "4";
                        // mientras haya datos en el buffer ….
                        while (contentLen != 0)
                        {
                            // escribir en el stream de conexión
                            //el contenido del stream del fichero
                            strm.Write(buff, 0, contentLen);
                            contentLen = fs.Read(buff, 0, buffLength);
                            //error = "5";
                        }
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                error = true;
                mensaje = e.Message;
                return false;
            }

        }

        public bool ExisteArchivo(String nomFile)
        {

            var request = (FtpWebRequest)WebRequest.Create(urlServer + "/" + nomFile);
            request.Credentials = new NetworkCredential(userFtp, passFtp);
            request.Method = WebRequestMethods.Ftp.GetFileSize;

            try
            {
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                error = true;
                mensaje = "El archivo que intenta subir ya existe";
                return true;
            }
            catch (WebException ex)
            {
                FtpWebResponse response = (FtpWebResponse)ex.Response;
                if (response.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
                {
                    error = true;
                    mensaje = ex.Message;
                    return false;
                }
                error = true;
                mensaje = ex.Message;
                return false;
            }

        }

        public bool EliminarArchivo(String nomFile)
        {
            FtpWebRequest request;
            request = (FtpWebRequest)WebRequest.Create(urlServer.ToString() + "/" + nomFile);

            request.Credentials = new NetworkCredential(userFtp, passFtp);
            request.Method = WebRequestMethods.Ftp.DeleteFile;
            try
            {
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                return true;
            }
            catch (WebException ex)
            {
                FtpWebResponse response = (FtpWebResponse)ex.Response;
                if (response.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
                {
                    error = true;
                    mensaje = ex.Message;
                    return false;
                }
                error = true;
                mensaje = ex.Message;
                return false;
            }
        }

        public byte[] DownloadFTPtoByte(String nomFile)
        {
            WebClient request = new WebClient();

            try
            {
                request.Credentials = new NetworkCredential(userFtp, passFtp);
                string uri = urlServer.ToString() + "/" + nomFile;
                byte[] datos = request.DownloadData(uri);
                return datos;
            }
            catch (Exception e)
            {
                error = true;
                mensaje = e.Message;
                return null;
            }
        }

        public Stream DownloadFTP(String nomFile)
        {
            try
            {
                byte[] archivo = this.DownloadFTPtoByte(nomFile);
                MemoryStream memory = new MemoryStream(archivo);
                return memory;
            }
            catch (Exception e)
            {
                error = true;
                mensaje = e.Message;
                return null;
            }

        }

        public String ExisteDirectorio(String path)
        {
            //peticionFtp.KeepAlive = true;
            var request = (FtpWebRequest)WebRequest.Create(new Uri(urlServer + path));
            request.KeepAlive = true;

            request.Credentials = new NetworkCredential(userFtp, passFtp);
            request.Method = WebRequestMethods.Ftp.MakeDirectory;

            try
            {
                FtpWebResponse respuesta;
                respuesta = (FtpWebResponse)request.GetResponse();
                respuesta.Close();
                return String.Empty;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public List<String> ListFilesInDir(String path)
        {
            List<String> files = new List<string>();
            var request = (FtpWebRequest)WebRequest.Create(new Uri(urlServer + path));
            request.KeepAlive = true;

            request.Credentials = new NetworkCredential(userFtp, passFtp);
            request.Method = WebRequestMethods.Ftp.ListDirectory;
            try
            {
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream);
                while (!reader.EndOfStream)
                {
                    files.Add(reader.ReadLine());
                }
                reader.Close();
                responseStream.Close(); //redundant
                response.Close();
                return files;
            }
            catch (Exception ex)
            {
                return new List<String>();
            }
        }
    }
}
