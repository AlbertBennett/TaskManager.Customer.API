using TaskManager.Customer.API.Models.DTO;
using TaskManager.Customer.API.Repositories.Abstractions;

namespace TaskManager.Customer.API.Repositories
{
    public sealed class CustomerRepository(ILogger<CustomerRepository> logger) : ICustomerRepository
    {
        private readonly ILogger<CustomerRepository> _logger = logger;

        List<Models.DAO.Customer> models = new List<Models.DAO.Customer>();

        public async Task<bool> DeleteCustomerAsync(int id)
        {
            var customer = await GetCustomerByIdAsync(id);

            if (customer == null)
            {
                _logger.LogError($"Error in deleting a customer. No customer found with  ID: {id}");

                return false;
            }

            try
            {
                models.Remove(customer);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in deleting a customer {ex.Message}");
            }

            return false;
        }

        public async Task<Models.DAO.Customer?> GetCustomerByEmailAsync(string emailAddress) => 
            models.FirstOrDefault(x => x.Email.ToLower().Equals(emailAddress.ToLower()));

        public async Task<Models.DAO.Customer?> GetCustomerByIdAsync(int id) => models.FirstOrDefault(x => x.Id.Equals(id));

        public async Task<bool> RegisterNewCustomerAsync(CustomerRequestBody customerRequest)
        {
            if(await GetCustomerByEmailAsync(customerRequest.Email) != null)
            {
                return false;
            }

            try
            {
                models.Add(new Models.DAO.Customer
                {
                    Id = models.Count(), // will be removed later on
                    FirstName = customerRequest.FirstName,
                    LastName = customerRequest.LastName,
                    Email = customerRequest.Email,
                    DisplayName = customerRequest.DisplayName
                });

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in registering a new customer {ex.Message}");
            }

            return false;
        }

        public async Task<bool> UpdateCustomerAsync(CustomerResponse customerResponse)
        {
            var customer = await GetCustomerByIdAsync(customerResponse.Id);
            bool hasChanged = false;

            if (customer != null) 
            {
                if (customerResponse.FirstName != null || !customerResponse.FirstName.Equals(string.Empty))
                {
                    if (customerResponse.FirstName != customer.FirstName)
                    {
                        hasChanged = true;
                        customer.FirstName = customerResponse.FirstName;
                    }
                }

                if (customerResponse.LastName != null || !customerResponse.LastName.Equals(string.Empty))
                {
                    if (customerResponse.LastName != customer.LastName)
                    {
                        hasChanged = true;
                        customer.LastName = customerResponse.LastName;
                    }
                }

                if (customerResponse.DisplayName != null || !customerResponse.DisplayName.Equals(string.Empty))
                {
                    if (customerResponse.DisplayName != customer.DisplayName)
                    {
                        hasChanged = true;
                        customer.DisplayName = customerResponse.DisplayName;
                    }
                }

                if (customerResponse.Email != null || !customerResponse.Email.Equals(string.Empty))
                {
                    if (customerResponse.Email != customer.Email)
                    {
                        hasChanged = true;
                        customer.Email = customerResponse.Email;
                    }
                }

                if (hasChanged)
                {
                    bool result = await DeleteCustomerAsync(customerResponse.Id);

                    if (result)
                    {
                        models.Add(customer);
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            return hasChanged;
        }
    }
}
