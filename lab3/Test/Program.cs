using System.Collections.Generic;
using System.IO;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Kotic.Kotic kotic = new Kotic.Kotic(new List<string>()
                {@"C:\Users\Admin\OneDrive\Рабочий стол\testotic"});
            kotic.GenerateArchive(@"C:\Users\Admin\OneDrive\Рабочий стол\7 семестр\OTIC");
        }
    }
}
