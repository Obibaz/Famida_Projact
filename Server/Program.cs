using DbLayer;

namespace Server
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var factory = new FemidaDbContextFactory();
            using (var db = factory.CreateDbContext(null))
            {

            }
            Console.WriteLine("Hello, World!");
        }
    }
}
