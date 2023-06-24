using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.IO;


namespace Rest.Controllers
{
    public class UploadAudiosController : ApiController
    {
        //public HttpResponseMessage Post(string ruta)
        [HttpPost]
        public HttpResponseMessage Post()
        {

            HttpResponseMessage result = null;
            var httpRequest = HttpContext.Current.Request;
            if (httpRequest.Files.Count > 0)
            {
                var docfiles = new List<string>();
                foreach (string file in httpRequest.Files)
                {
                    var postedFile = httpRequest.Files[file];
                    var filePath = HttpContext.Current.Server.MapPath("~/Areas/" + postedFile.FileName);
                    postedFile.SaveAs(filePath);
                    docfiles.Add(filePath);

                    try
                    {

                        //Create a FTP Request Object and Specfiy a Complete Path
                        FtpWebRequest reqObj = (FtpWebRequest)WebRequest.Create("ftp://localhost/httpdocs/assets/audios/" + postedFile.FileName);

                        //Call A FileUpload Method of FTP Request Object
                        reqObj.Method = WebRequestMethods.Ftp.UploadFile;

                        //If you want to access Resourse Protected,give UserName and PWD
                        reqObj.Credentials = new NetworkCredential("", "");

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
                result = Request.CreateResponse(HttpStatusCode.Created, docfiles);
            }
            else
            {
                result = Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            return result;
        }
    }
}
