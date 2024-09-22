using TaskManager.Customer.API.Models.Abstractions;

namespace TaskManager.Customer.API.Models
{
    public sealed class Country : IID
    {
        public int ID { get; set; }

        public string ISOCode { get; set; }

        public string Name { get; set; }
    }
}