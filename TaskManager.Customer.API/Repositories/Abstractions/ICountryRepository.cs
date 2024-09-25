using TaskManager.Customer.API.Models;

namespace TaskManager.Customer.API.Repositories.Abstractions
{
    public interface ICountryRepository
    {
        Task<Country?> GetCountryByCountryCodeAsync(string countryCode);
        Task<Country?> GetCountryByIDAsync(int id);
        Task<Country?> GetCountryByNameAsync(string name);
    }
}
