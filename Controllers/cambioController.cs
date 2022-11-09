using Rest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Web.Http;
using System.Text;

namespace Rest.Controllers
{
    public class cambioController : ApiController
    {
     
        [Route("api/updatepass")]
        [HttpPut]
        public HttpResponseMessage Updatepass([FromUri]string user, [FromUri] string  pass)
        {

            try
            {

                using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
                {
                    string contraCifrada = "";
                    Console.WriteLine(user);
                    Console.WriteLine(pass);
                    if (pass != null)
                    {
                        SHA256Managed sha = new SHA256Managed();
                        byte[] byteContra = Encoding.Default.GetBytes(pass);
                        byte[] byteContraCifrado = sha.ComputeHash(byteContra);
                        contraCifrada = BitConverter.ToString(byteContraCifrado).Replace("-", "");
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Falta  Ingresar password  " + pass);
                    }
                    User_Base usuario = new User_Base();
                    usuario = db.User_Base.FirstOrDefault(e => e.ub_user == user);
                    if (usuario == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Trabajador con ID " + user.ToString() + " no encontrado");
                    }
                    else
                    {
                        usuario.ub_password = contraCifrada;
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

        [Route("api/getUser")]
        [HttpGet]
        public HttpResponseMessage GetUser([FromUri]string matricula, [FromUri] string curp, [FromUri] string rfc)
        {

            try
            {

                using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
                {

                    List<ComprobarCLS> usuario = null;
                    //ComprobarCLS usuario = new ComprobarCLS();
                    //string query = " SELECT [ub_id],[ub_user],[ub_nombre],[ub_curp],[ub_rfc]  FROM [steujedo_sindicato].[steujedo_sindicato].[User_Base] where ub_user="+ matricula + " and ub_curp="+curp+" and ub_rfc="+rfc;
                    string query = " SELECT [ub_id],[ub_user],[ub_nombre],[ub_curp],[ub_rfc]  FROM [steujedo_sindicato].[steujedo_sindicato].[User_Base] where ub_user='" + matricula + "' and ub_curp='" + curp + "' and ub_rfc='" + rfc + "'";
                    usuario = db.Database.SqlQuery<ComprobarCLS>(query).ToList();
                    //usuario = db.User_Base.FirstOrDefault(e => e.ub_user == matricula && e.ub_curp==curp && e.ub_rfc==rfc);
                    if (usuario != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, usuario);
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Trabajador con Matricula" + matricula.ToString() + " no encontrado");
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
