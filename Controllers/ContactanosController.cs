using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Rest.Models;
using System.Net.Http;
using System.Net;

namespace Rest.Controllers
{
    public class ContactanosController : ApiController
    {
        
        [HttpGet]
        public IEnumerable<Contactanos> Get()
        {
            using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                return db.Contactanos.OrderBy(x=> x.con_id).ToList();

            }
        }

        public HttpResponseMessage Get(int id)
        {
            using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                var contacto = db.Contactanos.FirstOrDefault(x => x.con_id == id);
                if (contacto != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, contacto);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Contacto no encontrado.");
                }

            }
        }

        [HttpPut]
        public HttpResponseMessage Put(int id, ContactanosCLS contactanosCLS)
        {

            try
            {
                id = contactanosCLS.con_id;
                using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
                {
                    Contactanos contacto = db.Contactanos.Where(p => p.con_id.Equals(id)).First();
                    if (contacto == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Contacto no encontrado.");
                    }
                    else
                    {
                        contacto.con_texto = contactanosCLS.con_texto;
                        contacto.con_link = contactanosCLS.con_link;
                        contacto.con_visible = contactanosCLS.con_visible;
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
