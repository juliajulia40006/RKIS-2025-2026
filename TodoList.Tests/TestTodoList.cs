using TodoList.Models;
using TodoList.Exceptions;
using Xunit;

namespace TodoList.Tests;

public class TodoListTests
{
	[Fact]
	public void Add_WithValidItem_IncreasesCount()
	{
		var todoList = new global::TodoList.TodoList();
		var item = new TodoItem("testtest");

		todoList.Add(item);

		Assert.Equal(1, todoList.Count);
	}

	[Fact]
	public void Add_MultipleItems_CountMatches()
	{

		var todoList = new global::TodoList.TodoList();
		var item1 = new TodoItem("buebue");
		var item2 = new TodoItem("tempalay");
		var item3 = new TodoItem("muucc");

		todoList.Add(item1);
		todoList.Add(item2);
		todoList.Add(item3);

		Assert.Equal(3, todoList.Count);
	}

	[Fact]
	public void Add_ItemCanBeAccessedByIndex()
	{

		var todoList = new global::TodoList.TodoList();
		var item = new TodoItem("testa");

		todoList.Add(item);

		Assert.Equal(item, todoList[0]);
	}

	[Fact]
	public void Delete_WithValidIndex_RemovesTask()
	{

		var todoList = new global::TodoList.TodoList();
		var item = new TodoItem("deletethis");
		todoList.Add(item);
		var initialCount = todoList.Count;

		todoList.Delete(0);

		Assert.Equal(initialCount - 1, todoList.Count);
	}

	[Fact]
	public void Delete_WithValidIndex_RemovesCorrectTask()
	{

		var todoList = new global::TodoList.TodoList();
		var item1 = new TodoItem("12");
		var item2 = new TodoItem("21");
		todoList.Add(item1);
		todoList.Add(item2);

		todoList.Delete(0);

		Assert.Equal(item2, todoList[0]);
		Assert.Equal(1, todoList.Count);
	}

	[Fact]
	public void Delete_WithInvalidIndex_ThrowsArgumentOutOfRangeException()
	{

		var todoList = new global::TodoList.TodoList();
		var invalidIndex = 999;

		Assert.Throws<ArgumentOutOfRangeException>(() => todoList.Delete(invalidIndex));
	}

	[Fact]
	public void Delete_WithNegativeIndex_ThrowsArgumentOutOfRangeException()
	{

		var todoList = new global::TodoList.TodoList();
		var invalidIndex = -1;

		Assert.Throws<ArgumentOutOfRangeException>(() => todoList.Delete(invalidIndex));
	}

	[Fact]
	public void Count_WhenListIsEmpty_ReturnsZero()
	{

		var todoList = new global::TodoList.TodoList();

		var count = todoList.Count;

		Assert.Equal(0, count);
	}

	[Fact]
	public void Indexer_WithValidIndex_ReturnsCorrectItem()
	{

		var todoList = new global::TodoList.TodoList();
		var item = new TodoItem("tssst");
		todoList.Add(item);

		var result = todoList[0];

		Assert.Equal(item, result);
	}

	[Fact]
	public void Indexer_WithInvalidIndex_ThrowsException()
	{

		var todoList = new global::TodoList.TodoList();

		Assert.Throws<ArgumentOutOfRangeException>(() => todoList[999]);
	}
}