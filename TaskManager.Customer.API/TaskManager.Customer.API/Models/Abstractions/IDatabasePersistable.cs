namespace TaskManager.Customer.API.Models.Abstractions
{
    /// <summary>
    /// Defines an interface that is used to expose fields specific for objects that are persistable in the database
    /// </summary>
    public interface IDatabasePersistable
    {
        DateTime? LastLoggedInTime { get; set; }

        DateTime LastUpdatedDate { get; set; }

        int LastUpdatedId { get; set; }
    }
}
