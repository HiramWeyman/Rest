using Rest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;
using System.IO;
using System.Net.Http.Headers;

namespace Rest.Controllers
{
    public class ExcelController : ApiController
    {
        [Route("api/SolExcel/{id}")]
        [HttpGet]
        public HttpResponseMessage CreateLieferschein(int id)
        {

            
            using (steujedo_sindicatoEntities db = new steujedo_sindicatoEntities())
            {
                List<concurso_antig> listaEmpleado = null;
                db.Configuration.LazyLoadingEnabled = false;
                //string query = "  SELECT  pad_id,pad_plaza_id,pad_mat,pad_nombre,pad_adscripcion,pad_categoria,pad_sueldo,pad_funcion,pad_situacion,pad_permanencia,pad_f_ingreso,pad_permisos,pad_f_antig,pad_n_insaluble,pad_adscrip_base,pad_catego_base,pad_funcion_base,pad_situacion_base,pad_num_contacto,pad_observaciones,pad_string_fec,pad_cancelado,SUBSTRING(pad_f_antig,7, 4) as anio,catp_id,catp_descrip,catp_status,catp_u_captura,catp_f_captura,catp_categoria,catp_funcion,catp_adscripcion FROM steujedo_sindicato.steujedo_sindicato.Concurso_Plazas,steujedo_sindicato.steujedo_sindicato.Cat_Plazas where pad_plaza_id=catp_id and pad_cancelado IS NULL and pad_plaza_id=" + id + "  order by SUBSTRING(pad_f_antig,7, 4), pad_sueldo ";
                string query = " select * from concurso_antig where pad_plaza_id=" + id + " order by pad_fec_date,pad_sueldo ";
                listaEmpleado = db.Database.SqlQuery<concurso_antig>(query).ToList();

                //String ruta_img = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "imagenes\\logoSteujed.png");
                //iTextSharp.text.Font font1 = new Font(Font.FontFamily.HELVETICA, 12, Font.NORMAL);
                //iTextSharp.text.Font font2 = new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD);
                //iTextSharp.text.Font font = new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL);
                //Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 42, 35);
                byte[] buffer;
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.BadRequest);
                using (MemoryStream stream = new MemoryStream())
                {
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelPackage ep = new ExcelPackage();

                    //Crear una hoja
                    ep.Workbook.Worksheets.Add("Reporte de Solicitudes");
                    ExcelWorksheet ew = ep.Workbook.Worksheets[0];
    
                    //Ponemos nombres de las columnas
                    ew.Cells[1, 1].Value = "ID";
                    ew.Cells[1, 2].Value = "MATRICULA";
                    ew.Cells[1, 3].Value = "NOMBRE";
                    ew.Cells[1, 4].Value = "ADSCRIPCIÓN";
                    ew.Cells[1, 5].Value = "CATEGORIA";
                    ew.Cells[1, 6].Value = "FUNCIÓN";
                    ew.Cells[1, 7].Value = "SITUACIÓN";
                    ew.Cells[1, 8].Value = "PERMANENCIA";
                    ew.Cells[1, 9].Value = "F.INGRESO";
                    ew.Cells[1, 10].Value = "SUELDO";
                    ew.Cells[1, 11].Value = "PERMISOS";
                    ew.Cells[1, 12].Value = "ANTIGUEDAD";
                    ew.Cells[1, 13].Value = "INSALUBLE";
                    ew.Cells[1, 14].Value = "ADSCRIPCIÓN_BASE";
                    ew.Cells[1, 15].Value = "CAT_BASE";
                    ew.Cells[1, 16].Value = "FUNCIÓN_BASE";
                    ew.Cells[1, 17].Value = "SITUACIÓN_BASE";
                    ew.Cells[1, 18].Value = "NUM_CONTACTO";
                    ew.Cells[1, 19].Value = "OBSERVACIONES";
                    ew.Cells[1, 20].Value = "ESTATUS";
                    ew.Cells[1, 21].Value = "COMENTARIOS";


                    ew.Column(1).Width = 10;
                    ew.Column(2).Width = 15;
                    ew.Column(3).Width = 50;
                    ew.Column(4).Width = 50;
                    ew.Column(5).Width = 50;
                    ew.Column(6).Width = 40;
                    ew.Column(7).Width = 30;
                    ew.Column(8).Width = 30;
                    ew.Column(9).Width = 15;
                    ew.Column(10).Width = 10;
                    ew.Column(11).Width = 10;
                    ew.Column(12).Width = 15;
                    ew.Column(13).Width = 20;
                    ew.Column(14).Width = 40;
                    ew.Column(15).Width = 50;
                    ew.Column(16).Width = 30;
                    ew.Column(17).Width = 20;
                    ew.Column(18).Width = 18;
                    ew.Column(19).Width = 80;
                    ew.Column(20).Width = 15;
                    ew.Column(21).Width = 40;

                    using (var range = ew.Cells[1, 1, 1, 21])
                    {
                        range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        range.Style.Font.Color.SetColor(Color.White);
                        range.Style.Fill.BackgroundColor.SetColor(Color.DarkRed);
                    }
                    //ew.Cells[i + 2, 7].Style.Numberformat.Format = "yyyy-mm-dd";
                    //ew.Cells[i + 2, 7].Value = listaEmpleado[i].usua_f_alta;
                    int nroregistros = listaEmpleado.Count();
                    for (int i = 0; i < nroregistros; i++)
                    {
                        ew.Cells[i + 2, 1].Value = listaEmpleado[i].pad_id;
                        ew.Cells[i + 2, 2].Value = listaEmpleado[i].pad_mat;
                        ew.Cells[i + 2, 3].Value = listaEmpleado[i].pad_nombre;
                        ew.Cells[i + 2, 4].Value = listaEmpleado[i].pad_adscripcion;
                        ew.Cells[i + 2, 5].Value = listaEmpleado[i].pad_categoria;
                        ew.Cells[i + 2, 6].Value = listaEmpleado[i].pad_funcion;
                        ew.Cells[i + 2, 7].Value = listaEmpleado[i].pad_situacion;
                        ew.Cells[i + 2, 8].Value = listaEmpleado[i].pad_permanencia;
                        ew.Cells[i + 2, 9].Style.Numberformat.Format = "yyyy-mm-dd";
                        ew.Cells[i + 2, 9].Value = listaEmpleado[i].pad_f_ingreso;
                        ew.Cells[i + 2, 10].Value = listaEmpleado[i].pad_sueldo;
                        ew.Cells[i + 2, 11].Value = listaEmpleado[i].pad_permisos;
                        ew.Cells[i + 2, 12].Style.Numberformat.Format = "yyyy-mm-dd";
                        ew.Cells[i + 2, 12].Value = listaEmpleado[i].pad_f_antig;
                        ew.Cells[i + 2, 13].Value = listaEmpleado[i].pad_n_insaluble;
                        ew.Cells[i + 2, 14].Value = listaEmpleado[i].pad_adscrip_base;
                        ew.Cells[i + 2, 15].Value = listaEmpleado[i].pad_catego_base;
                        ew.Cells[i + 2, 16].Value = listaEmpleado[i].pad_funcion_base;
                        ew.Cells[i + 2, 17].Value = listaEmpleado[i].pad_situacion_base;
                        ew.Cells[i + 2, 18].Value = listaEmpleado[i].pad_num_contacto;
                        ew.Cells[i + 2, 19].Style.WrapText = true;
                        ew.Cells[i + 2, 19].Value = listaEmpleado[i].pad_observaciones;
                        ew.Cells[i + 2, 20].Value = listaEmpleado[i].pla_desc_corta;
                        ew.Cells[i + 2, 21].Style.WrapText = true;
                        ew.Cells[i + 2, 21].Value = listaEmpleado[i].pad_comentarios;

                    }
                    ep.SaveAs(stream);
                    buffer = stream.ToArray();
                    var contentLength = buffer.Length;

                    var statuscode = HttpStatusCode.OK;
                    response = Request.CreateResponse(statuscode);
                    response.Content = new StreamContent(new MemoryStream(buffer));
                    response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                    response.Content.Headers.ContentLength = contentLength;
                    ContentDispositionHeaderValue contentDisposition = null;
                    if (ContentDispositionHeaderValue.TryParse("inline; filename=ReporteSolicitudes.xlsx", out contentDisposition))
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
