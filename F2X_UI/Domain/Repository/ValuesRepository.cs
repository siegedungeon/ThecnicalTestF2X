using Domain.Models;
using IdentityModel.Client;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Domain.Repository
{
    public class ValuesRepository : IValuesRepository
    {
        private readonly DbContextDiego _context;
        private readonly IConfiguration _configuration;
        private string RouteApi = "";
        private readonly JsonSerializerOptions _options;
        private readonly HttpClient _httpClient;

        private static readonly Object _lock = new Object();
        private IMemoryCache _cache;

        public ValuesRepository(IConfiguration configuration,
            IHttpClientFactory clientFactory,
            ILoggerFactory loggerFactory,
            IMemoryCache memoryCache,
            DbContextDiego contextDiego)
        {
            _configuration = configuration;
            _httpClient = clientFactory.CreateClient();
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            RouteApi = _configuration.GetSection("AppSettings:repositoryUrl").Value;
            _cache = memoryCache;
            _context= contextDiego;
        }

        public async Task<List<ConteoResponseDTO>> GetVehiculesCounting(string fecha_consulta)
        {
            try
            {
                var fecha= ParseDateTime(fecha_consulta);

                var fromDatabase = _context.TBL_Conteo.Where(a => a.fechaConsultada == fecha).ToList();

                if (fromDatabase.Count > 0)
                {
                    return fromDatabase;
                }

                var resultsFromService = await ConteoResponseFromService(fecha_consulta);

                _context.TBL_Conteo.AddRange(resultsFromService);
                await _context.SaveChangesAsync();
                return resultsFromService;
            }
            catch (Exception e)
            {
                throw new ApplicationException($"Exception {e}");
            }
        }

        private static DateTime ParseDateTime(string fecha_consulta)
        {
            string expectedFormat = "yyyy-MM-dd";
            DateTime theDate;
            bool result = DateTime.TryParseExact(
                fecha_consulta,
                expectedFormat,
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None,
                out theDate);
            if (!result)
            {
                throw new ApplicationException($"Exception Fecha no tiene el formato correcto");
            }

            return theDate;
        }


        public async Task<List<RecaudoResponseDTO>> GetVehiculesCollection(string fecha_consulta)
        {
            try
            {
                var fecha = ParseDateTime(fecha_consulta);

                var fromDatabase = _context.TBL_Recaudo.Where(a => a.fechaConsultada == fecha).ToList();

                if (fromDatabase.Count > 0)
                {
                    return fromDatabase;
                }

                var resultsFromService = await RecaudoResponseDtosFromService(fecha_consulta);

                _context.TBL_Recaudo.AddRange(resultsFromService);
                await _context.SaveChangesAsync();
                return resultsFromService;
                
            }
            catch (Exception e)
            {
                throw new ApplicationException($"Exception {e}");
            }
        }

      

        public async Task<string> GetApiToken(string api_name, string secret)
        {
            var accessToken = GetFromCache();

            if (accessToken != null)
            {
                if (accessToken.expiration > DateTime.UtcNow)
                {
                    return accessToken.token;
                }
            }
            
            var newAccessToken = await getApiToken(api_name, secret);
            AddToCache(newAccessToken);

            return newAccessToken.token;
        }

        private async Task<AccessTokenItem> getApiToken(string api_name, string secret)
        {
            try
            {
                var request = new LoginRequestDTO()
                {
                    password = secret,
                    userName = api_name
                };
                var json = JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"{RouteApi}Login", content);

                var responseBody = await response.Content.ReadAsStringAsync();
                var returnedObj = JsonSerializer.Deserialize<AccessTokenItem>(responseBody);

                return returnedObj;

            }
            catch (Exception e)
            {
                throw new ApplicationException($"Exception {e}");
            }
        }

        private void AddToCache( AccessTokenItem accessTokenItem)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromHours(1));

            lock (_lock)
            {
                _cache.Set("ApiCache", JsonSerializer.Serialize(accessTokenItem), cacheEntryOptions);
            }
        }

        private AccessTokenItem GetFromCache()
        {
            string cacheEntry;
            
            if (!_cache.TryGetValue("ApiCache", out cacheEntry))
            {
                return null;
            }

            return JsonSerializer.Deserialize<AccessTokenItem>(cacheEntry);
        }
        private async Task<List<ConteoResponseDTO>> ConteoResponseFromService(string fecha_consulta)
        {
            _httpClient.BaseAddress = new Uri(RouteApi);

            var access_token = await GetApiToken(
                _configuration.GetSection("user").Value,
                _configuration.GetSection("password").Value
            );

            _httpClient.SetBearerToken(access_token);

            var response = await _httpClient.GetAsync($"ConteoVehiculos/{fecha_consulta}"); //YYYY-MM-DD
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                try
                {
                    var data = JsonSerializer.Deserialize<List<ConteoResponseDTO>>(responseContent);
                    return data;
                }
                catch (Exception)
                {
                    return new List<ConteoResponseDTO>();
                }
            }

            throw new ApplicationException($"Status code: {response.StatusCode}, Error: {response.ReasonPhrase}");
        }
        private async Task<List<RecaudoResponseDTO>> RecaudoResponseDtosFromService(string fecha_consulta)
        {
            _httpClient.BaseAddress = new Uri(RouteApi);

            var access_token = await GetApiToken(
                _configuration.GetSection("user").Value,
                _configuration.GetSection("password").Value
            );

            _httpClient.SetBearerToken(access_token);

            var response = await _httpClient.GetAsync($"RecaudoVehiculos/{fecha_consulta}");
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                try
                {
                    var data = JsonSerializer.Deserialize<List<RecaudoResponseDTO>>(responseContent);
                    return data;
                }
                catch (Exception)
                {
                    return new List<RecaudoResponseDTO>();
                }
            }

            throw new ApplicationException($"Status code: {response.StatusCode}, Error: {response.ReasonPhrase}");
        }


        public async Task<List<ReporteMesDTO>> GetReporteMes()
        {
            var aRetornar = new List<ReporteMesDTO>();

            var fecha = DateTime.UtcNow;
            var firstDayOfMonth = new DateTime(fecha.Year, fecha.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            var cosulta = from r in _context.TBL_Recaudo
                         join c in _context.TBL_Conteo 
                             on r.estacion equals c.estacion into g
                from d in g.DefaultIfEmpty()
                orderby d.fechaConsultada.Year, d.fechaConsultada.Month
                select new EstacionValuesDTO()
                {
                    Cantidad = d.cantidad,
                    Valor = d.cantidad,
                    Fecha = d.fechaConsultada,
                    Estacion = d.estacion
                };

            if (cosulta.ToList() != null)
            {
                var response = cosulta.GroupBy(a => a.Fecha);

                

                foreach (var item in response)
                {
                    aRetornar.Add(new ReporteMesDTO()
                    {
                        Fecha = item.Key,
                        Estaciones = item.ToList()
                    });
                }
            }

            return aRetornar;
        }
    }
}
