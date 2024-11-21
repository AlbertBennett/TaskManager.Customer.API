namespace TaskManager.Customer.API.Models.Abstractions
{
    /// <summary>
    /// An interface used to define objects that can be enabled or disabled
    /// </summary>
    public interface IEnabled
    {
        bool IsEnabled { get; set; }
    }
}
