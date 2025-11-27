namespace TodoList.Commands;

using System;

public interface ICommand
{
    void Execute();
	void Unexecute();

}
