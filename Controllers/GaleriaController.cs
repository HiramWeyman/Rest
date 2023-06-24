using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Rest.Models;
using System.Net.Http;
using System.Net;
using System.IO;

namespace Rest.Controllers
{
    public class GaleriaController : ApiController
    {
        [HttpGet]
        public IEnumerable<Galeria> Get()
        {
            using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                return db.Galeria.OrderByDescending(x => x.gal_id).ToList();

            }
        }

        public HttpResponseMessage Get(int id)
        {
            using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                var galeria = db.Galeria.FirstOrDefault(x => x.gal_id == id);
                if (galeria != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, galeria);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Galería no encontrada.");
                }

            }
        }

        public HttpResponseMessage Post(string Usuario, GaleriaCLS galeriaCLS)
        {

            try
            {
                using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
                {

                    Galeria galeria = new Galeria();
                    galeria.gal_titulo = galeriaCLS.gal_titulo;
                    galeria.gal_u_publica = Usuario;
                    galeria.gal_f_publica = DateTime.Now;
                    galeria.gal_cancela = "N";

                    db.Galeria.Add(galeria);
                    db.SaveChanges();
                    var Mensaje = Request.CreateResponse(HttpStatusCode.Created, galeria);

                    WebRequest request = WebRequest.Create("ftp://localhost/httpdocs/assets/images/galeria/" + galeria.gal_id);
                    request.Method = WebRequestMethods.Ftp.MakeDirectory;
                    request.Credentials = new NetworkCredential("", "");
                    using (var resp = (FtpWebResponse)request.GetResponse())
                    {
                        //return Request.CreateResponse(resp.StatusCode);
                    }

                    return Mensaje;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.InnerException);
            }

        }

        public HttpResponseMessage Put(int id, string Usuario, GaleriaCLS galeriaCLS)
        {

            try
            {
                id = galeriaCLS.gal_id;
                using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
                {
                    Galeria galeria = db.Galeria.Where(p => p.gal_id.Equals(id)).First();
                    if (galeria == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Galeria no encontrada");
                    }
                    else
                    {
                        galeria.gal_titulo = galeriaCLS.gal_titulo;
                        galeria.gal_u_publica = Usuario;
                        galeria.gal_f_publica = DateTime.Now;
                        db.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK);

                    }

                }

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.StackTrace);
            }

        }

        public HttpResponseMessage Delete(int id)
        {

            try
            {

                using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
                {
                    var galeria = db.Galeria.FirstOrDefault(e => e.gal_id == id);
                    if (galeria == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Galeria con Id" + id.ToString() + " no encontrado");

                    }
                    else
                    {
                        /*
                        FtpWebRequest reqObj = (FtpWebRequest)WebRequest.Create("ftp://localhost/httpdocs/assets/images/galeria/" + id);
                        reqObj.Credentials = new NetworkCredential("steujedo", "Sindicato#1586");

                        //reqObj.UsePassive = FtpModeUsePassive;
                        reqObj.KeepAlive = false;
                        reqObj.UseBinary = true;
                        reqObj.Method = WebRequestMethods.Ftp.RemoveDirectory;
                        FtpWebResponse response = (FtpWebResponse)reqObj.GetResponse();
                        response.Close();
                        //return response.StatusDescription.ToString();
                        */

                        db.Galeria.Remove(galeria);
                        db.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, galeria);

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
