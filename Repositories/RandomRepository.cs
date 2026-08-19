using Microsoft.EntityFrameworkCore;
using ProvaPub.Infra;
using ProvaPub.Models;
using ProvaPub.Repositories.Interfaces;

namespace ProvaPub.Repositories
{
	public class RandomRepository : IRandomRepository
	{
		private readonly TestDbContext _ctx;

		public RandomRepository(TestDbContext ctx)
		{
			_ctx = ctx;
		}

		public async Task<bool> ExistsNumber(int number)
		{
			return await _ctx.Numbers.AnyAsync(x => x.Number == number);
		}

		public async Task AddNumber(int number)
		{
			_ctx.Numbers.Add(new RandomNumber { Number = number });
			await _ctx.SaveChangesAsync();
		}
	}
}
