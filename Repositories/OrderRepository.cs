using Microsoft.EntityFrameworkCore;
using ProvaPub.Infra;
using ProvaPub.Models;
using ProvaPub.Repositories.Interfaces;

namespace ProvaPub.Repositories
{
	public class OrderRepository : IOrderRepository
	{
		private readonly TestDbContext _ctx;

		public OrderRepository(TestDbContext ctx)
		{
			_ctx = ctx;
		}

		public async Task<Order> InsertOrder(Order order)
		{
			order.OrderDate = order.OrderDate == default
				? DateTime.UtcNow
				: order.OrderDate.ToUniversalTime();

			var entity = (await _ctx.Orders.AddAsync(order)).Entity;
			await _ctx.SaveChangesAsync();
			_ctx.Entry(entity).State = EntityState.Detached;

			return entity;
		}
	}
}
