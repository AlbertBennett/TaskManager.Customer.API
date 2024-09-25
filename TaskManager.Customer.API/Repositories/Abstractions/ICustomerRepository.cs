using TaskManager.Customer.API.Models;

namespace TaskManager.Customer.API.Repositories.Abstractions
{
    public interface ICustomerRepository
    {
        Task<Models.Customer?> GetCustomerByIdAsync(int id);

        Task<bool> CustomerExistsWithEmailAsync(string emailAddress);

        Task<bool> RegisterNewCustomerAsync(string firstName, string lastName, string email, Country country);

        Task<bool> DeleteCustomerAsync(int id);
    }
}
