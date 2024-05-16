using System.Net.Sockets;
using System.Net;
using DbLayer;
using System.Threading;

namespace Server
{
    internal class Program
    {
        static async Task Main(string[] args)
        {


            var factory = new FemidaDbContextFactory();
            using (var db = factory.CreateDbContext(null))
            {
                // Ініціалізація бази даних, якщо потрібно
            }

            Server_lisengs server = new Server_lisengs();
            await server.StartAsync();

            Console.ReadLine(); // Зупинка програми при натисканні Enter
            Console.WriteLine("Hello, World!");
        }
    }

       

    
}
