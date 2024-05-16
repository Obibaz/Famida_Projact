using Models;
using Newtonsoft.Json;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
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

                // Серіалізуємо об'єкт в JSON та відправляємо його на сервер
                string jsonRequest = JsonConvert.SerializeObject(request);
                byte[] requestData = Encoding.UTF8.GetBytes(jsonRequest);
                ns.Write(requestData, 0, requestData.Length);

                // Отримуємо відповідь від сервера
                byte[] responseData = new byte[1024];
                int bytesRead = ns.Read(responseData, 0, responseData.Length);
                string jsonResponse = Encoding.UTF8.GetString(responseData, 0, bytesRead);

               

                if (waitForResponse)
                {
                    MyResponse response = JsonConvert.DeserializeObject<MyResponse>(jsonResponse);

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
