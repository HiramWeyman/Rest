using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;
using System.Web.Http.Cors;
using Rest.Models;
using System.IO;

namespace Rest.Controllers
{
    //[EnableCors("*", "*", "*")]
    public class UsuariosController : ApiController
    {
        [HttpGet]
        public IEnumerable<UsuariosCLS> Get()
        {
            List<UsuariosCLS> listaEmpleado = null;
            using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                listaEmpleado = (from usr in db.Usuarios
                                 join per in db.Perfils
                                 on usr.perfil_id equals per.id 
                                 join act in db.Actividades
                                 on usr.act_id equals act.id orderby usr.nombre_completo
                                
                                 select new UsuariosCLS
                                 {
                                     id = usr.id,
                                     matricula =(long)usr.matricula,
                                     nombre_completo =usr.nombre_completo,
                                     direccion = usr.direccion,
                                     telefono = usr.telefono,
                                     celular = usr.celular,
                                     trabajador_base_rec =usr.trabajador_base_rec,
                                     observaciones =usr.observaciones,
                                     act_id = (int)usr.act_id,
                                     role_id = (int)usr.role_id,
                                     perfil_desc=per.perfil_desc ?? "default",
                                     actividad_desc = act.actividad_desc
                                 }).ToList();
                return listaEmpleado;

            }
        }


        public HttpResponseMessage Get(int id)
        {
            using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                var user = db.Usuarios.FirstOrDefault(e => e.id == id);
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

        [HttpPost]
        public HttpResponseMessage Post(string usr,UsuariosCLS userCLS)
        {

            try
            {
                using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
                {
                    Console.WriteLine(usr);
                    Random rdn = new Random();
                    int a = rdn.Next(100, 900);
                    int b = rdn.Next(100, 900);
                    string c = a + "" + b;

                    Usuario usuarios = new Usuario();
                    usuarios.matricula = long.Parse(c);
                    usuarios.nombre_completo = userCLS.nombre_completo;
                    usuarios.direccion = userCLS.direccion;
                    usuarios.telefono = userCLS.telefono;
                    usuarios.celular = userCLS.celular;
                    usuarios.trabajador_base_rec = userCLS.trabajador_base_rec;
                    usuarios.observaciones = userCLS.observaciones;
                    usuarios.perfil_id = userCLS.perfil_id;
                    usuarios.act_id = userCLS.act_id;
                    usuarios.role_id = userCLS.role_id;
                    usuarios.user_login = userCLS.user_login;
                    if (userCLS.password != null)
                    {
                        SHA256Managed sha = new SHA256Managed();
                        byte[] byteContra = Encoding.Default.GetBytes(userCLS.password);
                        byte[] byteContraCifrado = sha.ComputeHash(byteContra);
                        string contraCifrada = BitConverter.ToString(byteContraCifrado).Replace("-", "");
                        usuarios.password = contraCifrada;
                    }
                    else {
                        usuarios.password = "";
                    }
                    usuarios.user_add = usr;
                 
                    db.Usuarios.Add(usuarios);
                    db.SaveChanges();
                    var Mensaje = Request.CreateResponse(HttpStatusCode.Created, userCLS);
                    return Mensaje;
                }

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

        }


        public HttpResponseMessage Put(int id,UsuariosCLS userCLS)
        {

            try
            {
                id = userCLS.id;
                using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
                {
                    Usuario usuario = db.Usuarios.Where(p => p.id.Equals(id)).First();
                    if (usuario == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Trabajador con ID " + id.ToString() + " no encontrado");
                    }
                    else
                    {
                        usuario.nombre_completo = userCLS.nombre_completo;
                        usuario.direccion = userCLS.direccion;
                        usuario.telefono = userCLS.telefono;
                        usuario.celular = userCLS.celular;
                        usuario.trabajador_base_rec = userCLS.trabajador_base_rec;
                        usuario.observaciones = userCLS.observaciones;
                        usuario.perfil_id = userCLS.perfil_id;
                        usuario.act_id = userCLS.act_id;
                        usuario.role_id = userCLS.role_id;
                        usuario.user_login = userCLS.user_login;
                        if (userCLS.password == null|| userCLS.password =="") {
                            usuario.password = null;
                        }  
                        else
                        {
                            SHA256Managed sha = new SHA256Managed();
                            byte[] byteContra = Encoding.Default.GetBytes(userCLS.password);
                            byte[] byteContraCifrado = sha.ComputeHash(byteContra);
                            string contraCifrada = BitConverter.ToString(byteContraCifrado).Replace("-", "");
                            usuario.password = contraCifrada;
                            
                        }
                        usuario.user_mod = userCLS.usr_mod;
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
                    var user = db.Usuarios.FirstOrDefault(e => e.id == id);
                    if (user == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Trabajador con Id" + id.ToString() + " no encontrado");

                    }
                    else
                    {
                        db.Usuarios.Remove(user);
                        db.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, user);
                    }


                }

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

        }



        [Route("api/Actividades")]
        [HttpGet]
        public IEnumerable<Actividade> GetActividades()
        {
            using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                return db.Actividades.OrderBy(x => x.actividad_desc).ToList();

            }
        }

        [Route("api/Actividades/{id}")]
        [HttpGet]
        public HttpResponseMessage GetActividades(int id) {

            using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                var act = db.Actividades.FirstOrDefault(e => e.id == id);
                if (act != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, act);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Actividad con Id" + id.ToString() + " no encontrado");
                }

            }
        }

