using System.Diagnostics;

namespace ConsoleApp6
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hanoi example ");
            HanoiType type = Hanoi.SelectHanoiType();

            Console.Write("Enter number of discs: ");
            byte k = Convert.ToByte(Console.ReadLine());

            Console.WriteLine($"Running case: {type} with {k} discs:");


            Stopwatch sw = Stopwatch.StartNew();
            K13 name = new K13();
            name.numDiscs = k;
            name.type = type;
            int length = name.ShortestPathForSmallDimension();
            sw.Stop();

            Console.WriteLine();
            Console.WriteLine($"\n\nDimension: {k}; Steps: {length}; Time: {sw.Elapsed.TotalSeconds}");
            Console.WriteLine();
        }
    }
}