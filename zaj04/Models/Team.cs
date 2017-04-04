using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace zaj04.Models
{
    public class Team
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Member> Members { get; set; }

        public Team()
        {
            Members = new HashSet<Member>();
        }
    }
}