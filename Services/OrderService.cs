using ProvaPub.Models;
using ProvaPub.Repositories.Interfaces;
using ProvaPub.Services.Interfaces;

namespace ProvaPub.Services
{
	public class OrderService : IOrderService
	{
		private readonly IOrderRepository _orderRepository;
		private readonly ICustomerService _customerService;
		private readonly IEnumerable<IPaymentProcessor> _paymentProcessors;
		private readonly IBrazilianDateTimeService _brazilianDateTimeService;
		private readonly IDateTimeProvider _dateTimeProvider;

		public OrderService(
			IOrderRepository orderRepository,
			ICustomerService customerService,
			IEnumerable<IPaymentProcessor> paymentProcessors,
			IBrazilianDateTimeService brazilianDateTimeService,
			IDateTimeProvider dateTimeProvider)
		{
			_orderRepository = orderRepository;
			_customerService = customerService;
			_paymentProcessors = paymentProcessors;
			_brazilianDateTimeService = brazilianDateTimeService;
			_dateTimeProvider = dateTimeProvider;
		}

		public async Task<Order> PayOrder(string paymentMethod, decimal paymentValue, int customerId)
		{
			if (string.IsNullOrWhiteSpace(paymentMethod))
				throw new ArgumentException("Forma de pagamento inválida.", nameof(paymentMethod));

			if (paymentValue <= 0)
				throw new ArgumentOutOfRangeException(nameof(paymentValue));

			var customer = await _customerService.GetCustomerById(customerId);

			var paymentProcessor = _paymentProcessors.FirstOrDefault(x =>
				x.PaymentMethod.Equals(paymentMethod, StringComparison.OrdinalIgnoreCase));

			if (paymentProcessor == null)
				throw new InvalidOperationException($"Forma de pagamento '{paymentMethod}' não suportada.");

			await paymentProcessor.Pay(paymentValue, customerId);

			var order = await InsertOrder(new Order
			{
				CustomerId = customerId,
				Value = paymentValue,
				OrderDate = _dateTimeProvider.UtcNow
			});

			order.Customer = customer;
			order.OrderDate = _brazilianDateTimeService.FromUtc(order.OrderDate);

			return order;
		}

		public async Task<Order> InsertOrder(Order order)
		{
			return await _orderRepository.InsertOrder(order);
		}
	}
}
