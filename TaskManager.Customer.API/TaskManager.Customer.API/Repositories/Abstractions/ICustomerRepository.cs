using TaskManager.Customer.API.Models.DTO;

namespace TaskManager.Customer.API.Repositories.Abstractions
{
    public interface ICustomerRepository
    {
        Task<Models.DAO.Customer?> GetCustomerByIdAsync(int id);
        Task<Models.DAO.Customer?> GetCustomerByEmailAsync(string emailAddress);
        Task<bool> RegisterNewCustomerAsync(CustomerRequestBody customerRequest);
        Task<bool> UpdateCustomerAsync(CustomerResponse customerResponse);
        Task<bool> DeleteCustomerAsync(int id);
    }
}
