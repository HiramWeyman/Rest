using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Rest.Models;
using System.Net.Http;
using System.Net;

namespace Rest.Controllers
{
    public class CatPlazasController : ApiController
    {
        [Route("api/CatPlazas")]
        [HttpGet]
        public IEnumerable<Cat_Plazas> Get()
        {
            using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                return db.Cat_Plazas.Where(x => x.catp_status != "S").OrderByDescending(x => x.catp_id).ToList();
                //return db.Cat_Plazas.OrderByDescending(x => x.catp_id).ToList();

            }
        }

        [Route("api/CatPlazasAdmin")]
        [HttpGet]
        public IEnumerable<Cat_Plazas> GetPlaza()
        {
            using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                //return db.Cat_Plazas.Where(x => x.catp_status != "S").OrderByDescending(x => x.catp_id).ToList();
                return db.Cat_Plazas.OrderByDescending(x => x.catp_id).ToList();

            }
        }

        [Route("api/CatPlazas/{id}")]
        [HttpGet]
        public HttpResponseMessage Get(int id)
        {
            using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                var cat_plaza = db.Cat_Plazas.FirstOrDefault(x => x.catp_id == id);
                if (cat_plaza != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, cat_plaza);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Plaza no encontrado.");
                }

            }
        }

        [Route("api/CatPlazas")]
        [HttpPost]
        public HttpResponseMessage Post(string Usuario, CatPlazasCLS catplazasCLS)
        {

            try
            {
                using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
                {

                    Cat_Plazas catplazas = new Cat_Plazas();
                    catplazas.catp_descrip = catplazasCLS.catp_descrip;
                    catplazas.catp_status = catplazasCLS.catp_status;
                    catplazas.catp_u_captura = Usuario;
                    catplazas.catp_f_captura = DateTime.Now;
                    catplazas.catp_categoria = catplazasCLS.catp_categoria;
                    catplazas.catp_funcion = catplazasCLS.catp_funcion;
                    catplazas.catp_adscripcion = catplazasCLS.catp_adscripcion;

                    db.Cat_Plazas.Add(catplazas);
                    db.SaveChanges();
                    var Mensaje = Request.CreateResponse(HttpStatusCode.Created, catplazasCLS);
                    return Mensaje;
                }

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

        }

        [Route("api/CatPlazas/{id}")]
        [HttpPut]
        public HttpResponseMessage Put(int id, CatPlazasCLS catplazasCLS)
        {

            try
            {
                id = catplazasCLS.catp_id;
                using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
                {
                    Cat_Plazas catplazas = db.Cat_Plazas.Where(p => p.catp_id.Equals(id)).First();
                    if (catplazas == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Plaza no encontrado");
                    }
                    else
                    {
                        catplazas.catp_descrip = catplazasCLS.catp_descrip;
                        catplazas.catp_status = catplazasCLS.catp_status;
                        catplazas.catp_categoria = catplazasCLS.catp_categoria;
                        catplazas.catp_funcion = catplazasCLS.catp_funcion;
                        catplazas.catp_adscripcion = catplazasCLS.catp_adscripcion;

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

        [Route("api/CatPlazas/{id}")]
        [HttpDelete]
        public HttpResponseMessage Delete(int id)
        {

            try
            {

                using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
                {
                    var catplazas = db.Cat_Plazas.FirstOrDefault(e => e.catp_id == id);
                    if (catplazas == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Plaza con Id" + id.ToString() + " no encontrada");

                    }
                    else
                    {
                        db.Cat_Plazas.Remove(catplazas);
                        db.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, catplazas);
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
