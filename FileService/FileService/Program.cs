using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;



namespace FileService
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string url = @"C:\Users\Emre\Desktop\Hot-Reload-Uygulaması\TestFiles";


            if (!Directory.Exists(url))
            {
                Console.WriteLine("Incorrect path");
                return;

            }
            else
            {
                using (FileSystemWatcher watcher = new FileSystemWatcher(url))
                {
                    
                    watcher.NotifyFilter = NotifyFilters.LastWrite| NotifyFilters.DirectoryName | NotifyFilters.FileName;


                    watcher.Filter = "*.txt";

                    watcher.Changed += OnChange;
                    watcher.Deleted += OnDelete;

                    watcher.EnableRaisingEvents = true;
                    Console.WriteLine($"Watcher is started. Press 'q' to escape");
                    while (Console.ReadKey().KeyChar != 'q') ;
                }
            }
        }
    
        private static void OnChange(object sender,FileSystemEventArgs e)
        {

            Console.WriteLine($"File has been changed : {e.Name}");
            using(WatcherDbEntities db = new WatcherDbEntities())
            {

                ActionLogs log = new ActionLogs()
                {
                    Context = "Update",
                    Description = "File has been updated",
                    Path = $"\\{e.Name}",
                    Date = DateTime.Now,
                };

                
                
                db.ActionLogs.Add(log);
                db.SaveChanges();
            }
        }
        private static void OnDelete(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine($"File has been removed : {e.Name}");
            using(WatcherDbEntities db = new WatcherDbEntities())
            {
                ActionLogs log = new ActionLogs()
                {
                    Context = "Delete",
                    Description=  "File has been removed",
                    Path = $"\\{e.Name}",
                    Date = DateTime.Now,    
                };
                db.ActionLogs.Add(log);
                db.SaveChanges();

            }
        }
    
    
    
    }

   


}
