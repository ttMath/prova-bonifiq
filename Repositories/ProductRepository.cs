using ProvaPub.Infra;
using ProvaPub.Models;
using ProvaPub.Repositories.Interfaces;

namespace ProvaPub.Repositories
{
	public class ProductRepository : PagedRepository<Product>, IProductRepository
	{
		public ProductRepository(TestDbContext ctx)
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
