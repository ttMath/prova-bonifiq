using ProvaPub.Models;

namespace ProvaPub.Repositories.Interfaces
{
	public interface IOrderRepository
	{
		Task<Order> InsertOrder(Order order);
	}
}
