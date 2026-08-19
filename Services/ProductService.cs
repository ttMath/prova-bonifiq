using ProvaPub.Models;
using ProvaPub.Infra;

namespace ProvaPub.Services
{
	public class ProductService : PagedService<Product>
	{
		public ProductService(TestDbContext ctx)
			: base(ctx)
		{
		}

		public ProductList ListProducts(int page)
		{
			var result = ListPage(_ctx.Products, page);
			return new ProductList(result.Items, result.TotalCount, result.HasNext);
		}
	}
}
