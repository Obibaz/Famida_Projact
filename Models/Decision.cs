using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Decision
    {
        public int Id { get; set; }
        public int Index { get; set; }
        public DateTime Date { get; set; }
        public string Form { get; set; }
        public string Type { get; set; }

        public int CourtId { get; set; }
        public virtual Court Court { get; set; }
    }
}
