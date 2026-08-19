using Microsoft.EntityFrameworkCore;
using ProvaPub.Models;
using ProvaPub.Repository;

namespace ProvaPub.Services
{
	public class CustomerService : PagedService<Customer>
	{
		public CustomerService(TestDbContext ctx)
			: base(ctx)
		{
		}

		public CustomerList ListCustomers(int page)
		{
			var result = ListPage(_ctx.Customers, page);
			return new CustomerList(result.Items, result.TotalCount, result.HasNext);
		}

		public async Task<Customer> GetCustomerById(int customerId)
		{
			if (customerId <= 0)
				throw new ArgumentOutOfRangeException(nameof(customerId));

			var customer = await _ctx.Customers
				.AsNoTracking()
				.FirstOrDefaultAsync(x => x.Id == customerId);

			if (customer == null)
				throw new InvalidOperationException($"Customer Id {customerId} does not exists");

			return customer;
		}

		public async Task<bool> CanPurchase(int customerId, decimal purchaseValue)
		{
			if (purchaseValue <= 0) throw new ArgumentOutOfRangeException(nameof(purchaseValue));

			await GetCustomerById(customerId);

			var baseDate = DateTime.UtcNow.AddMonths(-1);
			var ordersInThisMonth = await _ctx.Orders.CountAsync(s => s.CustomerId == customerId && s.OrderDate >= baseDate);
			if (ordersInThisMonth > 0)
				return false;

			var haveBoughtBefore = await _ctx.Customers.CountAsync(s => s.Id == customerId && s.Orders.Any());
			if (haveBoughtBefore == 0 && purchaseValue > 100)
				return false;

			if (DateTime.UtcNow.Hour < 8 || DateTime.UtcNow.Hour > 18 || DateTime.UtcNow.DayOfWeek == DayOfWeek.Saturday || DateTime.UtcNow.DayOfWeek == DayOfWeek.Sunday)
				return false;

			return true;
		}
	}
}
