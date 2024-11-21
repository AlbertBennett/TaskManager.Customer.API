namespace TaskManager.Customer.API.Models.Abstractions
{
    /// <summary>
    /// An interface that defines the core fields of a customer
    /// </summary>
    public interface ICustomerBase
    {
        string DisplayName { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string Email { get; set; }
    }
}
