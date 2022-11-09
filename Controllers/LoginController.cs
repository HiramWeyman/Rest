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
    //[EnableCors("http://localhost:4200", "X-Custom-Header", "*")]
    public class LoginController : ApiController
    {

        //public IEnumerable<Usuario> GetLogin(int id)
        //{
        //    using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
        //    {
        //        db.Configuration.LazyLoadingEnabled = false;
        //        return db.Usuarios.Where(x => x.matricula == id && x.role_id == 1).ToList();

        //    }
        //}

        //public HttpResponseMessage GetLogin(Login log)
        //{
        //    //string user_login = "";
        //    using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
        //    {
        //        SHA256Managed sha = new SHA256Managed();
        //        byte[] byteContra = Encoding.Default.GetBytes(log.password);
        //        byte[] byteContraCifrado = sha.ComputeHash(byteContra);
        //        string contraCifrada = BitConverter.ToString(byteContraCifrado).Replace("-", "");
        //        db.Configuration.LazyLoadingEnabled = false;
        //        var user = db.Usuarios.FirstOrDefault(e => e.user_login == log.user_login && e.password == log.password && e.role_id == 1);
        //        if (user != null)
        //        {
        //            return Request.CreateResponse(HttpStatusCode.OK, user);
        //            //user_login = db.Database.SqlQuery<string>("Select user_login from Usuarios where user_login=@usuario and password=@password adnd role_id=1", new SqlParameter("@usuario", usr), new SqlParameter("@password", password)).FirstOrDefault();
        //            //Session["Usuario"] = user_login;
        //        }
        //        else
        //        {
        //            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Trabajador  " + log.user_login.ToString() + " no encontrado");
        //        }

        //    }
        //}

        public HttpResponseMessage Post(Login log)
        {
            //string user_login = "";
            Console.WriteLine(log);
            using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
            {
                string contraCifrada = "";
                if (log.password != null)
                {
                    SHA256Managed sha = new SHA256Managed();
                    byte[] byteContra = Encoding.Default.GetBytes(log.password);
                    byte[] byteContraCifrado = sha.ComputeHash(byteContra);
                    contraCifrada = BitConverter.ToString(byteContraCifrado).Replace("-", "");
                }
                else {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Falta  Ingresar password  " + log.user_login.ToString());
                }
               
                db.Configuration.LazyLoadingEnabled = false;
                var user = db.Usuarios.FirstOrDefault(e => e.user_login == log.user_login && e.password == contraCifrada && (e.role_id == 1 || e.role_id == 5 || e.role_id == 6 || e.role_id == 7 || e.role_id == 8 || e.role_id == 11));
                if (user != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, user);
                    //user_login = db.Database.SqlQuery<string>("Select user_login from Usuarios where user_login=@usuario and password=@password adnd role_id=1", new SqlParameter("@usuario", usr), new SqlParameter("@password", password)).FirstOrDefault();
                    //Session["Usuario"] = user_login;

                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Trabajador  " + log.user_login.ToString() + " no encontrado");
                }

            }
        }
    }
}
