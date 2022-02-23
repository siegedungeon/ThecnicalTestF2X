using System;
using System.IO;
using System.Threading.Tasks;
using ApiBackendF2X.Utility;
using DinkToPdf;
using DinkToPdf.Contracts;
using Domain.Attributes;
using Domain.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace ApiBackendF2X.Controllers
{
    [OpenApiTag("Values",
        Description = "Controlador para obtencion de datos para el UI",
        DocumentationDescription = "Cada endpoint cuenta con su documentacion de entrada y salida")]
    [ApiKey]
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private IConverter _converter;
        private readonly IValuesService _Service;
        public ValuesController(IValuesService service, IConverter converter)
        {
            _Service = service;
            _converter = converter;
        }


        // GET: api/Conteo/{FechaConsultada}
        /// <summary>
        /// Obtiene resultados a partir de una fecha consultada
        /// </summary>
        /// <remarks>
        /// Es Obligatorio mantener el formato de la fecha yyyy-MM-dd
        /// </remarks>
        /// <param name="FechaConsultada">FechaConsultada string</param>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>               
        [HttpGet]
        [Route("Conteo/{FechaConsultada}")]
        public async Task<IActionResult> GetConteo(string FechaConsultada)
        {
            try
            {
                var result = await _Service.GetVehiculesCounting(FechaConsultada);
                return Ok(result);

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET: api/Recaudo/{FechaConsultada}
        /// <summary>
        /// Obtiene resultados a partir de una fecha consultada
        /// </summary>
        /// <remarks>
        /// Es Obligatorio mantener el formato de la fecha yyyy-MM-dd
        /// </remarks>
        /// <param name="FechaConsultada">FechaConsultada string</param>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>           
        [HttpGet]
        [Route("Recaudo/{FechaConsultada}")]
        public async Task<IActionResult> GetRecaudo(string FechaConsultada)
        {
            try
            {
                var result = await _Service.GetVehiculesCollection(FechaConsultada);
                return Ok(result);

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        // GET: api/Reporte/
        /// <summary>
        /// Genera Reporte de recaudo del mes hasta un dia antes de la peticion
        /// </summary>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>           
        [HttpGet]
        [Route("Reporte/")]
        public async Task<IActionResult> Reporte()
        {
            try
            {
                var result = await _Service.GetReporteMes();
                var globalSettings = new GlobalSettings
                {
                    ColorMode = ColorMode.Color,
                    Orientation = Orientation.Portrait,
                    PaperSize = PaperKind.A4,
                    Margins = new MarginSettings { Top = 10 },
                    DocumentTitle = "PDF Report",
                    Out = @"C:\PDF\Report.pdf"
                };
                var objectSettings = new ObjectSettings
                {
                    PagesCount = true,
                    HtmlContent = TemplateGenerator.GetHTMLString(result),
                    WebSettings = { DefaultEncoding = "utf-8" },
                    HeaderSettings = { FontName = "Arial", FontSize = 9, Right = "Page [page] of [toPage]", Line = true },
                    FooterSettings = { FontName = "Arial", FontSize = 9, Line = true, Center = "Report Footer" }
                };
                var pdf = new HtmlToPdfDocument()
                {
                    GlobalSettings = globalSettings,
                    Objects = { objectSettings }
                };

                var file = _converter.Convert(pdf);
                return File(file, "application/pdf");

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
