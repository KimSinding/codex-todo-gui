using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace TodoList;

public static class TaskStorage
{
    private const string FileName = "tasks.json";

    public static string FilePath => Path.Combine(GetProjectRoot(), "data", FileName);

    public static List<TodoTask> Load()
    {
        if (!File.Exists(FilePath))
        {
            return [];
        }

        try
        {
            return JsonSerializer.Deserialize<List<TodoTask>>(File.ReadAllText(FilePath)) ?? [];
        }
        catch (JsonException)
        {
            return [];
        }
    }

    public static void Save(IEnumerable<TodoTask> tasks)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(FilePath)!);
        var json = JsonSerializer.Serialize(tasks, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(FilePath, json);
    }

    private static string GetProjectRoot()
    {
        var directory = new DirectoryInfo(Directory.GetCurrentDirectory());

        while (directory is not null)
        {
            if (directory.EnumerateFiles("*.csproj").Any())
            {
                return directory.FullName;
            }

            directory = directory.Parent;
        }

        return AppContext.BaseDirectory;
    }
}
