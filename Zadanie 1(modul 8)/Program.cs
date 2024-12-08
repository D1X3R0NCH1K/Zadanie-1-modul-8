using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zadanie_1_modul_8_
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Введите путь до папки: ");
            string folderPath = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(folderPath))
            {
                Console.WriteLine("Указан пустой путь.");
                return;
            }

            try
            {
                // Проверяем существует ли папка
                if (Directory.Exists(folderPath))
                {
                    CleanOldFilesAndFolders(folderPath);
                    Console.WriteLine("Очистка завершена.");
                }
                else
                {
                    Console.WriteLine("Папка по заданному адресу не существует.");
                }
            }
            catch (UnauthorizedAccessException uae)
            {
                Console.WriteLine("Ошибка доступа: " + uae.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Произошла ошибка: " + ex.Message);
            }
        }

        static void CleanOldFilesAndFolders(string folderPath)
        {
            var directoryInfo = new DirectoryInfo(folderPath);
            TimeSpan timeSpan = TimeSpan.FromMinutes(30);
            DateTime thresholdTime = DateTime.Now - timeSpan;

            // Удаляем файлы
            foreach (var file in directoryInfo.GetFiles())
            {
                if (file.LastAccessTime < thresholdTime)
                {
                    try
                    {
                        file.Delete();
                        Console.WriteLine($"Удален файл: {file.FullName}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Не удалось удалить файл {file.FullName}: {ex.Message}");
                    }
                }
            }

            // Удаляем подпапки рекурсивно
            foreach (var subDirectory in directoryInfo.GetDirectories())
            {
                CleanOldFilesAndFolders(subDirectory.FullName); // Рекурсивный вызов
                if (subDirectory.LastAccessTime < thresholdTime)
                {
                    try
                    {
                        subDirectory.Delete(true); // true для удаления с содержимым
                        Console.WriteLine($"Удалена папка: {subDirectory.FullName}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Не удалось удалить папку {subDirectory.FullName}: {ex.Message}");
                    }
                }
            }
        }
    }
}
