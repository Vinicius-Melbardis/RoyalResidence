using RoyalResidence.DTO;
using RoyalResidenceWeb.Models;
using RoyalResidenceWeb.Services.IServices;

namespace RoyalResidenceWeb.Services
{
    public class ResidenceService : BaseService, IResidenceService
    {
        private const string APIEndpoint = "/api/residence";
        public ResidenceService(IHttpClientFactory httpClient, IConfiguration configuration) : base(httpClient)
        {
        }

        public Task<T?> CreateAsync<T>(ResidenceCreateDTO dto, string token)
        {
            return SendAsync<T>(new ApiRequest
            {
                ApiType = SD.ApiType.POST,
                Data = dto,
                Url = APIEndpoint,
                Token = token
            });
        }

        public Task<T?> DeleteAsync<T>(int id, string token)
        {
            return SendAsync<T>(new ApiRequest
            {
                ApiType = SD.ApiType.DELETE,
                Url = $"{APIEndpoint}/{id}",
                Token = token
            });
        }

        public Task<T?> GetAllAsync<T>(string token)
        {
            return SendAsync<T>(new ApiRequest
            {
                ApiType = SD.ApiType.GET,
                Url = APIEndpoint,
                Token = token
            });
        }

        public Task<T?> GetAsync<T>(int id, string token)
        {
            return SendAsync<T>(new ApiRequest
            {
                ApiType = SD.ApiType.GET,
                Url = $"{APIEndpoint}/{id}",
                Token = token
            });
        }

        public Task<T?> UpdateAsync<T>(ResidenceUpdateDTO dto, string token)
        {
            return SendAsync<T>(new ApiRequest
            {
                ApiType = SD.ApiType.PUT,
                Data = dto,
                Url = $"{APIEndpoint}/{dto.Id}",
                Token = token
            });

        }
    }
}
