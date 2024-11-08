using System.ServiceModel;

namespace PWS_Lab6
{
    [ServiceContract]
    public interface IService
    {
        [OperationContract]
        void DoWork();
    }
}
