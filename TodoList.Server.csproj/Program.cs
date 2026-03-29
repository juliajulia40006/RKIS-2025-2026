using System.Net;
using System.Text;

namespace TodoList.Server;

class Program
{
	private static readonly string _dataDirectory = "server_data";

	static async Task Main(string[] args)
	{
		if (!Directory.Exists(_dataDirectory))
		{
			Directory.CreateDirectory(_dataDirectory);
		}

		var listener = new HttpListener();
		listener.Prefixes.Add("http://localhost:5000/");
		listener.Start();
		Console.WriteLine("Сервер запущен: http://localhost:5000/");

		while (true)
		{
			HttpListenerContext context = await listener.GetContextAsync();
			_ = HandleRequestAsync(context);
		}
	}

	static async Task HandleRequestAsync(HttpListenerContext context)
	{
		HttpListenerRequest request = context.Request;
		HttpListenerResponse response = context.Response;

		try
		{
			string path = request.Url?.AbsolutePath ?? "";
			string method = request.HttpMethod;

			Console.WriteLine($"{method} {path}");

			if (method == "POST" && path == "/profiles")
			{
				byte[] data = await ReadRequestBody(request);
				string filePath = Path.Combine(_dataDirectory, "profiles.dat");
				await File.WriteAllBytesAsync(filePath, data);
				response.StatusCode = 200;
				string responseText = "OK";
				byte[] buffer = Encoding.UTF8.GetBytes(responseText);
				response.ContentLength64 = buffer.Length;
				await response.OutputStream.WriteAsync(buffer);
			}
			else if (method == "GET" && path == "/profiles")
			{
				string filePath = Path.Combine(_dataDirectory, "profiles.dat");
				if (File.Exists(filePath))
				{
					byte[] data = await File.ReadAllBytesAsync(filePath);
					response.ContentLength64 = data.Length;
					await response.OutputStream.WriteAsync(data);
					response.StatusCode = 200;
				}
				else
				{
					response.StatusCode = 404;
				}
			}
			else if (method == "POST" && path.StartsWith("/todos/"))
			{
				string userId = path.Substring(7);
				byte[] data = await ReadRequestBody(request);
				string filePath = Path.Combine(_dataDirectory, $"server_todos_{userId}.dat");
				await File.WriteAllBytesAsync(filePath, data);
				response.StatusCode = 200;
				string responseText = "OK";
				byte[] buffer = Encoding.UTF8.GetBytes(responseText);
				response.ContentLength64 = buffer.Length;
				await response.OutputStream.WriteAsync(buffer);
			}
			else if (method == "GET" && path.StartsWith("/todos/"))
			{
				string userId = path.Substring(7);
				string filePath = Path.Combine(_dataDirectory, $"server_todos_{userId}.dat");
				if (File.Exists(filePath))
				{
					byte[] data = await File.ReadAllBytesAsync(filePath);
					response.ContentLength64 = data.Length;
					await response.OutputStream.WriteAsync(data);
					response.StatusCode = 200;
				}
				else
				{
					response.StatusCode = 404;
				}
			}
			else
			{
				response.StatusCode = 404;
				string responseText = "Not Found";
				byte[] buffer = Encoding.UTF8.GetBytes(responseText);
				response.ContentLength64 = buffer.Length;
				await response.OutputStream.WriteAsync(buffer);
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Ошибка: {ex.Message}");
			response.StatusCode = 500;
		}
		finally
		{
			response.Close();
		}
	}

	static async Task<byte[]> ReadRequestBody(HttpListenerRequest request)
	{
		using var memoryStream = new MemoryStream();
		await request.InputStream.CopyToAsync(memoryStream);
		return memoryStream.ToArray();
	}
}