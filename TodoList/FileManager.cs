namespace TodoList;

using System.Security.Cryptography;
using System.Text;

public class FileManager : IDataStorage
{
	private readonly string _dataDirectory;

	public FileManager(string dataDirectory)
	{
		_dataDirectory = dataDirectory;
		EnsureDataDirectory();
	}

	private void EnsureDataDirectory()
	{
		if (!Directory.Exists(_dataDirectory))
		{
			Directory.CreateDirectory(_dataDirectory);
		}
	}

	private string GetProfilesFilePath()
	{
		return Path.Combine(_dataDirectory, "profiles.dat");
	}

	private string GetTodosFilePath(Guid userId)
	{
		return Path.Combine(_dataDirectory, $"todos_{userId}.dat");
	}

	public void SaveProfiles(IEnumerable<Profile> profiles)
	{
		string filePath = GetProfilesFilePath();

		using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
		using (var bufferedStream = new BufferedStream(fileStream, 8192))
		using (var aes = CryptoHelper.CreateAes())
		using (var cryptoStream = new CryptoStream(bufferedStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
		using (var writer = new StreamWriter(cryptoStream, Encoding.UTF8))
		{
			writer.WriteLine("Id;Login;Password;FirstName;LastName;BirthYear");

			foreach (var profile in profiles)
			{
				string line = $"{profile.Id};{profile.Login};{profile.Password};{profile.FirstName};{profile.LastName};{profile.BirthYear}";
				writer.WriteLine(line);
			}
		}
	}

	public IEnumerable<Profile> LoadProfiles()
	{
		string filePath = GetProfilesFilePath();

		if (!File.Exists(filePath))
		{
			return new List<Profile>();
		}

		var profiles = new List<Profile>();

		try
		{
			using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
			using (var bufferedStream = new BufferedStream(fileStream, 8192))
			using (var aes = CryptoHelper.CreateAes())
			using (var cryptoStream = new CryptoStream(bufferedStream, aes.CreateDecryptor(), CryptoStreamMode.Read))
			using (var reader = new StreamReader(cryptoStream, Encoding.UTF8))
			{
				string? line;
				bool isFirstLine = true;

				while ((line = reader.ReadLine()) != null)
				{
					if (isFirstLine)
					{
						isFirstLine = false;
						continue;
					}

					if (string.IsNullOrWhiteSpace(line))
						continue;

					string[] parts = line.Split(';');
					if (parts.Length < 6)
						continue;

					Guid id = Guid.Parse(parts[0]);
					string login = parts[1];
					string password = parts[2];
					string firstName = parts[3];
					string lastName = parts[4];
					int birthYear = int.Parse(parts[5]);

					profiles.Add(new Profile(id, login, password, firstName, lastName, birthYear));
				}
			}
		}
		catch (CryptographicException ex)
		{
			throw new InvalidOperationException("Ошибка расшифровки данных профилей. Файл поврежден.", ex);
		}
		catch (IOException ex)
		{
			throw new InvalidOperationException($"Ошибка доступа к файлу профилей: {ex.Message}", ex);
		}
		catch (FormatException ex)
		{
			throw new InvalidOperationException($"Ошибка формата данных: {ex.Message}", ex);
		}

		return profiles;
	}

	public void SaveTodos(Guid userId, IEnumerable<TodoItem> todos)
	{
		string filePath = GetTodosFilePath(userId);

		using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
		using (var bufferedStream = new BufferedStream(fileStream, 8192))
		using (var aes = CryptoHelper.CreateAes())
		using (var cryptoStream = new CryptoStream(bufferedStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
		using (var writer = new StreamWriter(cryptoStream, Encoding.UTF8))
		{
			writer.WriteLine("Index;Text;Status;LastUpdate");

			int index = 0;
			foreach (var item in todos)
			{
				string escapedText = item.Text.Replace("\"", "\"\"").Replace("\n", "\\n");
				writer.WriteLine($"{index};\"{escapedText}\";{item.Status};{item.LastUpdate:yyyy-MM-ddTHH:mm:ss}");
				index++;
			}
		}
	}

	public IEnumerable<TodoItem> LoadTodos(Guid userId)
	{
		string filePath = GetTodosFilePath(userId);

		if (!File.Exists(filePath))
		{
			return new List<TodoItem>();
		}

		var todos = new List<TodoItem>();

		try
		{
			using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
			using (var bufferedStream = new BufferedStream(fileStream, 8192))
			using (var aes = CryptoHelper.CreateAes())
			using (var cryptoStream = new CryptoStream(bufferedStream, aes.CreateDecryptor(), CryptoStreamMode.Read))
			using (var reader = new StreamReader(cryptoStream, Encoding.UTF8))
			{
				string? line;
				bool isFirstLine = true;

				while ((line = reader.ReadLine()) != null)
				{
					if (isFirstLine)
					{
						isFirstLine = false;
						continue;
					}

					if (string.IsNullOrWhiteSpace(line))
						continue;

					string[] parts = ParseCsvLine(line);
					if (parts.Length < 4)
						continue;

					string text = parts[1].Replace("\"\"", "\"").Replace("\\n", "\n").Trim('"');

					TodoStatus status = Enum.Parse<TodoStatus>(parts[2]);

					DateTime lastUpdate = DateTime.Parse(parts[3]);

					var item = new TodoItem(text);
					item.SetStatus(status);
					item.SetLastUpdate(lastUpdate);

					todos.Add(item);
				}
			}
		}
		catch (CryptographicException ex)
		{
			throw new InvalidOperationException($"Ошибка расшифровки данных задач пользователя {userId}. Файл поврежден.", ex);
		}
		catch (IOException ex)
		{
			throw new InvalidOperationException($"Ошибка доступа к файлу задач пользователя {userId}: {ex.Message}", ex);
		}
		catch (FormatException ex)
		{
			throw new InvalidOperationException($"Ошибка формата данных: {ex.Message}", ex);
		}

		return todos;
	}

	private string[] ParseCsvLine(string line)
	{
		int fieldCount = CountFields(line);
		string[] result = new string[fieldCount];
		int fieldIndex = 0;
		bool inQuotes = false;
		string currentField = "";

		for (int i = 0; i < line.Length; i++)
		{
			char c = line[i];

			if (c == '"')
			{
				inQuotes = !inQuotes;
			}
			else if (c == ';' && !inQuotes)
			{
				result[fieldIndex] = currentField;
				currentField = "";
				fieldIndex++;
			}
			else
			{
				currentField += c;
			}
		}

		if (fieldIndex < result.Length)
		{
			result[fieldIndex] = currentField;
		}

		return result;
	}

	private int CountFields(string line)
	{
		int count = 1;
		bool inQuotes = false;

		for (int i = 0; i < line.Length; i++)
		{
			char c = line[i];

			if (c == '"')
			{
				inQuotes = !inQuotes;
			}
			else if (c == ';' && !inQuotes)
			{
				count++;
			}
		}

		return count;
	}
}