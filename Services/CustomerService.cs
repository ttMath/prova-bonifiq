using ProvaPub.Models;
using ProvaPub.Repositories.Interfaces;

namespace ProvaPub.Services
{
	public class CustomerService : ICustomerService
	{
		private readonly ICustomerRepository _customerRepository;
		private readonly IDateTimeProvider _dateTimeProvider;

		public CustomerService(ICustomerRepository customerRepository, IDateTimeProvider dateTimeProvider)
		{
			_customerRepository = customerRepository;
			_dateTimeProvider = dateTimeProvider;
		}

		public CustomerList ListCustomers(int page)
		{
			return _customerRepository.ListCustomers(page);
		}

		public async Task<Customer> GetCustomerById(int customerId)
		{
			if (customerId <= 0)
				throw new ArgumentOutOfRangeException(nameof(customerId));

			var customer = await _customerRepository.GetCustomerById(customerId);

			if (customer == null)
				throw new InvalidOperationException($"Customer Id {customerId} does not exists");

			return customer;
		}

		public async Task<bool> CanPurchase(int customerId, decimal purchaseValue)
		{
			if (customerId <= 0)
				throw new ArgumentOutOfRangeException(nameof(customerId));

			if (purchaseValue <= 0)
				throw new ArgumentOutOfRangeException(nameof(purchaseValue));

			await GetCustomerById(customerId);

			var now = _dateTimeProvider.UtcNow;
			var baseDate = now.AddMonths(-1);

			if (await _customerRepository.HasPurchasedSince(customerId, baseDate))
				return false;

			if (!await _customerRepository.HasPurchasedBefore(customerId) && purchaseValue > 100)
				return false;

			if (now.Hour < 8 || now.Hour > 18 || now.DayOfWeek == DayOfWeek.Saturday || now.DayOfWeek == DayOfWeek.Sunday)
				return false;

			return true;
		}
	}
}
