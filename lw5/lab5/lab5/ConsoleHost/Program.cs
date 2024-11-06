using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Description;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleHost
{
    internal class Program
    {
        static void Main(string[] args)
        {

            using (ServiceHost host = new ServiceHost(typeof(lab5.WCFSimplex)))
            {
                host.Open();

                Console.WriteLine("Service host running...");

                foreach (ServiceEndpoint sep in host.Description.Endpoints)
                {
                    Console.WriteLine("  endpoint {0} ({1})",
                                      sep.Address, sep.Binding.Name);
                }

                Console.ReadLine();

                host.Close();
            }
        }
    }
}
