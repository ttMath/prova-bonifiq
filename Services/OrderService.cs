using Microsoft.EntityFrameworkCore;
using ProvaPub.Infra;
using ProvaPub.Models;
using ProvaPub.Services.Payments;

namespace ProvaPub.Services
{
	public class OrderService
	{
		private readonly TestDbContext _ctx;
		private readonly ICustomerService _customerService;
		private readonly IEnumerable<IPaymentProcessor> _paymentProcessors;
		private readonly BrazilianDateTimeService _brazilianDateTimeService;

		public OrderService(
			TestDbContext ctx,
			ICustomerService customerService,
			IEnumerable<IPaymentProcessor> paymentProcessors,
			BrazilianDateTimeService brazilianDateTimeService)
		{
			_ctx = ctx;
			_customerService = customerService;
			_paymentProcessors = paymentProcessors;
			_brazilianDateTimeService = brazilianDateTimeService;
		}

		public async Task<Order> PayOrder(string paymentMethod, decimal paymentValue, int customerId)
		{
			if (string.IsNullOrWhiteSpace(paymentMethod))
				throw new ArgumentException("Forma de pagamento inválida.", nameof(paymentMethod));

			if (paymentValue <= 0)
				throw new ArgumentOutOfRangeException(nameof(paymentValue));


			var paymentProcessor = _paymentProcessors.FirstOrDefault(x =>
				x.PaymentMethod.Equals(paymentMethod, StringComparison.OrdinalIgnoreCase));

			if (paymentProcessor == null)
				throw new InvalidOperationException($"Forma de pagamento '{paymentMethod}' não suportada.");

			await paymentProcessor.Pay(paymentValue, customerId);

			var order = await InsertOrder(new Order
			{
				CustomerId = customerId,
				Value = paymentValue,
				OrderDate = DateTime.UtcNow
			});
			
			var customer = await _customerService.GetCustomerById(customerId);

			_ctx.Entry(order).State = EntityState.Detached;
			order.Customer = customer;
			order.OrderDate = _brazilianDateTimeService.FromUtc(order.OrderDate);

			return order;
		}

		public async Task<Order> InsertOrder(Order order)
		{
			order.OrderDate = order.OrderDate == default
				? DateTime.UtcNow
				: order.OrderDate.ToUniversalTime();

			var entity = (await _ctx.Orders.AddAsync(order)).Entity;
			await _ctx.SaveChangesAsync();

			return entity;
		}
	}
}
