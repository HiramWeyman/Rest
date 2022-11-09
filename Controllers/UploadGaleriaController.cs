﻿using System;
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
    public class UploadGaleriaController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage Post(int id)
        {

            HttpResponseMessage result = null;
            var httpRequest = HttpContext.Current.Request;
            if (httpRequest.Files.Count > 0)
            {
                var docfiles = new List<string>();
                var acumularNombres = "";
                foreach (string file in httpRequest.Files)
                {

                    var postedFile = httpRequest.Files[file];
                    acumularNombres += postedFile.FileName + ",,";
                    var filePath = HttpContext.Current.Server.MapPath("~/Areas/" + postedFile.FileName);
                    postedFile.SaveAs(filePath);
                    docfiles.Add(filePath);

                    try
                    {

                        //Create a FTP Request Object and Specfiy a Complete Path
                        FtpWebRequest reqObj = (FtpWebRequest)WebRequest.Create("ftp://65.99.205.97/httpdocs/assets/images/galeria/" + id + "/" + postedFile.FileName);

                        //Call A FileUpload Method of FTP Request Object
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
                        Galeria galeria = db.Galeria.Where(p => p.gal_id.Equals(id)).First();
                        if (galeria == null)
                        {
                            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Galería no encontrada");
                        }
                        else
                        {
                            galeria.gal_ruta = acumularNombres;
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
    }
}
