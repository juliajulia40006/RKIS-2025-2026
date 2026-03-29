		using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace TodoList;

public class ApiDataStorage : IDataStorage
{
	private readonly HttpClient _client;
	private readonly string _baseUrl = "http://localhost:5000/";

	public ApiDataStorage()
	{
		_client = new HttpClient
		{
			BaseAddress = new Uri(_baseUrl),
			Timeout = TimeSpan.FromSeconds(30)
		};
	}

	public void SaveProfiles(IEnumerable<Profile> profiles)
	{
		string json = JsonSerializer.Serialize(profiles);
		byte[] encrypted = EncryptData(json);
		var content = new ByteArrayContent(encrypted);
		var response = _client.PostAsync("/profiles", content).Result;
		response.EnsureSuccessStatusCode();
	}

	public IEnumerable<Profile> LoadProfiles()
	{
		var response = _client.GetAsync("/profiles").Result;

		if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
		{
			return new List<Profile>();
		}

		response.EnsureSuccessStatusCode();
		byte[] encrypted = response.Content.ReadAsByteArrayAsync().Result;
		string json = DecryptData(encrypted);
		return JsonSerializer.Deserialize<List<Profile>>(json) ?? new List<Profile>();
	}

	public void SaveTodos(Guid userId, IEnumerable<TodoItem> todos)
	{
		var todosForSave = todos.Select(t => new TodoItemSaveDto
		{
			Text = t.Text,
			Status = t.Status,
			LastUpdate = t.LastUpdate
		});

		string json = JsonSerializer.Serialize(todosForSave);
		byte[] encrypted = EncryptData(json);
		var content = new ByteArrayContent(encrypted);
		var response = _client.PostAsync($"/todos/{userId}", content).Result;
		response.EnsureSuccessStatusCode();
	}

	public IEnumerable<TodoItem> LoadTodos(Guid userId)
	{
		var response = _client.GetAsync($"/todos/{userId}").Result;

		if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
		{
			return new List<TodoItem>();
		}

		response.EnsureSuccessStatusCode();
		byte[] encrypted = response.Content.ReadAsByteArrayAsync().Result;
		string json = DecryptData(encrypted);
		var dtos = JsonSerializer.Deserialize<List<TodoItemSaveDto>>(json) ?? new List<TodoItemSaveDto>();

		return dtos.Select(dto =>
		{
			var item = new TodoItem(dto.Text);
			item.SetStatus(dto.Status);
			item.SetLastUpdate(dto.LastUpdate);
			return item;
		}).ToList();
	}

	private byte[] EncryptData(string data)
	{
		using var aes = CryptoHelper.CreateAes();
		byte[] encrypted;

		using (var memoryStream = new MemoryStream())
		{
			using (var cryptoStream = new CryptoStream(memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
			using (var writer = new StreamWriter(cryptoStream, Encoding.UTF8))
			{
				writer.Write(data);
			}
			encrypted = memoryStream.ToArray();
		}

		return encrypted;
	}

	private string DecryptData(byte[] encryptedData)
	{
		using var aes = CryptoHelper.CreateAes();

		using (var memoryStream = new MemoryStream(encryptedData))
		using (var cryptoStream = new CryptoStream(memoryStream, aes.CreateDecryptor(), CryptoStreamMode.Read))
		using (var reader = new StreamReader(cryptoStream, Encoding.UTF8))
		{
			return reader.ReadToEnd();
		}
	}

	private class TodoItemSaveDto
	{
		public string Text { get; set; }
		public TodoStatus Status { get; set; }
		public DateTime LastUpdate { get; set; }
	}
}