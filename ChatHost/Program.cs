using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel; 

namespace ChatHost
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var host = new ServiceHost(typeof(wcf_chat.Service1))) // Create a new ServiceHost instance to host the WCF service
            {
                host.Open(); 
                Console.WriteLine("Host has activated!");
                Console.ReadLine(); // Wait for user input before closing the host
            }
        }
    }
}
