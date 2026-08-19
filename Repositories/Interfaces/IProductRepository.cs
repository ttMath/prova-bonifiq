using ProvaPub.Models;

namespace ProvaPub.Repositories.Interfaces
{
	public interface IProductRepository
	{
		ProductList ListProducts(int page);
	}
}
