using System;
using System.Threading.Tasks;
using Domain.Attributes;
using Domain.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace ApiBackendF2X.Controllers
{
    [SwaggerTag("Values",
        Description = "Controlador para obtencion de datos para el UI",
        DocumentationDescription = "Cada endpoint cuenta con su documentacion de entrada y salida")]
    [ApiKey]
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IValuesService _Service;
        public ValuesController(IValuesService service)
        {
            _Service = service;
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
    }
}
