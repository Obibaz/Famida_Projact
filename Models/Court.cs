using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Court
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public string Poz { get; set; }
        public string Vid { get; set; }
        public string Notes { get; set; }

        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<Decision> Decisions { get; set; }
    }
}
