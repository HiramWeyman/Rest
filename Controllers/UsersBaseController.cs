using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Rest.Models;
using System.Security.Cryptography;
using System.Text;

namespace Rest.Controllers
{
    public class UsersBaseController : ApiController
    {
        public IEnumerable<User_Base> Get()
        {
            using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                return db.User_Base.OrderByDescending(x => x.ub_nombre).ToList();

            }
        }

        public HttpResponseMessage Get(string Usuario)
        {
            using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                var user = db.User_Base.FirstOrDefault(x => x.ub_user == Usuario);
                if (user != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, user);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Administrativo no encontrado.");
                }

            }
        }

        public HttpResponseMessage Post(UserBaseCLS userbaseCLS)
        {

            try
            {
                using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
                {

                    User_Base userbase = new User_Base();
                    userbase.ub_user = userbaseCLS.ub_user;
                    userbase.ub_nombre = userbaseCLS.ub_nombre;
                        SHA256Managed sha = new SHA256Managed();
                        byte[] byteContra = Encoding.Default.GetBytes(userbaseCLS.ub_password);
                        byte[] byteContraCifrado = sha.ComputeHash(byteContra);
                        string contraCifrada = BitConverter.ToString(byteContraCifrado).Replace("-", "");
                    userbase.ub_password = contraCifrada;

                    db.User_Base.Add(userbase);
                    db.SaveChanges();
                    var Mensaje = Request.CreateResponse(HttpStatusCode.Created, userbase);
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
