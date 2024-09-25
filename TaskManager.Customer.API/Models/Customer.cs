using TaskManager.Customer.API.Models.Abstractions;

namespace TaskManager.Customer.API.Models
{
    public sealed class Customer : IID
    {
        public int ID { get; set; }
        public required string FirstName {  get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }

        public int CountryCodeId { get; set; }
        public required Country Country {  get; set; }
    }
}