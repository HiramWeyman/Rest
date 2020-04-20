using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using Rest.Models;

namespace Rest.Controllers
{
    [EnableCors("http://localhost:4200", "X-Custom-Header", "*")]
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

        public HttpResponseMessage GetLogin(int id)
        {
            using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                var user = db.Usuarios.FirstOrDefault(e => e.matricula == id && e.role_id==1);
                if (user != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, user);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Trabajador con Id" + id.ToString() + " no encontrado");
                }

            }
        }
    }
}
