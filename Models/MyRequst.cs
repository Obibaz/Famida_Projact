﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class MyRequst
    {
        public int? Id { get; set; }

        public string Header { get; set; }
        public List<User>? Userss { get; set; }
        public User User_1 { get; set; }
        public List<Court> Courtss { get; set; }

    }
}
