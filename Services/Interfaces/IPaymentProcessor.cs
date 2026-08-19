namespace ProvaPub.Services.Interfaces
{
	public interface IPaymentProcessor
	{
		string PaymentMethod { get; }
		Task Pay(decimal paymentValue, int customerId);
	}
}
