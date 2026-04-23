using TodoList.Commands;
using TodoList.Exceptions;
using TodoList.Models;
using Xunit;

namespace TodoList.Tests;

public class CommandParserTests
{
	private void SetupCommandParser()
	{
		var todoList = new global::TodoList.TodoList();
		var profile = new Profile(Guid.NewGuid(), "user", "pass", "bib", "bob", 2000);

		CommandParser.Initialize(todoList, profile);
	}

	[Theory]
	[InlineData("add bue")]
	[InlineData("add  b uu ee")]
	[InlineData("add   bb  u   e")]
	public void Parse_AddCommand_ReturnsAddCommand(string input)
	{

		SetupCommandParser();

		var command = CommandParser.Parse(input);

		Assert.NotNull(command);
	}

	[Theory]
	[InlineData("view --all")]
	[InlineData("view -a")]
	public void Parse_ListCommand_ReturnsCommand(string input)
	{

		SetupCommandParser();

		var command = CommandParser.Parse(input);

		Assert.NotNull(command);
	}

	[Theory]
	[InlineData("status 1 completed")]
	[InlineData("status 5 completed")]
	[InlineData("status 14 completed")]
	public void Parse_CompleteCommand_ReturnsCommand(string input)
	{

		SetupCommandParser();

		var command = CommandParser.Parse(input);

		Assert.NotNull(command);
	}

	[Theory]
	[InlineData("delete 1")]
	[InlineData("delete 5")]
	[InlineData("delete 59")]
	public void Parse_DeleteCommand_ReturnsCommand(string input)
	{

		SetupCommandParser();

		var command = CommandParser.Parse(input);

		Assert.NotNull(command);
	}

	[Theory]
	[InlineData("help")]
	public void Parse_HelpCommand_ReturnsCommand(string input)
	{

		SetupCommandParser();

		var command = CommandParser.Parse(input);

		Assert.NotNull(command);
	}

	[Theory]
	[InlineData("undo")]
	public void Parse_UndoCommand_ReturnsCommand(string input)
	{

		SetupCommandParser();

		var command = CommandParser.Parse(input);

		Assert.NotNull(command);
	}

	[Theory]
	[InlineData("redo")]
	public void Parse_RedoCommand_ReturnsCommand(string input)
	{

		SetupCommandParser();

		var command = CommandParser.Parse(input);

		Assert.NotNull(command);
	}

	[Theory]
	[InlineData("exit")]
	public void Parse_ExitCommand_ReturnsCommand(string input)
	{

		SetupCommandParser();

		var command = CommandParser.Parse(input);

	
		Assert.NotNull(command);
	}

	[Theory]
	[InlineData("")]
	[InlineData("   ")]
	public void Parse_EmptyInput_ThrowsInvalidCommandException(string input)
	{

		SetupCommandParser();

		Assert.Throws<InvalidCommandException>(() => CommandParser.Parse(input));
	}

	[Theory]
	[InlineData("unknown")]
	[InlineData("bebebe")]
	[InlineData("foooo")]
	public void Parse_UnknownCommand_ThrowsInvalidCommandException(string input)
	{
		SetupCommandParser();

		Assert.Throws<InvalidCommandException>(() => CommandParser.Parse(input));
	}
}