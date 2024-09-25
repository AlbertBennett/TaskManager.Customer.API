namespace TaskManager.Customer.API.DataTransferObjects
{
    public class CustomerDTO
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }

        public required string CountryCode { get; set; }
    }
}
