using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Rest.Models;
using System.Net.Http;
using System.Net;

namespace Rest.Controllers
{
    public class PreRevolventeController : ApiController
    {
        [HttpGet]
        public IEnumerable<Pre_Revolvente> Get()
        {
            using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                return db.Pre_Revolvente.OrderByDescending(x => x.pr_id).ToList();

            }
        }

        public IEnumerable<Pre_Revolvente> Get(string matricula)
        {
            using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                return db.Pre_Revolvente.Where(x => x.pr_matricula == matricula).OrderByDescending(x => x.pr_id).ToList();
            }
        }

        public HttpResponseMessage Post(string matricula, string nombre, RevolventeCLS revolventeCLS)
        {

            try
            {
                using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
                {

                    Pre_Revolvente pre_revolvente = new Pre_Revolvente();

                    pre_revolvente.pr_nombre = nombre;
                    pre_revolvente.pr_matricula = matricula;
                    pre_revolvente.pr_telefono = revolventeCLS.pr_telefono;
                    pre_revolvente.pr_ingreso = revolventeCLS.pr_ingreso;
                    pre_revolvente.pr_modificacion = revolventeCLS.pr_modificacion;
                    pre_revolvente.pr_tipo = revolventeCLS.pr_tipo;
                    pre_revolvente.pr_estatus = "PENDIENTE";
                    pre_revolvente.pr_fecha = DateTime.Now;
                    pre_revolvente.pr_adscripcioon = revolventeCLS.pr_adscripcioon;
                    pre_revolvente.pre_tarjeta_cuenta = revolventeCLS.pre_tarjeta_cuenta;
                    pre_revolvente.pre_banco = revolventeCLS.pre_banco;

                    db.Pre_Revolvente.Add(pre_revolvente);
                    db.SaveChanges();
                    var Mensaje = Request.CreateResponse(HttpStatusCode.Created, revolventeCLS);
                    return Mensaje;
                }

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

        }

        [HttpPut]
        public HttpResponseMessage Put(int id, string valor)
        {

            try
            {
                //id = cajaahorroCLS.pre_id;
                using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
                {
                    Pre_Revolvente revolvente = db.Pre_Revolvente.Where(p => p.pr_id.Equals(id)).First();
                    if (revolvente == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Prestamo no encontrado");
                    }
                    else
                    {
                        revolvente.pr_estatus = valor;
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
