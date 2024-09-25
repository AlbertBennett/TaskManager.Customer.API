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
            if (customerDto == null) {
                return BadRequest("You need to provide the customers FirstName, LatsName, Email and CountryCode in the request body to register them");
            }

            if (customerDto.FirstName == null || customerDto.FirstName.Equals(string.Empty))
            {
                return BadRequest("You need to provide the customer FirstName in the request body for them to be registered on the system");
            }

            if (customerDto.LastName == null || customerDto.LastName.Equals(string.Empty))
            {
                return BadRequest("You need to provide the customer LastName in the request body for them to be registered on the system");
            }

            if (customerDto.CountryCode == null || customerDto.CountryCode.Equals(string.Empty))
            {
                return BadRequest("You need to provide the customer CountryCode (IE, UK, etc) address in the request body for them to be registered on the system");
            }

            if (customerDto.Email == null || customerDto.Email.Equals(string.Empty))
            {
                return BadRequest("You need to provide the customer Email address in the request body for them to be registered on the system");
            }

            if (await _customerRepository.CustomerExistsWithEmailAsync(customerDto.Email))
            {
                return BadRequest("A customer with that email address already exists");
            }

            Country? country = await _countryRepository.GetCountryByCountryCodeAsync(customerDto.CountryCode);

            if (country == null)
            {
                return BadRequest($"A country with the code: {customerDto.CountryCode} does not exist");
            }

            bool result = await _customerRepository.RegisterNewCustomerAsync(
                customerDto.FirstName, customerDto.LastName, customerDto.Email, country);

            return result ? Ok() : BadRequest("Unable to register the new customer");
        }

        [HttpGet]
        public async Task<IActionResult> Get(int id)
        {
            if(id < 0)
            {
                return BadRequest($"The \"id\" parameter needs to be a positive number");
            }

            Models.Customer? customer = await _customerRepository.GetCustomerByIdAsync(id);

            return customer == null ? NotFound() : Ok(customer);
        }
    }
}
