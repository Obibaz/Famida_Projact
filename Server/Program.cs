using System.Net.Sockets;
using System.Net;
using DbLayer;
using System.Threading;

namespace Server
{
    internal class Program
    {
        static void Main(string[] args)
        {


            Thread _thread;
            var factory = new FemidaDbContextFactory();
            using (var db = factory.CreateDbContext(null))
            {

            }

            Server_lisengs server = new Server_lisengs();



            _thread = new Thread(server.Start);
            _thread.IsBackground = true;
            _thread.Start();

            Console.ReadLine();

            Console.WriteLine("Hello, World!");
        }

       

    }
}
