using RoyalResidence.DTO;

namespace RoyalResidenceWeb.Services.IServices
{
    public interface IResidenceService
    {
        Task<T?> GetAllAsync<T>();
        Task<T?> GetAsync<T>(int id);
        Task<T?> CreateAsync<T>(ResidenceCreateDTO dto);
        Task<T?> UpdateAsync<T>(ResidenceUpdateDTO dto);
        Task<T?> DeleteAsync<T>(int id);
    }
}
