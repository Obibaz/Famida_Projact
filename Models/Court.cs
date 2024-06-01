using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Models
{
    public class Court
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public string? Poz { get; set; }
        public string? Vid { get; set; }
        public string? Notes { get; set; }

        public bool Is_del { get; set; }
        public DateTime? Dates { get; set; }
        
        public DateTime? Dlain { get; set; }

        [JsonIgnore]
        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<Decision> Decisions { get; set; }
    }
}
