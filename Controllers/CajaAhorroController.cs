using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Rest.Models;
using System.Net.Http;
using System.Net;

namespace Rest.Controllers
{
    public class CajaAhorroController : ApiController
    {

        [HttpGet]
        public IEnumerable<Caja_Ahorro> Get()
        {
            using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                return db.Caja_Ahorro.OrderByDescending(x => x.pre_id).ToList();

            }
        }

        public IEnumerable<Caja_Ahorro> Get(string tipo)
        {
            using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                return db.Caja_Ahorro.Where(x => x.pre_tipo == tipo).OrderByDescending(x => x.pre_id).ToList();
            }
        }

        public IEnumerable<Caja_Ahorro> Get(string matricula, string tipo)
        {
            using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                return db.Caja_Ahorro.Where(x => x.pre_matricula == matricula && x.pre_tipo == tipo ).OrderByDescending(x => x.pre_id).ToList();
            }
        }

        public HttpResponseMessage Post(string matricula, string nombre, string adscripcion, string tipo, CajaAhorroCLS cajaahorroCLS)
        {

            try
            {
                using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
                {

                    Caja_Ahorro caja_ahorro = new Caja_Ahorro();
                    
                    caja_ahorro.pre_nombre = nombre;
                    caja_ahorro.pre_matricula = matricula;
                    caja_ahorro.pre_adscripcioon = adscripcion;
                    caja_ahorro.pre_tarjeta_cuenta = cajaahorroCLS.pre_tarjeta_cuenta;
                    caja_ahorro.pre_banco = cajaahorroCLS.pre_banco;
                    caja_ahorro.pre_telefono = cajaahorroCLS.pre_telefono;
                    caja_ahorro.pre_cantidad = cajaahorroCLS.pre_cantidad;
                    caja_ahorro.pre_tipo = tipo;
                    caja_ahorro.pre_fecha = DateTime.Now; 
                    
                    db.Caja_Ahorro.Add(caja_ahorro);
                    db.SaveChanges();
                    var Mensaje = Request.CreateResponse(HttpStatusCode.Created, cajaahorroCLS);
                    return Mensaje;
                }

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

        }


    }
}
