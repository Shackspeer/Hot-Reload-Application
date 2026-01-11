using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileService
{
    internal class Reader
    {

        private string[] File1; 


        public Reader(string url)
        {
            this.File1 = File.ReadAllLines(url);
        }



        public object Compare(string url)
        {
            try
            {
                if (!File.Exists(url))
                    return new
                    {
                        state = false,
                        message = "Exception : target file is not found"
                    };
                if (this.File1 == null)
                {
                    return new
                    {
                        state = false,
                        message = "Exception: in order to compare file you have to initialize its first state with using Reader() method"
                    };
                }

                string[] File2 = File.ReadAllLines(url);

                string[] removed = this.File1.Except(File2).ToArray();
                string[] appended = File2.Except(this.File1).ToArray();

                return new { 
                    state = true,
                    removed,
                    appended,
                    message = "State : operation completed successfully"
                };
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"İşlem Hatası: {ex.Message}");
                return new { state = false, message = ex.Message };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Beklenmedik Hata: {ex.Message}");
                return new { state = false, message = "Dosya okunurken bir hata oluştu." };
            }
        }



        private void Read(string url1,string url2)
        {
           



        }

    }
}
