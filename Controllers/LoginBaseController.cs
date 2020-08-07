using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using Rest.Models;
using System.Security.Cryptography;
using System.Text;
using System.Data.SqlClient;

namespace Rest.Controllers
{
    public class LoginBaseController : ApiController
    {
        public HttpResponseMessage Post(UserBaseCLS log)
        {
            //string user_login = "";
            Console.WriteLine(log);
            using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
            {
                string contraCifrada = "";
                if (log.ub_password != null)
                {
                    SHA256Managed sha = new SHA256Managed();
                    byte[] byteContra = Encoding.Default.GetBytes(log.ub_password);
                    byte[] byteContraCifrado = sha.ComputeHash(byteContra);
                    contraCifrada = BitConverter.ToString(byteContraCifrado).Replace("-", "");
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Falta  Ingresar password  " + log.ub_user.ToString());
                }

                db.Configuration.LazyLoadingEnabled = false;
                var user = db.User_Base.FirstOrDefault(e => e.ub_user == log.ub_user && e.ub_password == contraCifrada );
                if (user != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, user);
                    //user_login = db.Database.SqlQuery<string>("Select user_login from Usuarios where user_login=@usuario and password=@password adnd role_id=1", new SqlParameter("@usuario", usr), new SqlParameter("@password", password)).FirstOrDefault();
                    //Session["Usuario"] = user_login;

                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Trabajador  " + log.ub_user.ToString() + " no encontrado");
                }

            }
        }
    }
}
