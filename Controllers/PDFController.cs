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
using iTextSharp.text;
using System.IO;
using iTextSharp.text.pdf;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;
using Font = iTextSharp.text.Font;
using System.Net.Http.Headers;
using System.Web.Http.Results;
using Grpc.Core;

namespace Rest.Controllers
{
    public class PDFController : ApiController
    {
        [Route("api/Reportelista")]
        [HttpGet]
        public HttpResponseMessage CreateLieferschein()
        {

            List<UsuariosCLS> listaEmpleado = null;
            using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                listaEmpleado = (from usr in db.Usuarios
                                 join per in db.Perfils
                                 on usr.perfil_id equals per.id
                                 join act in db.Actividades
                                 on usr.act_id equals act.id
                                 where usr.role_id == 2
                                 && usr.user_baja.Equals(null)
                                 orderby usr.nombre_completo

                                 select new UsuariosCLS
                                 {
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
                                 }).ToList();

                String ruta_img = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "imagenes\\logoSteujed.png");
                iTextSharp.text.Font font1 = new Font(Font.FontFamily.HELVETICA, 12, Font.NORMAL);
                iTextSharp.text.Font font2 = new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD);
                iTextSharp.text.Font font = new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL);
                Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 42, 35);
                byte[] buffer;
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.BadRequest);
                using (MemoryStream stream = new MemoryStream())
                {
                    PdfWriter.GetInstance(doc, stream);
                    doc.Open();

                    iTextSharp.text.Image image1 = iTextSharp.text.Image.GetInstance(ruta_img);
                    //image1.ScalePercent(50f);
                    image1.ScaleAbsoluteWidth(70);
                    image1.ScaleAbsoluteHeight(60);
                    image1.Alignment = Element.ALIGN_CENTER;
                    doc.Add(image1);
                    Paragraph espacio = new Paragraph(" ");
                    doc.Add(espacio);

                    Paragraph title1 = new Paragraph(" SINDICATO DE TRABAJADORES Y EMPLEADOS DE LA UNIVERSIDAD JUÁREZ DEL ESTADO DE DURANGO", font2);
                    title1.Alignment = Element.ALIGN_CENTER;
                    doc.Add(title1);
                    doc.Add(espacio);

                    Paragraph title = new Paragraph("Listado de Empleados Eventuales", font1);
                    title.Alignment = Element.ALIGN_CENTER;
                    doc.Add(title);
                    doc.Add(espacio);

                    ////Creando la tabla
                    PdfPTable tabla = new PdfPTable(9);
                    tabla.WidthPercentage = 100f;
                    ////Asignando los anchos de las columnas
                    float[] valores = new float[9] { 14, 35, 20, 20, 20,20,20,20,30 };
                    tabla.SetWidths(valores);

                    ////Creando celdas agregando contenido
                    PdfPCell celda1 = new PdfPCell(new Phrase("Matricula", font));
                    celda1.BackgroundColor = new BaseColor(240, 240, 240);
                    celda1.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    tabla.AddCell(celda1);

                    PdfPCell celda2 = new PdfPCell(new Phrase("Nombre Completo", font));
                    celda2.BackgroundColor = new BaseColor(240, 240, 240);
                    celda2.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    tabla.AddCell(celda2);

                    PdfPCell celda3 = new PdfPCell(new Phrase("Perfil", font));
                    celda3.BackgroundColor = new BaseColor(240, 240, 240);
                    celda3.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    tabla.AddCell(celda3);

                    PdfPCell celda4 = new PdfPCell(new Phrase("Actividad", font));
                    celda4.BackgroundColor = new BaseColor(240, 240, 240);
                    celda4.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    tabla.AddCell(celda4);

                    PdfPCell celda5 = new PdfPCell(new Phrase("f.ingreso", font));
                    celda5.BackgroundColor = new BaseColor(240, 240, 240);
                    celda5.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    tabla.AddCell(celda5);

                    PdfPCell celda6 = new PdfPCell(new Phrase("Direccion", font));
                    celda6.BackgroundColor = new BaseColor(240, 240, 240);
                    celda6.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    tabla.AddCell(celda6);

                    PdfPCell celda7 = new PdfPCell(new Phrase("Telefono", font));
                    celda7.BackgroundColor = new BaseColor(240, 240, 240);
                    celda7.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    tabla.AddCell(celda7);

                    PdfPCell celda8 = new PdfPCell(new Phrase("Celular", font));
                    celda8.BackgroundColor = new BaseColor(240, 240, 240);
                    celda8.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    tabla.AddCell(celda8);

                    PdfPCell celda9 = new PdfPCell(new Phrase("Observaciones", font));
                    celda9.BackgroundColor = new BaseColor(240, 240, 240);
                    celda9.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    tabla.AddCell(celda9);


                    ////Poniendo datos en la la tabla
                    //List<encuesta_usuariosCLS> listaUser = (List<encuesta_usuariosCLS>)Session["ListaUser"];
                    int nroregistros = listaEmpleado.Count();
                    for (int i = 0; i < nroregistros; i++)
                    {
                        tabla.AddCell(new PdfPCell(new Phrase(listaEmpleado[i].matricula.ToString(), font)));
                        tabla.AddCell(new PdfPCell(new Phrase(listaEmpleado[i].nombre_completo.ToString(), font)));
                        if (listaEmpleado[i].perfil_desc != null)
                        {
                            tabla.AddCell(new PdfPCell(new Phrase(listaEmpleado[i].perfil_desc.ToString(), font)));
                        }
                        else {
                            tabla.AddCell(new PdfPCell(new Phrase("", font)));
                        }
                        tabla.AddCell(new PdfPCell(new Phrase(listaEmpleado[i].actividad_desc.ToString(), font)));
                        tabla.AddCell(new PdfPCell(new Phrase(listaEmpleado[i].fecho_ingreso.ToString(), font))).HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        tabla.AddCell(new PdfPCell(new Phrase(listaEmpleado[i].direccion.ToString(), font)));
                        tabla.AddCell(new PdfPCell(new Phrase(listaEmpleado[i].telefono.ToString(), font))).HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        if (listaEmpleado[i].celular != null)
                        {
                            tabla.AddCell(new PdfPCell(new Phrase(listaEmpleado[i].celular.ToString(), font))).HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        }
                        else {
                            tabla.AddCell(new PdfPCell(new Phrase("", font))).HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        }
                        if (listaEmpleado[i].observaciones!=null) {
                            tabla.AddCell(new PdfPCell(new Phrase(listaEmpleado[i].observaciones.ToString(), font)));
                        }
                        else {
                            tabla.AddCell(new PdfPCell(new Phrase("", font)));
                        }
                        
                        //tabla.AddCell(new PdfPCell(new Phrase(listaEmpleado[i].empleado_tipopersonal.ToString(), font)));
                        //tabla.AddCell(new PdfPCell(new Phrase(listaEmpleado[i].usua_presento, font))).HorizontalAlignment = PdfPCell.ALIGN_CENTER;


                    }
                    ////Agregando la tabla al documento
                    doc.Add(tabla);
                    doc.Close();
                    buffer = stream.ToArray();
                    var contentLength = buffer.Length;

                    var statuscode = HttpStatusCode.OK;
                    response = Request.CreateResponse(statuscode);
                    response.Content = new StreamContent(new MemoryStream(buffer));
                    response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
                    response.Content.Headers.ContentLength = contentLength;
                    ContentDispositionHeaderValue contentDisposition = null;
                    if (ContentDispositionHeaderValue.TryParse("inline; filename=Reporte.pdf", out contentDisposition))
                    {
                        response.Content.Headers.ContentDisposition = contentDisposition;
                    }
                    else
                    {
                        //var statuscode = HttpStatusCode.NotFound;
                        // var message = String.Format("Unable to find file. file \"{0}\" may not exist.");
                        // var responseData = responseDataFactory.CreateWithOnlyMetadata(statuscode, message);
                        response = Request.CreateResponse((HttpStatusCode.NotFound));
                    }
                }


                return response;


            }


        }
    }
}
