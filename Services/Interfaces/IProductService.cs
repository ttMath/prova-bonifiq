using ProvaPub.Models;

namespace ProvaPub.Services.Interfaces
{
	public interface IProductService
	{
		ProductList ListProducts(int page);
	}
}
