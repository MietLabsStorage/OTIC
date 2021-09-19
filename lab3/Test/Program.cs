namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Kotic.Kotic kotic = new Kotic.Kotic(@"C:\Users\Admin\OneDrive\Рабочий стол\7 семестр\OTIC\Ququruza.txt");
            kotic.GenerateArchive(@"C:\Users\Admin\OneDrive\Рабочий стол\7 семестр\OTIC");
        }
    }
}
