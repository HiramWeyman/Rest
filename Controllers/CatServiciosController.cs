using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Rest.Models;
using System.Net.Http;
using System.Net;

namespace Rest.Controllers
{
    public class CatServiciosController : ApiController
    {
        [HttpGet]
        public IEnumerable<Cat_Servicios> Get()
        {
            using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                return db.Cat_Servicios.OrderByDescending(x => x.cats_id).ToList();

            }
        }

        public HttpResponseMessage Get(int id)
        {
            using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                var cat_servicio = db.Cat_Servicios.FirstOrDefault(x => x.cats_id == id);
                if (cat_servicio != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, cat_servicio);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Servicio no encontrado.");
                }

            }
        }

        public HttpResponseMessage Post(string Usuario, CatServiciosCLS catserviciosCLS)
        {

            try
            {
                using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
                {

                    Cat_Servicios catservicios = new Cat_Servicios();
                    catservicios.cats_descrip = catserviciosCLS.cats_descrip;
                    catservicios.cats_status = catserviciosCLS.cats_status;
                    catservicios.cats_u_captura = Usuario;
                    catservicios.cats_f_captura = DateTime.Now;

                    db.Cat_Servicios.Add(catservicios);
                    db.SaveChanges();
                    var Mensaje = Request.CreateResponse(HttpStatusCode.Created, catserviciosCLS);
                    return Mensaje;
                }

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

        }

        public HttpResponseMessage Put(int id, CatServiciosCLS catserviciosCLS)
        {

            try
            {
                id = catserviciosCLS.cats_id;
                using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
                {
                    Cat_Servicios cat_servicio = db.Cat_Servicios.Where(p => p.cats_id.Equals(id)).First();
                    if (cat_servicio == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Servicio no encontrado");
                    }
                    else
                    {
                        cat_servicio.cats_descrip = catserviciosCLS.cats_descrip;
                        cat_servicio.cats_status = catserviciosCLS.cats_status;
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

        public HttpResponseMessage Delete(int id)
        {

            try
            {

                using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
                {
                    var cat_servicio = db.Cat_Servicios.FirstOrDefault(e => e.cats_id == id);
                    if (cat_servicio == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Servicio con Id" + id.ToString() + " no encontrada");

                    }
                    else
                    {
                        db.Cat_Servicios.Remove(cat_servicio);
                        db.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, cat_servicio);
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
