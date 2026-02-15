using static TodoList.TodoItem;

namespace TodoList.Commands;

public class SearchCommand : ICommand
{
	public List<TodoItem> TodoItems { get; set; }

	public string? ContainsText { get; set; }
	public string? StartsWithText { get; set; }
	public string? EndsWithText { get; set; }
	public DateTime? FromDate { get; set; }
	public DateTime? ToDate { get; set; }
	public TodoStatus? Status { get; set; }
	public string? SortBy { get; set; }
	public string? ThenBy { get; set; }
	public bool SortDesc { get; set; }
	public int? Top { get; set; }

	public void Execute()
	{
		var query = TodoItems.AsEnumerable();

		if (!string.IsNullOrEmpty(ContainsText))
			query = query.Where(item => item.Text.Contains(ContainsText));

		if (!string.IsNullOrEmpty(StartsWithText))
			query = query.Where(item => item.Text.StartsWith(StartsWithText));

		if (!string.IsNullOrEmpty(EndsWithText))
			query = query.Where(item => item.Text.EndsWith(EndsWithText));

		if (FromDate.HasValue)
			query = query.Where(item => item.LastUpdate >= FromDate.Value);

		if (ToDate.HasValue)
			query = query.Where(item => item.LastUpdate <= ToDate.Value);

		if (Status.HasValue)
			query = query.Where(item => item.Status == Status.Value);

		if (!string.IsNullOrEmpty(SortBy))
		{
			if (SortBy == "text")
			{
				query = SortDesc
					? query.OrderByDescending(item => item.Text)
					: query.OrderBy(item => item.Text);
			}
			else if (SortBy == "date")
			{
				query = SortDesc
					? query.OrderByDescending(item => item.LastUpdate)
					: query.OrderBy(item => item.LastUpdate);
			}

			if (!string.IsNullOrEmpty(ThenBy))
			{
				var ordered = (IOrderedEnumerable<TodoItem>)query;

				if (ThenBy == "text" && SortBy != "text")
				{
					query = SortDesc
						? ordered.ThenByDescending(item => item.Text)
						: ordered.ThenBy(item => item.Text);
				}
				else if (ThenBy == "date" && SortBy != "date")
				{
					query = SortDesc
						? ordered.ThenByDescending(item => item.LastUpdate)
						: ordered.ThenBy(item => item.LastUpdate);
				}
			}
		}

		if (Top.HasValue && Top.Value > 0)
			query = query.Take(Top.Value);

		var results = query.ToList();

		if (results.Count == 0)
		{
			Console.WriteLine("\nНичего не найдено");
			return;
		}

		Console.WriteLine("\nРезультаты поиска:");
		TodoList.View(results, true, true, true);
	}

	public void Unexecute() { }
}