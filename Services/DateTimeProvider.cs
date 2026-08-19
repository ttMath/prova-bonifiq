using ProvaPub.Services.Interfaces;

namespace ProvaPub.Services
{
	public class DateTimeProvider : IDateTimeProvider
	{
		public DateTime UtcNow => DateTime.UtcNow;
	}
}
