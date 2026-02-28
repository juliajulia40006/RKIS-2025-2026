namespace TodoList;

public interface IDataStorage
{
	void SaveProfiles(IEnumerable<Profile> profiles);
	IEnumerable<Profile> LoadProfiles();
	void SaveTodos(Guid userId, IEnumerable<TodoItem> todos);
	IEnumerable<TodoItem> LoadTodos(Guid userId);
}
