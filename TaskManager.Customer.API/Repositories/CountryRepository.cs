using Microsoft.EntityFrameworkCore;
using TaskManager.Customer.API.Models;
using TaskManager.Customer.API.Repositories.Abstractions;

namespace TaskManager.Customer.API.Repositories
{
    public sealed class CountryRepository(CustomerDBContext customerDBContext) : ICountryRepository
    {
        private readonly CustomerDBContext _customerDBContext = customerDBContext;

        public async Task<Country?> GetCountryByIDAsync(int id)
        {
            return await _customerDBContext.Countries.FirstOrDefaultAsync(c => c.ID.Equals(id));
        }

        public async Task<Country?> GetCountryByCountryCodeAsync(string countryCode)
        {
            return await _customerDBContext.Countries.FirstOrDefaultAsync(c => c.ISOCode.Equals(countryCode));
        }

        public async Task<Country?> GetCountryByNameAsync(string name)
        {
            return await _customerDBContext.Countries.FirstOrDefaultAsync(c => c.Name.Equals(name));
        }
    }
}
