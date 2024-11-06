namespace lab5
{
    public class WCFSimplex : IWCFSimplex
    {
        public int Add(int a, int b) => a + b;
        public string Concat(string a, double b) => a + b;
        public A Sum(A a, A b) => a + b;
    }
}
