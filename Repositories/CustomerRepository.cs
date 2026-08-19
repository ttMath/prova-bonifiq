using Microsoft.EntityFrameworkCore;
using ProvaPub.Infra;
using ProvaPub.Models;
using ProvaPub.Repositories.Interfaces;
using ProvaPub.Services;

namespace ProvaPub.Repositories
{
	public class CustomerRepository : PagedService<Customer>, ICustomerRepository
	{
		public CustomerRepository(TestDbContext ctx)
			: base(ctx)
		{
		}

		public CustomerList ListCustomers(int page)
		{
			var result = ListPage(_ctx.Customers, page);
			return new CustomerList(result.Items, result.TotalCount, result.HasNext);
		}

		public async Task<Customer?> GetCustomerById(int customerId)
		{
			return await _ctx.Customers
				.AsNoTracking()
				.FirstOrDefaultAsync(x => x.Id == customerId);
		}

		public async Task<bool> HasPurchasedSince(int customerId, DateTime startDate)
		{
			return await _ctx.Orders.AnyAsync(x => x.CustomerId == customerId && x.OrderDate >= startDate);
		}

		public async Task<bool> HasPurchasedBefore(int customerId)
		{
			return await _ctx.Orders.AnyAsync(x => x.CustomerId == customerId);
		}
	}
}
