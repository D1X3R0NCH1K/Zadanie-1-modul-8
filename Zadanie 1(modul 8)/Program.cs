using System;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Введите путь к папке:");
        string folderPath = Console.ReadLine();

        /// Проверяем, существует ли директория
        if (!Directory.Exists(folderPath))
        {
            Console.WriteLine("Ошибка: Указанная папка не существует.");
            return;
        }

        try
        {
            CleanDirectory(folderPath);
            Console.WriteLine("Очистка завершена.");
        }
        catch (UnauthorizedAccessException ex)
        {
            Console.WriteLine($"Ошибка доступа: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Произошла ошибка: {ex.Message}");
        }
    }

    static void CleanDirectory(string path)
    {
        var timeLimit = DateTime.Now - TimeSpan.FromMinutes(30);

        /// Удаляем файлы
        foreach (var file in Directory.GetFiles(path))
        {
            try
            {
                var lastAccessTime = File.GetLastAccessTime(file);
                if (lastAccessTime < timeLimit)
                {
                    File.Delete(file);
                    Console.WriteLine($"Удален файл: {file}");
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine($"Нет доступа к файлу: {file}. {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при удалении файла {file}. {ex.Message}");
            }
        }

        /// Удаляем папки рекурсивно
        foreach (var directory in Directory.GetDirectories(path))
        {
            try
            {
                CleanDirectory(directory);

                /// Проверяем, если папка пуста после очистки
                if (Directory.GetFileSystemEntries(directory).Length == 0)
                {
                    Directory.Delete(directory);
                    Console.WriteLine($"Удалена папка: {directory}");
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine($"Нет доступа к папке: {directory}. {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при удалении папки {directory}. {ex.Message}");
            }
        }
    }
}
