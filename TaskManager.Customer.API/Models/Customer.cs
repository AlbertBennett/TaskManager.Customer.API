using TaskManager.Customer.API.Models.Abstractions;

namespace TaskManager.Customer.API.Models
{
    public sealed class Customer : IID
    {
        public int ID { get; set; }
        public string FirstName {  get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public int CountryCodeId { get; set; }
        public Country Country {  get; set; }
    }
}