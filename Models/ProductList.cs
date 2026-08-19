namespace ProvaPub.Models
{
	public class ProductList : PagedList<Product>
	{
		public ProductList()
		{
		}

		public ProductList(List<Product> products, int totalCount, bool hasNext)
			: base(products, totalCount, hasNext)
		{
		}

		public List<Product> Products
		{
			get => Items;
			set => Items = value;
		}
	}
}
