using TaskManager.Customer.API.Models;

namespace TaskManager.Customer.API.Repositories.Abstractions
{
    public interface ICustomerRepository
    {
        Task<Models.Customer?> GetCustomerById(int id);

        Task<bool> CustomerExistsWithEmail(string emailAddress);

        Task<bool> RegisterNewCustomer(string firstName, string lastName, string email, Country country);

        Task<bool> DeleteCustomer(int id);
    }
}
