using System.Collections.Generic;
using System.IO;
using Kotic;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Kotic.KoticArchivator koticArchivator = new Kotic.KoticArchivator(new List<string>()
                {@"C:\Users\Admin\OneDrive\Рабочий стол\testotic"});
            koticArchivator.GenerateArchive(@"C:\Users\Admin\OneDrive\Рабочий стол\7 семестр\OTIC");

            Kotic.KoticDearchivator koticDearchivator =
                new KoticDearchivator(@"C:\Users\Admin\OneDrive\Рабочий стол\7 семестр\OTIC\kotic.kotic");
            koticDearchivator.GenerateFilesFromArchive(@"C:\Users\Admin\OneDrive\Рабочий стол\7 семестр\OTIC\dearc");
        }
    }
}
