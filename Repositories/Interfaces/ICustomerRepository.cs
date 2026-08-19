using ProvaPub.Models;

namespace ProvaPub.Repositories.Interfaces
{
	public interface ICustomerRepository
	{
		CustomerList ListCustomers(int page);
		Task<Customer?> GetCustomerById(int customerId);
		Task<bool> HasPurchasedSince(int customerId, DateTime startDate);
		Task<bool> HasPurchasedBefore(int customerId);
	}
}
