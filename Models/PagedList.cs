namespace ProvaPub.Models
{
	public abstract class PagedList<T>
	{
		protected PagedList()
		{
			Items = new List<T>();
		}

		protected PagedList(List<T> items, int totalCount, bool hasNext)
		{
			Items = items;
			TotalCount = totalCount;
			HasNext = hasNext;
		}

		protected List<T> Items { get; set; }
		public int TotalCount { get; set; }
		public bool HasNext { get; set; }
	}
}
