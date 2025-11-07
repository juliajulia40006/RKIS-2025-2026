namespace TodoList;

public static class FileManager
{
    public static void EnsureDataDirectory(string dirPath)
    {
        if (!Directory.Exists(dirPath))
        {
            Directory.CreateDirectory(dirPath);
        }
    }
}

public static void SaveProfile(Profile profile, string filePath)
    {
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            writer.WriteLine(profile.FirstName);
            writer.WriteLine(profile.LastName);
            writer.WriteLine(profile.BirthYear);
        }
    }