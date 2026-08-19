namespace ProvaPub.Repositories.Interfaces
{
	public interface IRandomRepository
	{
		Task<bool> ExistsNumber(int number);
		Task AddNumber(int number);
	}
}
