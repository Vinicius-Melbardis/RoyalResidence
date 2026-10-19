using RoyalResidence.DTO;
using RoyalResidenceWeb.Models;

namespace RoyalResidenceWeb.Services.IServices
{
    public interface IBaseService
    {
        ApiResponse<object> ResponseModel { get; set; }
        Task<T> SendAsync<T>(ApiRequest apiRequest);
    }
}
