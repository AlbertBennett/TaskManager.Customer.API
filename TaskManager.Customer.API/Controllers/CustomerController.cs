using Microsoft.AspNetCore.Mvc;
using TaskManager.Customer.API.DataTransferObjects;
using TaskManager.Customer.API.Models;
using TaskManager.Customer.API.Repositories.Abstractions;

namespace TaskManager.Customer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public sealed class CustomerController(ICustomerRepository customerRepository,
        ICountryRepository countryRepository, ILogger<CustomerController> logger) : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository = customerRepository;
        private readonly ICountryRepository _countryRepository = countryRepository;

        private readonly ILogger<CustomerController> _logger = logger;

        [HttpPost]
        public async Task<IActionResult> RegisterNewCustomer([FromBody] CustomerDTO customerDto)
        {
            if (await _customerRepository.CustomerExistsWithEmail(customerDto.Email))
            {
                return BadRequest("A customer with that email address already exists");
            }

            Country? country = await _countryRepository.GetCountryByCountryCode(customerDto.CountryCode);

            if (country == null)
            {
                return BadRequest($"A country with the code: {customerDto.CountryCode} does not exist");
            }

            bool result = await _customerRepository.RegisterNewCustomer(
                customerDto.FirstName, customerDto.LastName, customerDto.Email, country);

            return result ? Ok() : BadRequest("Unable to register the new customer");
        }

        [HttpGet]
        public async Task<IActionResult> Get(int id)
        {
            Models.Customer? customer = await _customerRepository.GetCustomerById(id);

            return customer == null ? BadRequest($"A customer with id: {id} does not exist") : Ok(customer);
        }
    }
}
