using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Rest.Models;
using System.Net.Http;
using System.Net;

namespace Rest.Controllers
{
    public class ConcursoPlazasController : ApiController
    {
        [HttpGet]
        public IEnumerable<ConcursoPlazasCLS> Get()
        {
            List<ConcursoPlazasCLS> listaEmpleado = null;
            using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;

                listaEmpleado = (from advos in db.Concurso_Plazas
                                 join plaza in db.Cat_Plazas
                                 on advos.pad_plaza_id equals plaza.catp_id
                                 orderby advos.pad_plaza_id

                                 select new ConcursoPlazasCLS
                                 {
                                     pad_id = advos.pad_id,
                                     pad_plaza_id = advos.pad_plaza_id,
                                     pad_plaza_descrip = plaza.catp_descrip,
                                     pad_mat = advos.pad_mat,
                                     pad_nombre = advos.pad_nombre,
                                     pad_adscripcion = advos.pad_adscripcion,
                                     pad_categoria = advos.pad_categoria,
                                     pad_funcion = advos.pad_funcion,
                                     pad_situacion = advos.pad_situacion,
                                     pad_permanencia = advos.pad_permanencia,
                                     pad_f_ingreso = advos.pad_f_ingreso,
                                     pad_permisos = advos.pad_permisos,
                                     pad_f_antig = advos.pad_f_antig,
                                     pad_n_insaluble = advos.pad_n_insaluble,
                                     pad_adscrip_base = advos.pad_adscrip_base,
                                     pad_catego_base = advos.pad_catego_base,
                                     pad_funcion_base = advos.pad_funcion_base,
                                     pad_situacion_base = advos.pad_situacion_base,
                                     pad_num_contacto = advos.pad_num_contacto,
                                     pad_observaciones = advos.pad_observaciones

                                     /*
                                     id = usr.id,
                                     matricula = (long)usr.matricula,
                                     nombre_completo = usr.nombre_completo,
                                     direccion = usr.direccion,
                                     fecho_ingreso = usr.fecho_ingreso,
                                     telefono = usr.telefono,
                                     celular = usr.celular,
                                     trabajador_base_rec = usr.trabajador_base_rec,
                                     observaciones = usr.observaciones,
                                     act_id = (int)usr.act_id,
                                     role_id = (int)usr.role_id,
                                     perfil_desc = per.perfil_desc,
                                     actividad_desc = act.actividad_desc
                                     */
                                 }).ToList();


                return listaEmpleado;

            }
        }

        public IEnumerable<Concurso_Plazas> Get(int id, string bandera)
        {
            using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                return db.Concurso_Plazas.Where(x => x.pad_plaza_id == id).OrderBy(x => x.pad_f_antig).ToList();
            }
        }

        public HttpResponseMessage Get(int id)
        {
            using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                var concurso = db.Concurso_Plazas.FirstOrDefault(x => x.pad_id == id);
                if (concurso != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, concurso);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Registro no encontrado.");
                }

            }
        }
        
        public HttpResponseMessage Post(ConcursoPlazasCLS concursoplazasCLS)
        {

            try
            {
                using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
                {

                    Concurso_Plazas concurso_plazas = new Concurso_Plazas();
                    concurso_plazas.pad_plaza_id = concursoplazasCLS.pad_plaza_id;
                    concurso_plazas.pad_mat = concursoplazasCLS.pad_mat;
                    concurso_plazas.pad_nombre = concursoplazasCLS.pad_nombre;
                    concurso_plazas.pad_adscripcion = concursoplazasCLS.pad_adscripcion;
                    concurso_plazas.pad_categoria = concursoplazasCLS.pad_categoria;
                    concurso_plazas.pad_sueldo = concursoplazasCLS.pad_sueldo;
                    concurso_plazas.pad_funcion = concursoplazasCLS.pad_funcion;
                    concurso_plazas.pad_situacion = concursoplazasCLS.pad_situacion;
                    concurso_plazas.pad_permanencia = concursoplazasCLS.pad_permanencia;
                    concurso_plazas.pad_f_ingreso = concursoplazasCLS.pad_f_ingreso;
                    concurso_plazas.pad_f_antig = concursoplazasCLS.pad_f_antig;
                    concurso_plazas.pad_n_insaluble = concursoplazasCLS.pad_n_insaluble;
                    concurso_plazas.pad_adscrip_base = concursoplazasCLS.pad_adscrip_base;
                    concurso_plazas.pad_catego_base = concursoplazasCLS.pad_catego_base;
                    concurso_plazas.pad_funcion_base = concursoplazasCLS.pad_funcion_base;
                    concurso_plazas.pad_situacion_base = concursoplazasCLS.pad_situacion_base;
                    concurso_plazas.pad_num_contacto = concursoplazasCLS.pad_num_contacto;
                    concurso_plazas.pad_observaciones = concursoplazasCLS.pad_observaciones;

                    db.Concurso_Plazas.Add(concurso_plazas);
                    db.SaveChanges();
                    var Mensaje = Request.CreateResponse(HttpStatusCode.Created, concursoplazasCLS);
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
