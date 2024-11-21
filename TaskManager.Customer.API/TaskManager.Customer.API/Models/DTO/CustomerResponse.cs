using TaskManager.Customer.API.Models.Abstractions;

namespace TaskManager.Customer.API.Models.DTO
{
    /// <summary>
    /// Defines the API response customer object
    /// </summary>
    public class CustomerResponse : IID, ICustomerBase
    {
        /// <summary>
        /// Unique ID to identify the Customer
        /// </summary>
        public required int Id { get; set; }

        /// <summary>
        /// The customers display name for their profile
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Customer's first name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Customer's last name
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Customer's email
        /// </summary>
        public string Email { get; set; }
    }
}
