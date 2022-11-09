using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.IO;
using Rest.Models;
using System.Linq;

namespace Rest.Controllers
{
    public class PrestamosController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage Post(int id, string bandera)
        {

            HttpResponseMessage result = null;
            var httpRequest = HttpContext.Current.Request;
            if (httpRequest.Files.Count > 0)
            {
                var docfiles = new List<string>();
                var NombreArchivo = "";
                foreach (string file in httpRequest.Files)
                {

                    var postedFile = httpRequest.Files[file];
                    NombreArchivo = postedFile.FileName;
                    var filePath = HttpContext.Current.Server.MapPath("~/Areas/" + postedFile.FileName);
                    postedFile.SaveAs(filePath);
                    docfiles.Add(filePath);

                    try
                    {

                        try
                        {
                        FtpWebRequest reqFTP = null;
                        Stream ftpStream = null;
                        reqFTP = (FtpWebRequest)FtpWebRequest.Create("ftp://65.99.205.97/httpdocs/assets/images/prestamos/" + id);
                        reqFTP.Method = WebRequestMethods.Ftp.MakeDirectory;
                        reqFTP.UseBinary = true;
                        reqFTP.Credentials = new NetworkCredential("steujedo", "Sindicato#1586");
                        FtpWebResponse response2 = (FtpWebResponse)reqFTP.GetResponse();
                        ftpStream = response2.GetResponseStream();
                        ftpStream.Close();
                        response2.Close();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                            //directory already exist I know that is weak but there is no way to check if a folder exist on ftp...
                        }

                        //Call A FileUpload Method of FTP Request Object
                        FtpWebRequest reqObj = (FtpWebRequest)WebRequest.Create("ftp://65.99.205.97/httpdocs/assets/images/prestamos/" + id + "/" + postedFile.FileName);
                        reqObj.Method = WebRequestMethods.Ftp.UploadFile;

                        //If you want to access Resourse Protected,give UserName and PWD
                        reqObj.Credentials = new NetworkCredential("steujedo", "Sindicato#1586");

                        // Copy the contents of the file to the byte array.
                        byte[] fileContents = File.ReadAllBytes(filePath);
                        reqObj.ContentLength = fileContents.Length;

                        //Upload File to FTPServer
                        Stream requestStream = reqObj.GetRequestStream();
                        requestStream.Write(fileContents, 0, fileContents.Length);
                        requestStream.Close();
                        FtpWebResponse response = (FtpWebResponse)reqObj.GetResponse();
                        response.Close();

                        File.Delete(filePath);
                        
                    }

                    catch (Exception Ex)
                    {
                        throw Ex;
                    }

                }

                
                try
                {

                    using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
                    {
                        Caja_Ahorro caja = db.Caja_Ahorro.Where(p => p.pre_id.Equals(id)).First();
                        if (caja == null)
                        {
                            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Prestamo no encontrada");
                        }
                        else
                        {
                            if(bandera.Equals("RECIBO"))
                            {
                                caja.pre_recibo = "assets/images/prestamos/"+id+"/"+NombreArchivo;
                            }
                            if (bandera.Equals("INE"))
                            {
                                caja.pre_ine = "assets/images/prestamos/" + id + "/" + NombreArchivo;
                            }

                            db.SaveChanges();
                            return Request.CreateResponse(HttpStatusCode.OK);
                        }

                    }

                }
                
                catch (Exception ex)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                }
                
                //result = Request.CreateResponse(HttpStatusCode.Created, docfiles);
            }
            else
            {
                result = Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            return result;
        }

        [HttpPut]
        public HttpResponseMessage Put(int id, string valor)
        {

            try
            {
                //id = cajaahorroCLS.pre_id;
                using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
                {
                    Caja_Ahorro caja = db.Caja_Ahorro.Where(p => p.pre_id.Equals(id)).First();
                    if (caja == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Publicacion no encontrada");
                    }
                    else
                    {
                        caja.pre_estatus = valor;
                        db.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK);

                    }

                }

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

        }
    }
}
