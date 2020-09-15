using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Rest.Models;
using System.Net.Http;
using System.Net;

namespace Rest.Controllers
{
    public class DescArchivosController : ApiController
    {
        [HttpGet]
        public IEnumerable<Descarga_Archivos> Get()
        {
            using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                //return db.Publicaciones.Where(y => y.pub_id_categoria==1).OrderByDescending(x => x.pub_id).ToList();
                return db.Descarga_Archivos.OrderByDescending(x => x.da_id).ToList();

            }
        }

        public HttpResponseMessage Post(string descripcion, string nombre, string matricula)
        {

            try
            {
                using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
                {

                    Descarga_Archivos descarga = new Descarga_Archivos();
                    descarga.da_descripcion = descripcion;
                    descarga.da_nombre = nombre;
                    descarga.da_fecha = DateTime.Now;
                    descarga.da_matricula = matricula;

                    db.Descarga_Archivos.Add(descarga);
                    db.SaveChanges();
                    var Mensaje = Request.CreateResponse(HttpStatusCode.Created, descarga);
                    return Mensaje;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.InnerException);
            }

        }

    }
}
