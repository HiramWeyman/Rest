using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Rest.Models;
using System.Net.Http;
using System.Net;

namespace Rest.Controllers
{
    public class VideosFLController : ApiController
    {
        [HttpGet]
        public IEnumerable<Videos_FL> Get()
        {
            using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                //return db.Publicaciones.Where(y => y.pub_id_categoria==1).OrderByDescending(x => x.pub_id).ToList();
                return db.Videos_FL.OrderByDescending(x => x.vid_id).ToList();

            }
        }

        public HttpResponseMessage Get(int id)
        {
            using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                var publicacion = db.Videos_FL.FirstOrDefault(x => x.vid_id == id);
                if (publicacion != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, publicacion);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Video no encontrado.");
                }

            }
        }

        public HttpResponseMessage Post(string usuario, VideosFLCLS videosFLCLS)
        {

            try
            {
                using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
                {

                    Videos_FL video = new Videos_FL();
                    video.vid_titulo = videosFLCLS.vid_titulo;
                    video.vid_src = videosFLCLS.vid_src;
                    video.vid_ancho = videosFLCLS.vid_ancho;
                    video.vid_largo = videosFLCLS.vid_largo;
                    video.vid_u_publica = usuario;
                    video.vid_f_publica = DateTime.Now;


                    db.Videos_FL.Add(video);
                    db.SaveChanges();
                    var Mensaje = Request.CreateResponse(HttpStatusCode.Created, video);

                    return Mensaje;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.InnerException);
            }

        }

        public HttpResponseMessage Put(int id, VideosFLCLS videosFLCLS)
        {

            try
            {
                id = videosFLCLS.vid_id;
                using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
                {
                    Videos_FL video = db.Videos_FL.Where(p => p.vid_id.Equals(id)).First();
                    if (video == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Video no encontrado");
                    }
                    else
                    {
                        video.vid_titulo = videosFLCLS.vid_titulo;
                        video.vid_src = videosFLCLS.vid_src;
                        video.vid_ancho = videosFLCLS.vid_ancho;
                        video.vid_largo = videosFLCLS.vid_largo;
                        db.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK);

                    }

                }

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.StackTrace);
            }

        }

        public HttpResponseMessage Delete(int id)
        {

            try
            {

                using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
                {
                    var video = db.Videos_FL.FirstOrDefault(e => e.vid_id == id);
                    if (video == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Video con Id" + id.ToString() + " no encontrado");

                    }
                    else
                    {
                        db.Videos_FL.Remove(video);
                        db.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, video);
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
