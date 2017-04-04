using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace zaj04.Models
{
    public enum MemberType
    {
        Dev,
        Own,
        SM,
        Tester
    }
    public class Member
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public MemberType MemberType { get; set; }

        public virtual Team Team { get; set; }
    }
}