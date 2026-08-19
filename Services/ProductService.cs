using ProvaPub.Models;
using ProvaPub.Repositories.Interfaces;
using ProvaPub.Services.Interfaces;

namespace ProvaPub.Services
{
	public class ProductService : IProductService
	{
		private readonly IProductRepository _productRepository;

		public ProductService(IProductRepository productRepository)
		{
			_productRepository = productRepository;
		}

		public ProductList ListProducts(int page)
		{
			return _productRepository.ListProducts(page);
		}
	}
}
