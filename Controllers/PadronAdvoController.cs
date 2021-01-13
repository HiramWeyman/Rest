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
                return db.Padron_advo.OrderBy(x => x.pad_nombre).ToList();

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

        [Route("api/updateEstatus/{mat}")]
        [HttpPut]
        public HttpResponseMessage PutUserPad(long mat, PadronCLS userCLS)
        {

            try
            {
                mat = userCLS.pad_mat;
                using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
                {
                    Padron_advo usuariosPad = new Padron_advo();
                    usuariosPad = db.Padron_advo.Where(p => p.pad_mat.Equals(mat)).First();
                    if (usuariosPad == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Trabajador con Matricula " + mat.ToString() + " no encontrado");
                    }
                    else
                    {

                        usuariosPad.pad_situacion = userCLS.pad_situacion;
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

        [Route("api/getUserPadMat")]
        [HttpGet]
        public HttpResponseMessage GetUserPadMat([FromUri]string matricula)
        {

            try
            {
               
                using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
                {

                    //List<Padron_advo> usuario = null;
                    long mat = long.Parse(matricula);
                    //ComprobarCLS usuario = new ComprobarCLS();
                    string query = " SELECT [ub_id],[ub_user],[ub_nombre],[ub_curp],[ub_rfc]  FROM [steujedo_sindicato].[steujedo_sindicato].[User_Base] where [ub_user]=" + matricula;
                    List<Padron_advo> usuario = db.Padron_advo.Where(x => x.pad_mat.Equals(mat)).ToList();
                    //usuario = db.User_Base.FirstOrDefault(e => e.ub_user == matricula && e.ub_curp==curp && e.ub_rfc==rfc);
                    if (usuario != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, usuario);
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Trabajador  no encontrado");
                    }

                }

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

        }

        [Route("api/getUserPadNom")]
        [HttpGet]
        public HttpResponseMessage GetUserPadNom([FromUri]string nombre)
        {

            try
            {
                List<Padron_advo> listaEmpleadoNombre = null;
                using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
                {
  
                    listaEmpleadoNombre = db.Padron_advo.Where(p => p.pad_nombre.Contains(nombre)).ToList();
          
                    if (listaEmpleadoNombre != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, listaEmpleadoNombre);
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Trabajador  no encontrado");
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
