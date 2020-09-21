using System;
using System.IO;
using System.Linq;

namespace file_tree
{
    static class Program
    {
        static void Main()
        {
            
            while (true)
            {
                Console.WriteLine("1.Вывод дерева по каталогу.\n2.Копирование файла в каталог.\n3.Выход.");
                int a = Convert.ToInt32(Console.ReadLine());
                
                switch (a)
                {
                    case 1:
                        {
                            string startDir;
                            Console.WriteLine("Введите каталог: ");
                            startDir = Console.ReadLine();

                            WriteColored(startDir, ConsoleColor.Blue);
                            Console.WriteLine();

                            PrintTree(startDir);
                            Console.WriteLine("Done.");
                            Console.ReadKey();
                            break;
                        }
                    case 2:
                        {
                            Console.WriteLine("Введите диск: ");
                            string drive = Console.ReadLine();

                            Console.WriteLine("название файла с расширением: ");
                            string file = Console.ReadLine();
                            Console.WriteLine("Введите конечный путь: ");
                            string targetDir = Console.ReadLine();
                            FinderFile($"{drive}:\\", file, targetDir);



                            Console.WriteLine("Done.");
                            Console.ReadKey();
                            break;
                        }
                    case 3:
                        {
                            return;
                        }
                    default:
                        {
                            Console.WriteLine("Ошибка");
                            Console.ReadKey();
                            return;
                        }
                }

                Console.Clear();
            }
        }

        public static bool IsDirectory(this FileSystemInfo fsItem)
        {
            return (fsItem.Attributes & FileAttributes.Directory) == FileAttributes.Directory;
        }

        public static void WriteColored(string s, ConsoleColor color)
        {
            var prevColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.Write(s);
            Console.ForegroundColor = prevColor;
        }

        public static void WriteName(FileSystemInfo fsItem)
        {
            WriteColored(fsItem.Name, fsItem.IsDirectory() ? ConsoleColor.Blue : ConsoleColor.Green);
        }

        static void PrintTree(string startDir, string prefix = "")
        {
            var di = new DirectoryInfo(startDir);
            var fsItems = di.GetFileSystemInfos()
                .Where(f => !f.Name.StartsWith(".")) 
                .OrderBy(f => f.Name)
                .ToList();

            for (int i = 0; i < fsItems.Count; i++)
            {
                try
                {
                    var fsItem = fsItems[i];

                    if (i == fsItems.Count - 1)
                    {
                        Console.Write(prefix + "└── ");
                        WriteName(fsItem);
                        Console.WriteLine();
                        if (fsItem.IsDirectory())
                        {
                            PrintTree(fsItem.FullName, prefix + "    ");
                        }
                    }
                    else
                    {
                        Console.Write(prefix + "├── ");
                        WriteName(fsItem);
                        Console.WriteLine();
                        if (fsItem.IsDirectory())
                        {
                            PrintTree(fsItem.FullName, prefix + "│   ");
                        }
                    }
                }
                catch (Exception er)
                {
                    Console.WriteLine("| " + er.Message);
                }
            }
            
   
        }

        static void FinderFile(string sDir, string fileName, string targetDir)
        {
            foreach(string dir in Directory.GetDirectories(sDir))
            {
                try
                {
                    foreach(string file in Directory.GetFiles(dir, fileName))
                    {
                        string findfile = Path.GetFullPath(file);
                        CopyFile(findfile, targetDir+fileName);
                        
                    }
                    FinderFile(dir, fileName, targetDir);
                }
                catch (Exception er)
                {
                    Console.WriteLine(er.Message);
                }
            }
        }

        static void CopyFile(string curDir, string targetDir)
        {
            FileInfo fi = new FileInfo(curDir);
            if (fi.Exists)
            {
                fi.CopyTo(targetDir, true); 
            }
        }
    }
}