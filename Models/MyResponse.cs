using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class MyResponse
    {
        public int? Id { get; set; }
        public string? Massage { get; set; }
        public User? Userss { get; set; }

        public List<User>? UsersList { get; set; }

    }
}
