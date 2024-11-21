using Microsoft.AspNetCore.Mvc;
using System.Net;
using TaskManager.Customer.API.Models.DTO;
using TaskManager.Customer.API.Repositories.Abstractions;

namespace TaskManager.Customer.API.Controllers
{
    [Route("TaskManager/api/[controller]")]
    [ApiController]
    public sealed class CustomerController(ICustomerRepository customerRepository,
        ILogger<CustomerController> logger) : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository = customerRepository;
        private readonly ILogger<CustomerController> _logger = logger;

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            if (id < 0)
            {
                return BadRequest($"The \"id\" parameter needs to be a positive number");
            }

            Models.DAO.Customer? customer = await _customerRepository.GetCustomerByIdAsync(id);

            return customer == null ? NotFound() : Ok(new CustomerResponse
            {
                Id = customer.Id,
                DisplayName = customer.DisplayName,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email
            });
        }

        [HttpGet]
        public async Task<IActionResult> SearchForCustomer(string emailAddress)
        {
            if(emailAddress == null || emailAddress == string.Empty)
            {
                return BadRequest($"The \"emailAddress\" parameter needs to be supplied as a request body");
            }

            Models.DAO.Customer? customer = await _customerRepository.GetCustomerByEmailAsync(emailAddress);

            return customer == null ? NotFound() : Ok(new CustomerResponse
            {
                Id = customer.Id,
                DisplayName = customer.DisplayName,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id < 0)
            {
                return BadRequest($"The \"id\" parameter needs to be a positive number");
            }

            var result = await _customerRepository.DeleteCustomerAsync(id);

            return result ? NotFound() : Ok();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterNewCustomer([FromBody] CustomerRequestBody customerDto)
        {
            if (customerDto == null)
            {
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

            if (customerDto.DisplayName == null || customerDto.DisplayName.Equals(string.Empty))
            {
                return BadRequest("You need to provide the customer DisplayName in the request body for them to be registered on the system");
            }

            if (customerDto.Email == null || customerDto.Email.Equals(string.Empty))
            {
                return BadRequest("You need to provide the customer Email address in the request body for them to be registered on the system");
            }

            if (await _customerRepository.GetCustomerByEmailAsync(customerDto.Email) != null)
            {
                return BadRequest("A customer with that email address already exists");
            }

            bool result = await _customerRepository.RegisterNewCustomerAsync(customerDto);

            return result ? Created() : BadRequest("Unable to register the new customer");
        }

        [HttpPatch]
        public async Task<IActionResult> UpdateCustomer([FromBody] CustomerResponse customerDto)
        {
            if (customerDto == null)
            {
                return BadRequest("You need to provide any of the following customers FirstName, LatsName, Email and CountryCode in the request body to register them");
            }

            bool result = await _customerRepository.UpdateCustomerAsync(customerDto);

            return result ? Ok() : new StatusCodeResult((int)HttpStatusCode.NotModified);
        }
    }
}
