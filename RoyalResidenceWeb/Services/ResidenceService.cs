using RoyalResidence.DTO;
using RoyalResidenceWeb.Models;
using RoyalResidenceWeb.Services.IServices;

namespace RoyalResidenceWeb.Services
{
    public class ResidenceService : BaseService, IResidenceService
    {
        private const string APIEndpoint = "/api/residence";
        public ResidenceService(IHttpClientFactory httpClient, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
            : base(httpClient, httpContextAccessor)
        {
        }

        public Task<T?> CreateAsync<T>(ResidenceCreateDTO dto)
        {
            return SendAsync<T>(new ApiRequest
            {
                ApiType = SD.ApiType.POST,
                Data = dto,
                Url = APIEndpoint,
            });
        }

        public Task<T?> DeleteAsync<T>(int id)
        {
            return SendAsync<T>(new ApiRequest
            {
                ApiType = SD.ApiType.DELETE,
                Url = $"{APIEndpoint}/{id}",
            });
        }

        public Task<T?> GetAllAsync<T>()
        {
            return SendAsync<T>(new ApiRequest
            {
                ApiType = SD.ApiType.GET,
                Url = APIEndpoint,
            });
        }

        public Task<T?> GetAsync<T>(int id)
        {
            return SendAsync<T>(new ApiRequest
            {
                ApiType = SD.ApiType.GET,
                Url = $"{APIEndpoint}/{id}",
            });
        }

        public Task<T?> UpdateAsync<T>(ResidenceUpdateDTO dto)
        {
            return SendAsync<T>(new ApiRequest
            {
                ApiType = SD.ApiType.PUT,
                Data = dto,
                Url = $"{APIEndpoint}/{dto.Id}",
            });

        }
    }
}
