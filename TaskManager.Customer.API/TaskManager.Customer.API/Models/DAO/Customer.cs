using TaskManager.Customer.API.Models.Abstractions;
using TaskManager.Customer.API.Models.DTO;

namespace TaskManager.Customer.API.Models.DAO
{
    public class Customer : IID, ICustomerBase, IDatabasePersistable, IEnabled
    {
        /// <summary>
        /// Unique ID to identify the Customer
        /// </summary>
        public required int Id { get; set; }

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

        /// <summary>
        /// The date the customer last logged in
        /// </summary>
        public DateTime? LastLoggedInTime { get; set; }

        /// <summary>
        /// The date this profile was last updated
        /// </summary>
        public DateTime LastUpdatedDate { get; set; }

        /// <summary>
        /// Who updated the customers profile last
        /// </summary>
        public int LastUpdatedId { get; set; }

        /// <summary>
        /// Is the customer profile currently enabled
        /// </summary>
        public bool IsEnabled { get; set; }
    }
}
