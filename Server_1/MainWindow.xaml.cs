using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DbLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Models;
using Newtonsoft.Json;

namespace Server_1
{
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

            while (true)
            {
                TcpClient client = await _listener.AcceptTcpClientAsync();
                _ = Task.Run(() => HandleClientAsync(client));
            }
        }

        private async Task HandleClientAsync(TcpClient client)
        {
            using (client)
            {
                NetworkStream ns = client.GetStream();
                byte[] buffer = new byte[102400];
                int bytesRead = await ns.ReadAsync(buffer, 0, buffer.Length);
                string jsonString = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                MyRequst my = JsonConvert.DeserializeObject<MyRequst>(jsonString);

                MyResponse response = new MyResponse();


                switch (my.Header)
                {
                    case "Login":
                        response = await HandleLoginAsync(my);
                        break;
                    case "GET ALL USERS":
                        response = await HandleGetAllUsersAsync();
                        break;
                    case "SAVE CHENGE":
                        response = await HandleSaveChangeAsync(my);
                        break;
                    case "DELETE USER":
                        response = await HandleDeleteUserAsync(my);
                        break;
                    case "ADD USER":
                        response = await HandleAddUserAsync(my);
                        break;
                    case "ADD COURT":
                        response = await HandleAddCourtAsync(my);
                        break;
                    case "UPD":
                        response = await HandleUpdateAsync(my);
                        break;
                    case "SAVE CH":
                        response = await HandleSaveChAsync(my);
                        break;
                    default:
                        response.Massage = "Unknown request header!";
                        break;
                }

                var settings = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };

                string jsonResponse = JsonConvert.SerializeObject(response, settings);
                byte[] responseData = Encoding.UTF8.GetBytes(jsonResponse);
                await ns.WriteAsync(responseData, 0, responseData.Length);

                // ДОДАВАННЯ ДО ТЕКСТБОКСУ
                Dispatcher.Invoke(() =>
                {
                    Lisen.Text += "  "+DateTime.Now.ToShortDateString().ToString()+" "+
                    DateTime.Now.ToShortTimeString().ToString()
                    +" ||| "+ my.User_1.Status.ToString() 
                    +" "+ my.User_1.Name.ToString()
                    + " --> "+ my.Header+ "\n";
                });
            }
        }

        private async Task<MyResponse> HandleLoginAsync(MyRequst my)
        {
            var response = new MyResponse() { Massage = "Невірний логін або пароль!" };
            var factory = new FemidaDbContextFactory();
            using (var db = factory.CreateDbContext(null))
            {
                var users = await db.Users
                    .Include(u => u.Courts)
                    .ThenInclude(c => c.Decisions)
                    .ToListAsync();

                foreach (var item in users)
                {
                    if (item.Name == my.User_1.Name && item.Pass == my.User_1.Pass && item.Active)
                    {
                        response = new MyResponse() { Massage = "SUCCESS", Userss = item };
                    }
                    else if (item.Name == my.User_1.Name && item.Pass == my.User_1.Pass && !item.Active)
                    {
                        response = new MyResponse() { Massage = "Доступ заблоковано!" };
                    }
                }
            }

            return response;
        }

        private async Task<MyResponse> HandleGetAllUsersAsync()
        {
            var response = new MyResponse();
            var factory = new FemidaDbContextFactory();
            using (var db = factory.CreateDbContext(null))
            {
                response.UsersList = await db.Users.ToListAsync();
            }

            return response;
        }

        private async Task<MyResponse> HandleSaveChangeAsync(MyRequst my)
        {
            var response = new MyResponse() { Massage = "Користувача не знайдено!" };
            var factory = new FemidaDbContextFactory();
            using (var db = factory.CreateDbContext(null))
            {
                var tmp = await db.Users.FirstOrDefaultAsync(x => x.Id == my.Id);
                if (tmp != null)
                {
                    tmp.Name = my.User_1.Name;
                    tmp.Pass = my.User_1.Pass;
                    tmp.Status = my.User_1.Status;
                    tmp.Active = my.User_1.Active;
                    await db.SaveChangesAsync();
                    response.Massage = "Зміни застосовані!";
                }
            }

            return response;
        }

        private async Task<MyResponse> HandleDeleteUserAsync(MyRequst my)
        {
            var response = new MyResponse() { Massage = "Помилка видалення!" };
            var factory = new FemidaDbContextFactory();
            using (var db = factory.CreateDbContext(null))
            {
                var userToDelete = await db.Users.FirstOrDefaultAsync(x => x.Id == my.Id);
                if (userToDelete != null)
                {
                    db.Users.Remove(userToDelete);
                    await db.SaveChangesAsync();
                    response.Massage = "Видалено!";
                }
            }

            return response;
        }

        private async Task<MyResponse> HandleAddUserAsync(MyRequst my)
        {
            var response = new MyResponse() { Massage = "Помилка додавання!" };
            var factory = new FemidaDbContextFactory();
            using (var db = factory.CreateDbContext(null))
            {
                await db.Users.AddAsync(my.User_1);
                await db.SaveChangesAsync();
                response.Massage = "Успішно додано!";
            }

            return response;
        }

        private async Task<MyResponse> HandleAddCourtAsync(MyRequst my)
        {
            List<string> list = new();
            List<Decision> listds = new();
            string disigi = null;
            MyResponse response = new MyResponse() { Massage = "Помилка додавання!" };

            await Task.Run(() =>
            {
                Thread staThread = new Thread(() =>
                {
                    var test = new Pars(my.Inf);
                    bool? dialogResult = test.ShowDialog();
                    if (dialogResult != true)
                    {
                        listds = test._decisions;
                        list = test._dis;
                    }
                    // Dispatcher для роботи з UI
                    test.Dispatcher.InvokeShutdown();
                });
                staThread.SetApartmentState(ApartmentState.STA);
                staThread.Start();
                staThread.Join();
            });

            if (!list.IsNullOrEmpty())
            {

                for (int i = 0; i < listds.Count; i++)
                {
                    listds[i].Index = int.Parse(list[i]);
                }

                var factory = new FemidaDbContextFactory();
                using (var db = factory.CreateDbContext(null))
                {
                    var usersWithCourts = await db.Users.Include(u => u.Courts).ThenInclude(c => c.Decisions).ToListAsync();

                    foreach (var user in usersWithCourts)
                    {
                        Court court = new Court() { Number = my.Inf, Decisions = listds, Poz = my.Inf1, Vid = my.Inf2, Notes = "Тут Ваша замітка про суд" };
                        if (user.Id == my.User_1.Id)
                            user.Courts.Add(court);
                    }

                    await db.SaveChangesAsync();
                    response.Massage = "Успішно додано!";
                }
            }
            else if (list.IsNullOrEmpty())
                response.Massage = "Рішення не знайдено, перевірьте данні!";
            return response;
        }

        private async Task<MyResponse> HandleUpdateAsync(MyRequst my)
        {
            var response = new MyResponse() { Massage = "eror" };
            var factory = new FemidaDbContextFactory();
            using (var db = factory.CreateDbContext(null))
            {
                var users = await db.Users
                    .Include(u => u.Courts)
                    .ThenInclude(c => c.Decisions)
                    .ToListAsync();

                foreach (var item in users)
                {
                    if (item.Name == my.User_1.Name)
                    {
                        response = new MyResponse() { Massage = "SUCCESS", Userss = item };
                    }
                }
            }

            return response;
        }

        private async Task<MyResponse> HandleSaveChAsync(MyRequst my)
        {
            var response = new MyResponse() { Massage = "eror" };
            var factory = new FemidaDbContextFactory();
            using (var db = factory.CreateDbContext(null))
            {
                var users = await db.Users
                    .Include(u => u.Courts)
                    .ThenInclude(c => c.Decisions)
                    .ToListAsync();

                foreach (var item in users)
                {
                    if (item.Name == my.User_1.Name)
                    {
                        var tmp1 = item.Courts.FirstOrDefault(x => x.Id == my.Id);
                        tmp1.Decisions = my.Court1.Decisions.ToList();
                        response.Massage = "Зміни застосовані!";
                    }
                }

                await db.SaveChangesAsync();
            }

            return response;
        }

        private void Lisen_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }
    }
}
