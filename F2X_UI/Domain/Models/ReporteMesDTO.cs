using System;
using System.Collections.Generic;

namespace Domain.Models
{
    public class ReporteMesDTO
    {
        public DateTime Fecha { get; set; }
        public List<EstacionValuesDTO> Estaciones { get; set; }
    }

    public class EstacionReporte
    {
        public long TotalCantidad { get; set; }
        public long TotalValor { get; set; }
    }

    public class EstacionValuesDTO
    {
        public long Cantidad { get; set; }
        public long Valor { get; set; }
        public string Estacion { get; set; }
        public DateTime Fecha { get; set; }
    }
}
