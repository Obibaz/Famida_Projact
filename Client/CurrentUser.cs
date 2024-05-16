using Models;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;

#pragma warning disable SYSLIB0011

namespace  Client
{
    static class CurrentUser
    {
        public static string? address = "127.0.0.1";
        public static int? port = 9001;

        //public static User? user;
   

        public static async Task<MyResponse> SendMessageAsync(MyRequst request, bool waitForResponse = true)
        {
            try
            {
                if (address is null || port is null)
                {
                    throw new Exception("You aren't connected to the server!");
                }

                using TcpClient acceptor = new(address, port.Value);
                await using NetworkStream ns = acceptor.GetStream();

                BinaryFormatter bf = new();

                bf.Serialize(ns, request);

                if (waitForResponse)
                {
                    MyResponse response = (MyResponse)bf.Deserialize(ns);

                    if (response.Massage is null)
                    {
                        return response;
                    }
                    else
                    {
                        throw new Exception(response.Massage);
                    }
                }

                return null!;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        //смена окон.  минус - при смене окон запущенные потоки продолжают работать, поэтому надо юзать CancellationTokenSource

        public static void SwitchWindow<T>(Window currentWindow) where T : Window
        {
            if (Activator.CreateInstance(typeof(T)) is T window)
            {
                window.Show();
                currentWindow.Close();
            }
        }
    }
}
