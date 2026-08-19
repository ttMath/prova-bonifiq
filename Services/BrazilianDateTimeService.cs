namespace ProvaPub.Services
{
	public class BrazilianDateTimeService
	{
		private readonly TimeZoneInfo _timeZone = GetTimeZone();

		public DateTime FromUtc(DateTime dateTime)
		{
			var utcDateTime = dateTime.Kind == DateTimeKind.Utc
				? dateTime
				: DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);

			return TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, _timeZone);
		}

		private static TimeZoneInfo GetTimeZone()
		{
			try
			{
				return TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");
			}
			catch (TimeZoneNotFoundException)
			{
				return TimeZoneInfo.FindSystemTimeZoneById("America/Sao_Paulo");
			}
		}
	}
}
