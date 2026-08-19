namespace ProvaPub.Services.Payments
{
	public interface IPaymentProcessor
	{
		string PaymentMethod { get; }
		Task Pay(decimal paymentValue, int customerId);
	}
}
