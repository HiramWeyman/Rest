using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Rest.Models;

namespace Rest.Controllers
{
    public class IntegrantesController : ApiController
    {
        [HttpGet]
        public IEnumerable<Integrantes> Get()
        {
            using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                return db.Integrantes.Where(y=> y.int_cancelado=="N").OrderBy(x => x.int_orden).ToList();

            }
        }

        public IEnumerable<Integrantes> Get(string bandera)
        {
            using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                return db.Integrantes.OrderBy(x => x.int_orden).ToList();

            }
        }

        public HttpResponseMessage Get(int id)
        {
            using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                var integracion = db.Integrantes.FirstOrDefault(x => x.int_id == id);
                if (integracion != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, integracion);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Integrantes no encontrado.");
                }

            }
        }
        [Route("api/Comite")]
        [HttpGet]
        public HttpResponseMessage GetComite()
        {
            using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
            {

                db.Configuration.LazyLoadingEnabled = false;
                var Comite = db.Comite_Ejecutivo.FirstOrDefault();
                if (Comite != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, Comite);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Comite no encontrado.");
                }


            }
        }


        [HttpPost]
        public HttpResponseMessage Post(string nombreArchivo, IntegrantesCLS integrantesCLS)
        {

            try
            {
                using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
                {

                    Integrantes integrante = new Integrantes();
                    integrante.int_nombre = integrantesCLS.int_nombre;
                    integrante.int_puesto = integrantesCLS.int_puesto;
                    integrante.int_ruta_imagen = "assets/images/integrantes/" + nombreArchivo;
                    integrante.int_cancelado = "N";

                    db.Integrantes.Add(integrante);
                    db.SaveChanges();
                    var Mensaje = Request.CreateResponse(HttpStatusCode.Created, integrantesCLS);
                    return Mensaje;
                }

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

        }

        public HttpResponseMessage Put(int id, IntegrantesCLS integrantesCLS)
        {

            try
            {
                id = integrantesCLS.int_id;
                using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
                {
                    Integrantes integrantes = db.Integrantes.Where(p => p.int_id.Equals(id)).First();
                    if (integrantes == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Integrante no encontrado");
                    }
                    else
                    {
                        integrantes.int_nombre = integrantesCLS.int_nombre;
                        integrantes.int_puesto = integrantesCLS.int_puesto;
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

        [Route("api/ActComite/{id}")]
        [HttpPut]
        public HttpResponseMessage PutComite(int id, string desc)
        {

            try
            {
             
                using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
                {
                    Comite_Ejecutivo comite = db.Comite_Ejecutivo.Where(p => p.com_id.Equals(id)).First();
                    if (comite == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Comite no encontrado");
                    }
                    else
                    {
                        comite.com_descripcion = desc;
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

        public HttpResponseMessage Put(int id, string nombreArchivo, IntegrantesCLS integrantesCLS)
        {

            try
            {
                id = integrantesCLS.int_id;
                using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
                {
                    Integrantes integrantes = db.Integrantes.Where(p => p.int_id.Equals(id)).First();
                    if (integrantes == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Integrante no encontrado");
                    }
                    else
                    {
                        integrantes.int_ruta_imagen = integrantes.int_ruta_imagen = "assets/images/integrantes/" + nombreArchivo;
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
                    var integrante = db.Integrantes.FirstOrDefault(e => e.int_id == id);
                    if (integrante == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Integrante con Id" + id.ToString() + " no encontrado");

                    }
                    else
                    {
                        db.Integrantes.Remove(integrante);
                        db.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, integrante);
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
