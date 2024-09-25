using Microsoft.AspNetCore.Mvc;
using TaskManager.Customer.API.Models;
using TaskManager.Customer.API.Repositories.Abstractions;

namespace TaskManager.Customer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public sealed class CountryController(ICountryRepository countryRepository,
        ILogger<CountryController> logger) : ControllerBase
    {
        private readonly ICountryRepository _countryRepository = countryRepository;

        private readonly ILogger<CountryController> _logger = logger;

        [HttpGet]
        public async Task<IActionResult> Get(string? name)
        {
            if (name == null || name.Length == 0)
            {
                return BadRequest("\"name\" query parameter needs to be supplied");
            }

            Country? result = name.Length > 3 ? await _countryRepository.GetCountryByName(name) :
                    await _countryRepository.GetCountryByCountryCode(name);

            return result != null ? Ok(result) : NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> Get(int? id)
        {
            if (id != null || id < 0)
            {
                return BadRequest("\"Id\" query parameter needs to be supplied with a possitive number");
            }

            Country? result = await _countryRepository.GetCountryByID(id.Value);

            return result != null ? Ok(result) : NotFound();
        }
    }
}