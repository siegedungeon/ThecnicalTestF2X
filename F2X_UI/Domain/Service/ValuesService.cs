using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Models;
using Domain.Repository;

namespace Domain.Service
{
    public class ValuesService : IValuesService
    {
        private readonly IValuesRepository _valueRepository;
        public ValuesService(IValuesRepository valueRepository)
        {
            _valueRepository = valueRepository;
        }

        public async Task<List<ReporteMesDTO>> GetReporteMes()
        {
            return await _valueRepository.GetReporteMes();
        }

        public async Task<List<RecaudoResponseDTO>> GetVehiculesCollection(string fecha_consulta)
        {
            return await _valueRepository.GetVehiculesCollection(fecha_consulta);
        }

        public async Task<List<ConteoResponseDTO>> GetVehiculesCounting(string fecha_consulta)
        {
            return await _valueRepository.GetVehiculesCounting(fecha_consulta);
        }
    }
}
