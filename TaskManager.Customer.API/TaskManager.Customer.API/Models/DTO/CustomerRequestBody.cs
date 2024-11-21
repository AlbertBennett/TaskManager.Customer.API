using TaskManager.Customer.API.Models.Abstractions;

namespace TaskManager.Customer.API.Models.DTO
{
    /// <summary>
    /// Defines the request body for registering customers through the API
    /// </summary>
    public class CustomerRequestBody : ICustomerBase
    {
        /// <summary>
        /// The customers display name for their profile
        /// </summary>
        public required string DisplayName { get; set; }

        /// <summary>
        /// Customer's first name
        /// </summary>
        public required string FirstName { get; set; }

        /// <summary>
        /// Customer's last name
        /// </summary>
        public required string LastName { get; set; }

        /// <summary>
        /// Customer's email
        /// </summary>
        public required string Email { get; set; }
    }
}