        [Route("api/Actividades")]
        [HttpPost]
        public HttpResponseMessage PostAct(string usr,ActividadCLS actCLS) {
            try
            {
                using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
                {
                   
                    Actividade actividad = new Actividade();
                    actividad.actividad_desc = actCLS.actividad_desc;
                    actividad.user_add = usr;
                    db.Actividades.Add(actividad);
                    db.SaveChanges();
                    var Mensaje = Request.CreateResponse(HttpStatusCode.Created, actCLS);
                    return Mensaje;
                }

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }


        [Route("api/Actividades/{id}")]
        [HttpPut]
        public HttpResponseMessage PutAct(int id, ActividadCLS actCLS) {
            try
            {
                id = actCLS.id;
                using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
                {
                    Actividade act = db.Actividades.Where(p => p.id.Equals(id)).First();
                    if (act == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Actividad con ID " + id.ToString() + " no encontrada");
                    }
                    else
                    {
                        act.actividad_desc = actCLS.actividad_desc;
                        act.user_mod = actCLS.user_mod;
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

        [Route("api/Actividades/{id}")]
        [HttpDelete]
        public HttpResponseMessage DeleteAct(int id)
        {

            try
            {

                using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
                {
                    var act = db.Actividades.FirstOrDefault(e => e.id == id);
                    if (act == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Actividad con Id" + id.ToString() + " no encontrada");

                    }
                    else
                    {
                        db.Actividades.Remove(act);
                        db.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, act);
                    }


                }

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

        }

        [Route("api/Perfil")]
        [HttpGet]
        public IEnumerable<Perfil> GetPerfil()
        {
            using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                return db.Perfils.OrderBy(x => x.perfil_desc).ToList();

            }
        }

        [Route("api/Perfil/{id}")]
        [HttpGet]
        public HttpResponseMessage GetPerfil(int id) {

            using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                var per = db.Perfils.FirstOrDefault(e => e.id == id);
                if (per != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, per);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Perfil con Id" + id.ToString() + " no encontrado");
                }

            }
        }

        [Route("api/Perfil")]
        [HttpPost]
        public HttpResponseMessage PostPer(string usr, PerfilCLS perCLS)
        {
            try
            {
                using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
                {

                    Perfil per = new Perfil();
                    per.perfil_desc = perCLS.perfil_desc;
                    per.user_add = usr;
                    db.Perfils.Add(per);
                    db.SaveChanges();
                    var Mensaje = Request.CreateResponse(HttpStatusCode.Created, perCLS);
                    return Mensaje;
                }

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }


        [Route("api/Perfil/{id}")]
        [HttpPut]
        public HttpResponseMessage PutPer(int id, PerfilCLS perCLS)
        {
            try
            {
                id = perCLS.id;
                using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
                {
                    Perfil per = db.Perfils.Where(p => p.id.Equals(id)).First();
                    if (per == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Perfil con ID " + id.ToString() + " no encontrado");
                    }
                    else
                    {
                        per.perfil_desc = perCLS.perfil_desc;
                        perCLS.user_mod = perCLS.user_mod;
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

        [Route("api/Perfil/{id}")]
        [HttpDelete]
        public HttpResponseMessage DeletePer(int id)
        {

            try
            {

                using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
                {
                    var per = db.Perfils.FirstOrDefault(e => e.id == id);
                    if (per == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Perfil con Id" + id.ToString() + " no encontrado");

                    }
                    else
                    {
                        db.Perfils.Remove(per);
                        db.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, per);
                    }


                }

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

        }
        [Route("api/Roles")]
        [HttpGet]
        public IEnumerable<Role> GetRoles()
        {
            using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                return db.Roles.OrderBy(x => x.rol_desc).ToList();

            }
        }



    }
}
