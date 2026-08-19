using Microsoft.EntityFrameworkCore;
using ProvaPub.Infra;

namespace ProvaPub.Repositories
{
	public abstract class PagedRepository<T> where T : class
	{
		private const int PageSize = 10;
		protected readonly TestDbContext _ctx;

		protected PagedRepository(TestDbContext ctx)
		{
			_ctx = ctx;
		}

		protected (List<T> Items, int TotalCount, bool HasNext) ListPage(IQueryable<T> query, int page)
		{
			if (page < 1)
				throw new ArgumentOutOfRangeException(nameof(page));

			var totalCount = query.Count();
			var items = query
				.OrderBy(x => EF.Property<int>(x, "Id"))
				.Skip((page - 1) * PageSize)
				.Take(PageSize)
				.ToList();

			return (items, totalCount, page * PageSize < totalCount);
		}
	}
}
