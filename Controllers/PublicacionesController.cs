using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Rest.Models;
using System.Net.Http;
using System.Web.Http;
using System.Net;

namespace Rest.Controllers
{
    public class PublicacionesController : ApiController
    {

        [HttpGet]
        public IEnumerable<Publicacione> Get()
        {
            using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                return db.Publicaciones.Where(y => y.pub_id_categoria==1).OrderByDescending(x => x.pub_id).ToList();

            }
        }

        public HttpResponseMessage Get(int id)
        {
            using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                var publicacion = db.Publicaciones.FirstOrDefault(x => x.pub_id == id);
                if (publicacion != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, publicacion);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Publicacion no encontrado.");
                }

            }
        }

        public HttpResponseMessage Get(String notaPrincipal)
        {
            using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                var publicacion = db.Publicaciones.FirstOrDefault(x => x.pub_id_categoria == 1);
                if (publicacion != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, publicacion);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Publicacion no encontrado.");
                }

            }
        }

        public HttpResponseMessage Put(int id, PublicacionesCLS publicacionesCLS)
        {

            try
            {
                id = publicacionesCLS.pub_id;
                using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
                {
                    Publicacione publicacion = db.Publicaciones.Where(p => p.pub_id.Equals(id)).First();
                    if (publicacion == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Publicacion no encontrado");
                    }
                    else
                    {
                        publicacion.pub_titulo = publicacionesCLS.pub_titulo;
                        publicacion.pub_subtitulo = publicacionesCLS.pub_subtitulo;
                        publicacion.pub_texto = publicacionesCLS.pub_texto;
                        publicacion.pub_u_publica = publicacionesCLS.pub_u_publica;
                        publicacion.pub_f_publica = DateTime.Now;
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
