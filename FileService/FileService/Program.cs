using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.ComponentModel.Design;



namespace FileService
{

    


    internal class Program
    {
        private static char _key;



        private static Reader reader;
        private static string[] StartState;

        private static Socket _socket;


        static void Main(string[] args)
        {
            Menu();
        }
    
        private static void OnChange(object sender,FileSystemEventArgs e)
        {

            reader = new Reader(e.FullPath);
            _socket.SendReloadSignal();




            Console.WriteLine($"File has been changed : {e.Name}");
            using(WatcherDbEntities db = new WatcherDbEntities())
            {
                System.Threading.Thread.Sleep(100);
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
            using (WatcherDbEntities db = new WatcherDbEntities())
            {
                

                ActionLogs log = new ActionLogs()
                {
                    Context = "Delete",
                    Description = "File has been removed",
                    Path = $"\\{e.Name}",
                    Date = DateTime.Now,
                };
                db.ActionLogs.Add(log);
                db.SaveChanges();

            }
        }


        private static void StartWatching(string path)
        {
            _socket = new Socket("8081");
            _socket.StartServer();
            if (!Directory.Exists(path))
            {
                Console.WriteLine("Incorrect path");
                return;

            }
            else
            {
               
                using (FileSystemWatcher watcher = new FileSystemWatcher(path))
                {
                    watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.DirectoryName | NotifyFilters.FileName;
                    watcher.Filter = "*.html";
                    watcher.Changed += OnChange;
                    watcher.Deleted += OnDelete;
                    watcher.EnableRaisingEvents = true;
                    Console.WriteLine($"Watcher is started. Press 'q' to escape");
                    while (Console.ReadKey().KeyChar != 'q') ;
                }
            }
        }



        private static void Menu()
        {
            Console.WriteLine($"" +
                $"Chose operation:\n\n" +
                $"1 - Hot Reload"
            );
            
            if(Console.ReadKey().KeyChar == '1')
            {
                StartWatching(@"C:\Users\Emre\Desktop\Hot-Reload-Uygulaması\src");
            }
            else
            {
                Console.WriteLine("Failed");
            }
        }
    
    
    }

   


}
