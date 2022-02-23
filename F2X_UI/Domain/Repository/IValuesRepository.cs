using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Models;

namespace Domain.Repository
{
    public interface IValuesRepository
    {
        Task<List<ConteoResponseDTO>> GetVehiculesCounting(string fecha_consulta);
        Task<List<RecaudoResponseDTO>> GetVehiculesCollection(string fecha_consulta);
        Task<List<ReporteMesDTO>> GetReporteMes();
    }
}
