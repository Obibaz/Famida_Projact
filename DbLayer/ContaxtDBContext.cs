using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using Microsoft.EntityFrameworkCore;

namespace DbLayer
{
    public class ContaxtDBContext : DbContext
    {
        public ContaxtDBContext(DbContextOptions options) : base(options)
        {
            //Database.EnsureDeleted();
            //Database.EnsureCreated();
            //First_Data();
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Court> Courts { get; set; }
        public DbSet<Decision> Decisions { get; set; }

        private void First_Data()
        {

            
            var user1 = new User { Name = "1", Pass = "1", Active = true, Status = "work" };
            var user2 = new User { Name = "2", Pass = "2", Active = true, Status = "admin" };
            

            Users.AddRange(user1, user2);

            SaveChanges();
        }

       
    }
}
