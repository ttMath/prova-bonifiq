using Microsoft.EntityFrameworkCore;
using ProvaPub.Models;
using ProvaPub.Repository;
using System.Security.Cryptography;

namespace ProvaPub.Services
{
	public class RandomService
	{
		private static readonly SemaphoreSlim Semaphore = new(1, 1);
		private readonly TestDbContext _ctx;

		public RandomService(TestDbContext ctx)
		{
			_ctx = ctx;
		}

		public async Task<int> GetRandom()
		{
			await Semaphore.WaitAsync();

			try
			{
				int number;

				do
				{
					number = RandomNumberGenerator.GetInt32(1, int.MaxValue);
				}
				while (await _ctx.Numbers.AnyAsync(x => x.Number == number));

				_ctx.Numbers.Add(new RandomNumber() { Number = number });
				await _ctx.SaveChangesAsync();

				return number;
			}
			finally
			{
				Semaphore.Release();
			}
		}
	}
}
