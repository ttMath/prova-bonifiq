namespace ProvaPub.Services.Payments
{
	public class PayPalPaymentProcessor : IPaymentProcessor
	{
		public string PaymentMethod => "paypal";

		public Task Pay(decimal paymentValue, int customerId)
		{
			return Task.CompletedTask;
		}
	}
}
