namespace ProvaPub.Services.Payments
{
	public class CreditCardPaymentProcessor : IPaymentProcessor
	{
		public string PaymentMethod => "creditcard";

		public Task Pay(decimal paymentValue, int customerId)
		{
			return Task.CompletedTask;
		}
	}
}
