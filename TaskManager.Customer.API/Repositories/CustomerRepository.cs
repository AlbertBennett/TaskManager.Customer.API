using Microsoft.EntityFrameworkCore;
using TaskManager.Customer.API.Models;
using TaskManager.Customer.API.Repositories.Abstractions;

namespace TaskManager.Customer.API.Repositories
{
    public sealed class CustomerRepository(CustomerDBContext customerDBContext,
        ILogger<CustomerRepository> logger) : ICustomerRepository
    {
        private readonly CustomerDBContext _customerDBContext = customerDBContext;

        private readonly ILogger<CustomerRepository> _logger = logger;

        public async Task<Models.Customer?> GetCustomerById(int id)
        {
            return await _customerDBContext.Customer.FirstOrDefaultAsync(x => x.ID.Equals(id));
        }

        public async Task<bool> CustomerExistsWithEmail(string emailAddress)
        {
            return await _customerDBContext.Customer.FirstOrDefaultAsync(x => x.Email.ToLower().Equals(emailAddress.ToLower())) != null;
        }

        public async Task<bool> RegisterNewCustomer(string firstName, string lastName, string email, Country country)
        {
            if (await _customerDBContext.Customer.AnyAsync(x => x.Email.ToLower().Equals(email.ToLower())))
            {
                return false;
            }

            try
            {
                await _customerDBContext.Customer.AddAsync(new Models.Customer
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    CountryCodeId = country.ID,
                    Country = country
                });

                await _customerDBContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in registering a new customer {ex.Message}");
            }

            return false;
        }

        public async Task<bool> DeleteCustomer(int id)
        {
            if (await _customerDBContext.Customer.AnyAsync(x => x.ID.Equals(id)))
            {
                return false;
            }

            try
            {
                var customer = await GetCustomerById(id);

                if (customer == null)
                {
                    _logger.LogError($"Error in deleting a customer. No customer found with  ID: {id}");

                    return false;
                }

                _customerDBContext.Customer.Remove(customer);

                await _customerDBContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in deleting a customer {ex.Message}");
            }

            return false;
        }
    }
}
