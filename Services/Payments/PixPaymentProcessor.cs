namespace ProvaPub.Services.Payments
{
	public class PixPaymentProcessor : IPaymentProcessor
	{
		public string PaymentMethod => "pix";

		public Task Pay(decimal paymentValue, int customerId)
		{
			return Task.CompletedTask;
		}
	}
}
