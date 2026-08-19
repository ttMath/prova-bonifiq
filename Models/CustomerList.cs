namespace ProvaPub.Models
{
	public class CustomerList : PagedList<Customer>
	{
		public CustomerList()
		{
		}

		public CustomerList(List<Customer> customers, int totalCount, bool hasNext)
			: base(customers, totalCount, hasNext)
		{
		}

		public List<Customer> Customers
		{
			get => Items;
			set => Items = value;
		}
	}
}
