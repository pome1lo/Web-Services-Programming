using System;

namespace ClientHTTP
{
    internal class Program
    {
        static void Main(string[] args)
        {
            lab5.A a = new lab5.A(), b = new lab5.A();
            a.f = 1; b.f = 2;
            a.k = 3; b.k = 4;
            a.s = "a"; b.s = "b";

            var client = new WCFSimplexClient();

            Console.WriteLine(client.Add(1, 2));
            Console.WriteLine(client.Concat("1", 2));

            lab5.A result = client.Sum(a, b);
            Console.WriteLine($"f: {result.f}\nk: {result.k}\ns: {result.s}");

            client.Close();
        }
    }
}
