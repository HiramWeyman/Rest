using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Rest.Models;

namespace Rest.Controllers
{
    public class ArchivosController : ApiController
    {
        [HttpGet]
        public IEnumerable<Archivos> Get()
        {
            using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                return db.Archivos.Where(x => x.archivo_cancela == "N").OrderByDescending(x => x.archivo_id).ToList();

            }
        }

        public HttpResponseMessage Get(int id)
        {
            using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                var archivo = db.Archivos.FirstOrDefault(x => x.archivo_id == id);
                if (archivo != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, archivo);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Archivo no encontrado.");
                }

            }
        }

        [HttpPost]
        public HttpResponseMessage Post(string nombreArchivo, string seccion, ArchivosCLS archivosCLS)
        {

            try
            {
                using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
                {

                    Archivos archivo = new Archivos();
                    archivo.archivo_descrip = archivosCLS.archivo_descrip;
                    archivo.archivo_ruta = "assets/archivos/" + nombreArchivo;
                    archivo.archivo_seccion = seccion;
                    archivo.archivo_cancela = "N";

                    db.Archivos.Add(archivo);
                    db.SaveChanges();
                    var Mensaje = Request.CreateResponse(HttpStatusCode.Created, archivosCLS);
                    return Mensaje;
                }

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

        }

        [HttpPut]
        public HttpResponseMessage Put(int id, ArchivosCLS archivosCLS)
        {

            try
            {
                id = archivosCLS.archivo_id;
                using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
                {
                    Archivos archivo = db.Archivos.Where(p => p.archivo_id.Equals(id)).First();
                    if (archivo == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Archivo no encontrado.");
                    }
                    else
                    {
                        archivo.archivo_descrip = archivosCLS.archivo_descrip;
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
                    var archivo = db.Archivos.FirstOrDefault(e => e.archivo_id == id);
                    if (archivo == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Archivo no encontrado");

                    }
                    else
                    {
                        db.Archivos.Remove(archivo);
                        db.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, archivo);
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
