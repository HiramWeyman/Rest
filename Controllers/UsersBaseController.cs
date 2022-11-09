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

        [Route("api/getUserBase")]
        [HttpGet]
        public HttpResponseMessage GetUserBase()
        {

            try
            {

                using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
                {

                    List<ComprobarCLS> usuario = null;
                    //ComprobarCLS usuario = new ComprobarCLS();
                    string query = " SELECT [ub_id],[ub_user],[ub_nombre],[ub_curp],[ub_rfc]  FROM [steujedo_sindicato].[steujedo_sindicato].[User_Base] order by [ub_nombre] ";
                    usuario = db.Database.SqlQuery<ComprobarCLS>(query).ToList();
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


        [Route("api/getUserBaseMat")]
        [HttpGet]
        public HttpResponseMessage GetUserBaseMat([FromUri]string matricula)
        {

            try
            {

                using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
                {

                    List<ComprobarCLS> usuario = null;
                    //ComprobarCLS usuario = new ComprobarCLS();
                    string query = " SELECT [ub_id],[ub_user],[ub_nombre],[ub_curp],[ub_rfc]  FROM [steujedo_sindicato].[steujedo_sindicato].[User_Base] where [ub_user]="+matricula;
                    usuario = db.Database.SqlQuery<ComprobarCLS>(query).ToList();
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


        [Route("api/usuariosbaseNombre")]
        [HttpGet]
        public HttpResponseMessage GetUserBaseNom([FromUri]string nombre)
        {

            try
            {

                using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
                {

                    List<ComprobarCLS> usuario = null;
                    //ComprobarCLS usuario = new ComprobarCLS();
                    string query = " SELECT [ub_id],[ub_user],[ub_nombre],[ub_curp],[ub_rfc]  FROM [steujedo_sindicato].[steujedo_sindicato].[User_Base] where [ub_nombre] LIKE '"+nombre+"%'";
                    usuario = db.Database.SqlQuery<ComprobarCLS>(query).ToList();
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

        [Route("api/updateuserbase/{user}")]
        [HttpPut]
        public HttpResponseMessage PutBaseUser(string user, UserBaseCLS userCLS)
        {

            try
            {
                user = userCLS.ub_user;
                using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
                {
                    User_Base usuarios = new User_Base();
                    usuarios = db.User_Base.Where(p => p.ub_user.Equals(user)).First();
                    if (usuarios == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Trabajador con ID " + user.ToString() + " no encontrado");
                    }
                    else
                    {
                      
                        if (userCLS.ub_password != null && userCLS.ub_password != "")
                        {

                            usuarios.ub_nombre = userCLS.ub_nombre;
                            usuarios.ub_curp = userCLS.ub_curp;
                            usuarios.ub_rfc = userCLS.ub_rfc;
                            SHA256Managed sha = new SHA256Managed();
                            byte[] byteContra = Encoding.Default.GetBytes(userCLS.ub_password);
                            byte[] byteContraCifrado = sha.ComputeHash(byteContra);
                            string contraCifrada = BitConverter.ToString(byteContraCifrado).Replace("-", "");
                            usuarios.ub_password = contraCifrada;
                         
                            db.SaveChanges();
                            return Request.CreateResponse(HttpStatusCode.OK);
                        }
                        else
                        {
                            usuarios.ub_nombre = userCLS.ub_nombre;
                            usuarios.ub_curp = userCLS.ub_curp;
                            usuarios.ub_rfc = userCLS.ub_rfc;
                            db.SaveChanges();
                            return Request.CreateResponse(HttpStatusCode.OK);
                        }


                    }

                }

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
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
                    userbase.ub_curp = userbaseCLS.ub_curp;
                    userbase.ub_rfc = userbaseCLS.ub_rfc;
                    SHA256Managed sha = new SHA256Managed();
                        byte[] byteContra = Encoding.Default.GetBytes(userbaseCLS.ub_password);
                        byte[] byteContraCifrado = sha.ComputeHash(byteContra);
                        string contraCifrada = BitConverter.ToString(byteContraCifrado).Replace("-", "");
                    userbase.ub_password = contraCifrada;
                    int persona = db.Database.SqlQuery<int>("select count(*) from steujedo_sindicato.User_Base where ub_user="+ userbaseCLS.ub_user)
                    .FirstOrDefault();
                    if (persona > 0)
                    {
                        var Mensaje = Request.CreateResponse(HttpStatusCode.BadRequest,"El usuario ya se encuentra registrado.");
                     
                        return Mensaje;
                    }
                    else {
                        db.User_Base.Add(userbase);
                        db.SaveChanges();
                        var Mensaje = Request.CreateResponse(HttpStatusCode.Created, userbase);
                        return Mensaje;
                    }
                    //var user = db.User_Base.FirstOrDefault(x => x.ub_user == userbaseCLS.ub_user && x.ub_password== userbase.ub_password);
                    //if (user != null)
                    //{
                        
                       
                    //}
                    //else
                    //{
                     
                 
                    //}
                 
                }

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

        }

    }
}
