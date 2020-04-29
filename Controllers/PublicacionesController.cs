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
                return db.Publicaciones.OrderByDescending(x => x.pub_id).ToList();

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

    }

}
