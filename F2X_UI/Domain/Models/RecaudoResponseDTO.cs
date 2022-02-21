using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
    public class RecaudoResponseDTO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long id { get; set; }
        public string estacion { get; set; }
        public string sentido { get; set; }
        public string hora { get; set; }
        public string categoria { get; set; }
        public long valorTabulado { get; set; }
        public DateTime fechaConsultada { get; set; }
    }
}