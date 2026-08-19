namespace ProvaPub.Models
{
	public class OrderRequest
	{
		public string PaymentMethod { get; set; } = string.Empty;
		public decimal PaymentValue { get; set; }
		public int CustomerId { get; set; }
	}
}
