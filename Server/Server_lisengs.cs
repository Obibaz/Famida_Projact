using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Azure;
using DbLayer;
using Models;
using Newtonsoft.Json;

namespace Server
{
    public class Server_lisengs
    {
        private TcpListener _listener;
        private readonly int _port = 9002;

        public Server_lisengs()
        {
            _listener = new TcpListener(IPAddress.Parse("127.0.0.1"), _port);

        }

        public async Task StartAsync()
        {
            _listener.Start();

            await Task.Run(() =>
            {
                while (true)
                {
                    TcpClient client = _listener.AcceptTcpClient();
                    HandleClient(client);
                }
            });
        }



        private void HandleClient(TcpClient client)
        {
            NetworkStream ns = client.GetStream();

            byte[] buffer = new byte[1024];
            int bytesRead = ns.Read(buffer, 0, buffer.Length);
            string jsonString = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            MyRequst my = JsonConvert.DeserializeObject<MyRequst>(jsonString);

            if (my.Header.ToString() == "Login")
            {
                MyResponse response = new MyResponse() { Massage = "WRONG" };
                var factory = new  FemidaDbContextFactory();
                using (var db = factory.CreateDbContext(null))
                {
                    foreach (var item in db.Users)
                    {
                        if (item.Name == my.User_1.Name
                            && item.Pass == my.User_1.Pass && item.Active == true)
                        {
                            response = new MyResponse() { Massage = "SUCCESS", Userss = item };
                        }
                    }
                    string jsonResponse = JsonConvert.SerializeObject(response);
                    byte[] responseData = Encoding.UTF8.GetBytes(jsonResponse);
                    ns.Write(responseData, 0, responseData.Length);


                }
            }
            else if (my.Header.ToString() == "GET ALL USERS")
            {

                MyResponse response = new MyResponse();
                var factory = new FemidaDbContextFactory();
                using (var db = factory.CreateDbContext(null))
                {
                    
                    response.UsersList= db.Users.ToList();
                    string jsonResponse = JsonConvert.SerializeObject(response);
                    byte[] responseData = Encoding.UTF8.GetBytes(jsonResponse);
                    ns.Write(responseData, 0, responseData.Length);

                }

            }
        }

        }
}
