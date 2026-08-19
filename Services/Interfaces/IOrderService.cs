using ProvaPub.Models;

namespace ProvaPub.Services.Interfaces
{
	public interface IOrderService
	{
		Task<Order> PayOrder(string paymentMethod, decimal paymentValue, int customerId);
	}
}
