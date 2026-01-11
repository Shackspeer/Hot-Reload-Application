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
                    
                    watcher.NotifyFilter = NotifyFilters.FileName| NotifyFilters.LastAccess;


                    watcher.Filter = "*.txt";

                    watcher.Changed += OnChange;

                    




                    watcher.EnableRaisingEvents = true;
                    Console.WriteLine($"Watcher is started. Press 'q' to escape");
                    while (Console.ReadKey().KeyChar != 'q') ;
                }
                

            }






            
        }
    
        private static void OnChange(object sender,FileSystemEventArgs e)
        {

            Console.WriteLine($"File changed : {e.Name}");
            using(WatcherDbEntities db = new WatcherDbEntities())
            {

                ActionLogs log = new ActionLogs();
                log.Context = "Update";
                log.Description = "File has updated";
                log.Path = $"\\{e.Name}";
                db.ActionLogs.Add(log);
                db.SaveChanges();
            }
        }
    
    
    
    }

   


}
