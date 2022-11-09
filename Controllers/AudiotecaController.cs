using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Rest.Models;

namespace Rest.Controllers
{
    public class AudiotecaController : ApiController
    {
        [HttpGet]
        public IEnumerable<Audioteca> Get()
        {
            using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                return db.Audioteca.Where(x => x.status == "A").OrderByDescending(x => x.id).ToList();

            }
        }

        public HttpResponseMessage Post(string nombreArchivo, string Usuario, AudiotecaCLS audiotecaCLS)
        {

            try
            {
                using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
                {

                    Audioteca audioteca = new Audioteca();
                    audioteca.title = audiotecaCLS.title;
                    audioteca.link = "assets/audios/" + nombreArchivo;
                    audioteca.status = "A";
                    audioteca.Usuario = Usuario;

                    db.Audioteca.Add(audioteca);
                    db.SaveChanges();
                    var Mensaje = Request.CreateResponse(HttpStatusCode.Created, audiotecaCLS);
                    return Mensaje;
                }

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

        }

        public HttpResponseMessage Delete(int id)
        {

            try
            {

                using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
                {
                    var audio = db.Audioteca.FirstOrDefault(e => e.id == id);
                    if (audio == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Audio con Id" + id.ToString() + " no encontrado");

                    }
                    else
                    {
                        db.Audioteca.Remove(audio);
                        db.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, audio);
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
