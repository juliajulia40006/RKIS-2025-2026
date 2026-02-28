namespace TodoList.Commands;

public class LoadCommand : ICommand
{
	public int DownloadsCount { get; set; }
	public int DownloadSize { get; set; }

	private static readonly object _consoleLock = new object();
	private int _startRow;

	public void Execute()
	{
		RunAsync().Wait();
	}

	private async Task RunAsync()
	{
		_startRow = Console.CursorTop;
		for (int i = 0; i < DownloadsCount; i++)
		{
			Console.WriteLine();
		}

		var tasks = new List<Task>();

		for (int i = 0; i < DownloadsCount; i++)
		{
			int index = i;
			tasks.Add(DownloadAsync(index));
		}

		await Task.WhenAll(tasks);

		Console.SetCursorPosition(0, _startRow + DownloadsCount);
		Console.WriteLine("\nВсе загрузки завершены.");
	}

	private async Task DownloadAsync(int index)
	{
		var random = new Random();

		for (int i = 0; i <= DownloadSize; i++)
		{
			int percent = i * 100 / DownloadSize;
			string bar = GenerateProgressBar(percent);

			lock (_consoleLock)
			{
				int targetRow = _startRow + index;
				if (targetRow >= 0 && targetRow < Console.BufferHeight)
				{
					Console.SetCursorPosition(0, targetRow);
					Console.Write(bar.PadRight(Console.WindowWidth - 1));
				}
			}

			await Task.Delay(random.Next(50, 200));
		}
	}

	private string GenerateProgressBar(int percent)
	{
		int completedBars = percent / 5;
		string bar = "[";

		for (int i = 0; i < 20; i++)
		{
			bar += i < completedBars ? "#" : "-";
		}

		bar += $"] {percent}%";
		return bar.PadRight(Console.WindowWidth - 1);
	}

	public void Unexecute() { }
}