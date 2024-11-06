using System.ServiceModel;

namespace lab5
{
    [ServiceContract]
    public interface IWCFSimplex
    {
        [OperationContract] int Add(int a, int b);
        [OperationContract] string Concat(string a, double b);
        [OperationContract] A Sum(A a, A b);
    }
}
