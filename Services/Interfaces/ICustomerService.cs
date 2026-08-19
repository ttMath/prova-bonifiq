using ProvaPub.Models;

namespace ProvaPub.Services.Interfaces
{
	public interface ICustomerService
	{
		CustomerList ListCustomers(int page);
		Task<Customer> GetCustomerById(int customerId);
		Task<bool> CanPurchase(int customerId, decimal purchaseValue);
	}
}
