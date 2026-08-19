using ProvaPub.Repositories.Interfaces;
using ProvaPub.Services.Interfaces;
using System.Security.Cryptography;

namespace ProvaPub.Services
{
	public class RandomService : IRandomService
	{
		private static readonly SemaphoreSlim Semaphore = new(1, 1);
		private readonly IRandomRepository _randomRepository;

		public RandomService(IRandomRepository randomRepository)
		{
			_randomRepository = randomRepository;
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
				while (await _randomRepository.ExistsNumber(number));

				await _randomRepository.AddNumber(number);

				return number;
			}
			finally
			{
				Semaphore.Release();
			}
		}
	}
}
