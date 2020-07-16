using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Rest.Models;
using System.Net.Http;
using System.Net;

namespace Rest.Controllers
{
    public class PadronAdvoController : ApiController
    {
        [HttpGet]
        public IEnumerable<Padron_advo> Get()
        {
            using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                return db.Padron_advo.OrderByDescending(x => x.pad_id).ToList();

            }
        }

        public HttpResponseMessage Get(int matricula)
        {
            using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                var administrativo = db.Padron_advo.FirstOrDefault(x => x.pad_mat == matricula);
                if (administrativo != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, administrativo);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Administrativo no encontrado.");
                }

            }
        }

    }
}
