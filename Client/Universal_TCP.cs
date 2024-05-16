using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.TextFormatting;
using Models;
using Newtonsoft.Json;

namespace Client
{
    static class Universal_TCP
    {
        public static string? address = "127.0.0.1";
        public static int? port = 9001;


        public static MyResponse SERVER_PROSTO(MyRequst my)
        {
            using (TcpClient client = new TcpClient("127.0.0.1", 9002))
            {
                NetworkStream ns = client.GetStream();
                var request = my;
                string jsonRequest = JsonConvert.SerializeObject(request);
                byte[] requestData = Encoding.UTF8.GetBytes(jsonRequest);
                ns.Write(requestData, 0, requestData.Length);

                // Отримуємо відповідь від сервера
                byte[] responseData = new byte[1024];
                int bytesRead = ns.Read(responseData, 0, responseData.Length);
                string jsonResponse = Encoding.UTF8.GetString(responseData, 0, bytesRead);


                var tmp = JsonConvert.DeserializeObject<MyResponse>(jsonResponse);

                return tmp;
            }
            return null;

        }


    }

   

}
