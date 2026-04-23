using TodoList.Models;
using Xunit;

namespace TodoList.Tests;

public class TodoItemTests
{
	[Fact]
	public void Constructor_WithValidText_CreatesTodoItem()
	{
	
		var text = "Buy milkis";

		var item = new TodoItem(text);

		Assert.Equal(text, item.Text);
		Assert.Equal(TodoStatus.NotStarted, item.Status);
	}

	[Fact]
	public void SetStatus_ToCompleted_UpdatesStatusAndLastUpdate()
	{

		var item = new TodoItem("Test task");
		var oldUpdate = item.LastUpdate;

		item.SetStatus(TodoStatus.Completed);

		Assert.Equal(TodoStatus.Completed, item.Status);
		Assert.NotEqual(oldUpdate, item.LastUpdate);
	}

	[Fact]
	public void SetStatus_ToInProgress_UpdatesStatus()
	{
		var item = new TodoItem("Test task");

		item.SetStatus(TodoStatus.InProgress);

		Assert.Equal(TodoStatus.InProgress, item.Status);
	}

	[Fact]
	public void SetStatus_ToPostponed_UpdatesStatus()
	{
		var item = new TodoItem("Test task");

		item.SetStatus(TodoStatus.Postponed);

		Assert.Equal(TodoStatus.Postponed, item.Status);
	}

	[Fact]
	public void SetStatus_ToFailed_UpdatesStatus()
	{
		var item = new TodoItem("Test task");

		item.SetStatus(TodoStatus.Failed);

		Assert.Equal(TodoStatus.Failed, item.Status);
	}

	[Fact]
	public void UpdateText_WithNewText_UpdatesTextAndLastUpdate()
	{
		var item = new TodoItem("minions");
		var newText = "hello kitty";
		var oldUpdate = item.LastUpdate;

		item.UpdateText(newText);

		Assert.Equal(newText, item.Text);
		Assert.NotEqual(oldUpdate, item.LastUpdate);
	}

	[Fact]
	public void GetShortInfo_WithShortText_ReturnsFormattedInfo()
	{
		var item = new TodoItem("korotkiy");

		var result = item.GetShortInfo();

		Assert.Contains("korotkiy", result);
		Assert.Contains("не начато", result);
	}

	[Fact]
	public void GetShortInfo_WithLongText_TruncatesText()
	{
		var longText = new string('a', 100);
		var item = new TodoItem(longText);

		var result = item.GetShortInfo();

		Assert.Contains("...", result);
		Assert.True(result.Length < longText.Length + 20);
	}

	[Fact]
	public void GetFullInfo_ReturnsCompleteInformation()
	{
		var item = new TodoItem("Full task description");

		var result = item.GetFullInfo();

		Assert.Contains("Full task description", result);
		Assert.Contains("не начато", result);
		Assert.Contains("Дата изменения", result);
	}

	[Theory]
	[InlineData(TodoStatus.NotStarted, "не начато")]
	[InlineData(TodoStatus.InProgress, "в процессе")]
	[InlineData(TodoStatus.Completed, "выполнено")]
	[InlineData(TodoStatus.Postponed, "отложено")]
	[InlineData(TodoStatus.Failed, "провалено")]
	public void GetStatusText_ForVariousStatuses_ReturnsCorrectText(TodoStatus status, string expected)
	{
		var result = TodoItem.GetStatusText(status);

		Assert.Equal(expected, result);
	}
}