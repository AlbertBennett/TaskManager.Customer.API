using TaskManager.Customer.API.Models;

namespace TaskManager.Customer.API.Repositories.Abstractions
{
    public interface ICountryRepository
    {
        Task<Country?> GetCountryByCountryCode(string countryCode);
        Task<Country?> GetCountryByID(int id);
    }
}
