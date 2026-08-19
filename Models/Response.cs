namespace ProvaPub.Models
{
	public class Response<T>
	{
		public Response()
		{
		}

		public Response(T data, string? message = null)
		{
			Data = data;
			Message = message;
		}

		public bool Success { get; set; } = true;
		public T? Data { get; set; }
		public string? Message { get; set; }
	}
}
