using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Client;
using DbLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.ApplicationServices;
using Models;
using Newtonsoft.Json;
using static System.Net.Mime.MediaTypeNames;


namespace Server_1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private TcpListener _listener;
        private readonly int _port = 9002;
        public MainWindow()
        {
            _listener = new TcpListener(IPAddress.Parse("127.0.0.1"), _port);
            InitializeComponent();
            var factory = new FemidaDbContextFactory();
            using (var db = factory.CreateDbContext(null))
            {
                // Ініціалізація бази даних, якщо потрібно
            }

            StartAsync();
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

            byte[] buffer = new byte[10240];
            int bytesRead = ns.Read(buffer, 0, buffer.Length);
            string jsonString = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            MyRequst my = JsonConvert.DeserializeObject<MyRequst>(jsonString);

            if (my.Header.ToString() == "Login")
            {
                MyResponse response = new MyResponse() { Massage = "Невірний логін або пароль!" };
                var factory = new FemidaDbContextFactory();
                using (var db = factory.CreateDbContext(null))
                {
                    foreach (var item in db.Users
       .Include(u => u.Courts)
       .ThenInclude(c => c.Decisions).ToList())
                    {
                        if (item.Name == my.User_1.Name
                            && item.Pass == my.User_1.Pass && item.Active == true)
                        {



                            response = new MyResponse() { Massage = "SUCCESS", Userss = item };

                        }
                        else if (item.Name == my.User_1.Name
                            && item.Pass == my.User_1.Pass && item.Active != true)
                        {
                            response = new MyResponse() { Massage = "Доступ заблоковано!" };
                        }
                    }
                    var settings = new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    };
                    string jsonResponse = JsonConvert.SerializeObject(response, settings);
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

                    response.UsersList = db.Users.ToList();
                    string jsonResponse = JsonConvert.SerializeObject(response);
                    byte[] responseData = Encoding.UTF8.GetBytes(jsonResponse);
                    ns.Write(responseData, 0, responseData.Length);

                }
            }

            else if (my.Header.ToString() == "SAVE CHENGE")
            {

                MyResponse response = new MyResponse() { Massage = "Користувача не знайдено!" };
                var factory = new FemidaDbContextFactory();
                using (var db = factory.CreateDbContext(null))
                {
                    var tmp = db.Users.FirstOrDefault(x => x.Id == my.Id);
                    if (tmp != null)
                    {
                        tmp.Name = my.User_1.Name;
                        tmp.Pass = my.User_1.Pass;
                        tmp.Status = my.User_1.Status;
                        tmp.Active = my.User_1.Active;
                        db.SaveChanges();
                        response.Massage = "Зміни застосовані!";
                    }

                    string jsonResponse = JsonConvert.SerializeObject(response);
                    byte[] responseData = Encoding.UTF8.GetBytes(jsonResponse);
                    ns.Write(responseData, 0, responseData.Length);

                }
            }

            else if (my.Header.ToString() == "DELETE USER")
            {

                MyResponse response = new MyResponse() { Massage = "Помилка видалення!" };
                var factory = new FemidaDbContextFactory();
                using (var db = factory.CreateDbContext(null))
                {
                    var userToDelete = db.Users.FirstOrDefault(x => x.Id == my.Id);
                    if (userToDelete != null)
                    {
                        db.Users.Remove(userToDelete);
                        db.SaveChanges();
                        response.Massage = "Видалено!";
                    }
                    string jsonResponse = JsonConvert.SerializeObject(response);
                    byte[] responseData = Encoding.UTF8.GetBytes(jsonResponse);
                    ns.Write(responseData, 0, responseData.Length);

                }
            }

            else if (my.Header.ToString() == "ADD USER")
            {

                MyResponse response = new MyResponse() { Massage = "Помилка додавання!" };
                var factory = new FemidaDbContextFactory();
                using (var db = factory.CreateDbContext(null))
                {
                    db.Users.Add(my.User_1);

                    db.SaveChanges();
                    response.Massage = "Успішно додано!";
                }
                string jsonResponse = JsonConvert.SerializeObject(response);
                byte[] responseData = Encoding.UTF8.GetBytes(jsonResponse);
                ns.Write(responseData, 0, responseData.Length);

            }

            else if (my.Header.ToString() == "ADD COURT")
            {
                List<string> list = new();
                MyResponse response = new MyResponse() { Massage = "Помилка додавання!" };


                Thread staThread = new Thread(() =>
                {
                    var test = new Pars(my.Inf);
                    bool? dialogResult = test.ShowDialog();

                    if (dialogResult != true)
                    {
                       list = test._dis;
                    }

                    // Використовуємо Dispatcher для роботи з UI
                    test.Dispatcher.InvokeShutdown();
                });
                staThread.SetApartmentState(ApartmentState.STA);
                staThread.Start();
                staThread.Join();


                var factory = new FemidaDbContextFactory();
                using (var db = factory.CreateDbContext(null))
                {
                    
                var usersWithCourts = db.Users.Include(u => u.Courts).ThenInclude(c => c.Decisions).ToList();
                    /////////////////////////////////////////////ВИЛЛЕЕТАЄТ ПРОВЕРИТЬ ДОДАВАННЯ 
                    
                    foreach (var user in usersWithCourts)
                    {
                        List<Decision> decisions = new();
                        foreach (var item in list)
                        {
                            decisions.Add(new Decision() { Index = int.Parse(item), Type = "TMP", Form = "TMP", Date = DateTime.Parse("11.11.2021") });
                        }
                        Court court = new Court() { Number = my.Inf, Decisions = decisions, Poz = "TMP", Vid = "TMP", Notes = "TMP" };
                        if(user.Id == my.User_1.Id)
                        user.Courts.Add(court);
                    }
                    db.SaveChanges();
                    response.Massage = "Успішно додано!";
                    
                }
                string jsonResponse = JsonConvert.SerializeObject(response);
                byte[] responseData = Encoding.UTF8.GetBytes(jsonResponse);
                ns.Write(responseData, 0, responseData.Length);

            }
            else if (my.Header.ToString() == "UPD")
            {
                MyResponse response = new MyResponse() { Massage = "eror" };
                var factory = new FemidaDbContextFactory();
                using (var db = factory.CreateDbContext(null))
                {
                    foreach (var item in db.Users
       .Include(u => u.Courts)
       .ThenInclude(c => c.Decisions).ToList())
                    {
                        if (item.Name == my.User_1.Name)
                        {
                            response = new MyResponse() { Massage = "SUCCESS", Userss = item };
                        }
                    }
                    var settings = new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    };
                    string jsonResponse = JsonConvert.SerializeObject(response, settings);
                    byte[] responseData = Encoding.UTF8.GetBytes(jsonResponse);
                    ns.Write(responseData, 0, responseData.Length);
                }



            }
        }

        private void Tmp1_Click(object sender, RoutedEventArgs e)
        {
         
        }
    }
}