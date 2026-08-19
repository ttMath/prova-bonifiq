using ProvaPub.Models;
using ProvaPub.Repositories.Interfaces;
using ProvaPub.Services;
using Xunit;

namespace ProvaPub.Tests
{
	public class CustomerServiceTests
	{
		[Fact]
		public async Task CanPurchase_WhenCustomerIdIsInvalid_ThrowsArgumentOutOfRangeException()
		{
			var service = CreateService();

			await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => service.CanPurchase(0, 10));
		}

		[Fact]
		public async Task CanPurchase_WhenPurchaseValueIsInvalid_ThrowsArgumentOutOfRangeException()
		{
			var service = CreateService();

			await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => service.CanPurchase(1, 0));
		}

		[Fact]
		public async Task CanPurchase_WhenCustomerDoesNotExist_ThrowsInvalidOperationException()
		{
			var repository = new CustomerRepositoryFake
			{
				Customer = null
			};
			var service = CreateService(repository);

			await Assert.ThrowsAsync<InvalidOperationException>(() => service.CanPurchase(1, 10));
		}

		[Fact]
		public async Task CanPurchase_WhenCustomerPurchasedInLastMonth_ReturnsFalse()
		{
			var repository = new CustomerRepositoryFake
			{
				HasPurchasedSinceResult = true,
				HasPurchasedBeforeResult = true
			};
			var service = CreateService(repository);

			var result = await service.CanPurchase(1, 10);

			Assert.False(result);
		}

		[Fact]
		public async Task CanPurchase_WhenCustomerNeverPurchasedAndValueIsAboveLimit_ReturnsFalse()
		{
			var repository = new CustomerRepositoryFake
			{
				HasPurchasedSinceResult = false,
				HasPurchasedBeforeResult = false
			};
			var service = CreateService(repository);

			var result = await service.CanPurchase(1, 100.01m);

			Assert.False(result);
		}

		[Fact]
		public async Task CanPurchase_WhenOutsideBusinessHours_ReturnsFalse()
		{
			var service = CreateService(now: new DateTime(2024, 1, 8, 7, 0, 0, DateTimeKind.Utc));

			var result = await service.CanPurchase(1, 10);

			Assert.False(result);
		}

		[Fact]
		public async Task CanPurchase_WhenAfterBusinessHours_ReturnsFalse()
		{
			var service = CreateService(now: new DateTime(2024, 1, 8, 19, 0, 0, DateTimeKind.Utc));

			var result = await service.CanPurchase(1, 10);

			Assert.False(result);
		}

		[Fact]
		public async Task CanPurchase_WhenWeekend_ReturnsFalse()
		{
			var service = CreateService(now: new DateTime(2024, 1, 6, 10, 0, 0, DateTimeKind.Utc));

			var result = await service.CanPurchase(1, 10);

			Assert.False(result);
		}

		[Fact]
		public async Task CanPurchase_WhenFirstPurchaseWithinLimitAndBusinessHours_ReturnsTrue()
		{
			var repository = new CustomerRepositoryFake
			{
				HasPurchasedSinceResult = false,
				HasPurchasedBeforeResult = false
			};
			var service = CreateService(repository);

			var result = await service.CanPurchase(1, 100);

			Assert.True(result);
		}

		[Fact]
		public async Task CanPurchase_WhenCustomerPurchasedBeforeAndValueIsAboveLimit_ReturnsTrue()
		{
			var repository = new CustomerRepositoryFake
			{
				HasPurchasedSinceResult = false,
				HasPurchasedBeforeResult = true
			};
			var service = CreateService(repository);

			var result = await service.CanPurchase(1, 500);

			Assert.True(result);
		}

		[Fact]
		public async Task CanPurchase_UsesOneMonthBeforeCurrentDateToCheckRecentPurchases()
		{
			var now = new DateTime(2024, 1, 8, 10, 0, 0, DateTimeKind.Utc);
			var repository = new CustomerRepositoryFake();
			var service = CreateService(repository, now);

			await service.CanPurchase(1, 10);

			Assert.Equal(now.AddMonths(-1), repository.LastStartDate);
		}

		[Fact]
		public async Task GetCustomerById_WhenCustomerExists_ReturnsCustomer()
		{
			var service = CreateService();

			var result = await service.GetCustomerById(1);

			Assert.Equal(1, result.Id);
			Assert.Equal("Customer", result.Name);
		}

		private static CustomerService CreateService(CustomerRepositoryFake? repository = null, DateTime? now = null)
		{
			return new CustomerService(
				repository ?? new CustomerRepositoryFake(),
				new DateTimeProviderFake(now ?? new DateTime(2024, 1, 8, 10, 0, 0, DateTimeKind.Utc)));
		}

		private sealed class CustomerRepositoryFake : ICustomerRepository
		{
			public Customer? Customer { get; set; } = new()
			{
				Id = 1,
				Name = "Customer",
				Orders = new List<Order>()
			};

			public bool HasPurchasedSinceResult { get; set; }
			public bool HasPurchasedBeforeResult { get; set; }
			public DateTime? LastStartDate { get; private set; }

			public CustomerList ListCustomers(int page)
			{
				return new CustomerList(new List<Customer>(), 0, false);
			}

			public Task<Customer?> GetCustomerById(int customerId)
			{
				return Task.FromResult(Customer?.Id == customerId ? Customer : null);
			}

			public Task<bool> HasPurchasedSince(int customerId, DateTime startDate)
			{
				LastStartDate = startDate;
				return Task.FromResult(HasPurchasedSinceResult);
			}

			public Task<bool> HasPurchasedBefore(int customerId)
			{
				return Task.FromResult(HasPurchasedBeforeResult);
			}
		}

		private sealed class DateTimeProviderFake : IDateTimeProvider
		{
			public DateTimeProviderFake(DateTime utcNow)
			{
				UtcNow = utcNow;
			}

			public DateTime UtcNow { get; }
		}
	}
}
