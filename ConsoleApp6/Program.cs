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

            switch (type)
            {
                case HanoiType.K13_01:
                case HanoiType.K13_12:
                    Stopwatch sw1 = Stopwatch.StartNew();
                    K13 graf1 = new K13(type);
                    graf1.numDiscs = k;
                    int length1 = graf1.ShortestPathForSmallDimension();
                    sw1.Stop();

                    Console.WriteLine();
                    Console.WriteLine($"\n\nDimension: {k}; Steps: {length1}; Time: {sw1.Elapsed.TotalSeconds}");
                    Console.WriteLine();
                    break;
                case HanoiType.K13e_01:
                case HanoiType.K13e_12:
                case HanoiType.K13e_23:
                case HanoiType.K13e_30:
                    Stopwatch sw2 = Stopwatch.StartNew();
                    K13e graf2 = new K13e(type);
                    graf2.numDiscs = k;
                    int length2 = graf2.ShortestPathForSmallDimension();
                    sw2.Stop();

                    Console.WriteLine();
                    Console.WriteLine($"\n\nDimension: {k}; Steps: {length2}; Time: {sw2.Elapsed.TotalSeconds}");
                    Console.WriteLine();
                    break;
                case HanoiType.K4e_01:
                case HanoiType.K4e_12:
                case HanoiType.K4e_23:
                    Stopwatch sw3 = Stopwatch.StartNew();
                    K4e graf3 = new K4e(type);
                    graf3.numDiscs = k;
                    int length3 = graf3.ShortestPathForSmallDimension();
                    sw3.Stop();

                    Console.WriteLine();
                    Console.WriteLine($"\n\nDimension: {k}; Steps: {length3}; Time: {sw3.Elapsed.TotalSeconds}");
                    Console.WriteLine();
                    break;
                case HanoiType.C4_01:
                case HanoiType.C4_12:
                    Stopwatch sw4 = Stopwatch.StartNew();
                    C4 graf4 = new C4(type);
                    graf4.numDiscs = k;
                    int length4 = graf4.ShortestPathForSmallDimension();
                    sw4.Stop();

                    Console.WriteLine();
                    Console.WriteLine($"\n\nDimension: {k}; Steps: {length4}; Time: {sw4.Elapsed.TotalSeconds}");
                    Console.WriteLine();
                    break;
                case HanoiType.P4_01:
                case HanoiType.P4_12:
                case HanoiType.P4_23:
                case HanoiType.P4_31:
                    Stopwatch sw5 = Stopwatch.StartNew();
                    K13 graf5 = new K13(type);
                    graf5.numDiscs = k;
                    int length5 = graf5.ShortestPathForSmallDimension();
                    sw5.Stop();

                    Console.WriteLine();
                    Console.WriteLine($"\n\nDimension: {k}; Steps: {length5}; Time: {sw5.Elapsed.TotalSeconds}");
                    Console.WriteLine();
                    break;
            }
        }
    }
}