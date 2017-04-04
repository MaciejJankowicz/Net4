using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace zaj04.Models
{
    public class MembersForTeamViewModel
    {
        public List<Member> Members { get; set; }
        public string TeamName { get; set; }
    }

    public class CreateOrEditMemberViewModel
    {
        public string Name { get; set; }
        public MemberType MemberType { get; set; }
        public string Index { get; set; }
        public string TeamIndex { get; set; }
        public List<SelectListItem> teamsList  { get; set; }
        public List<SelectListItem> membersList { get; set; }
    }

    //public class CreateOrEditMemberViewModels
    //{
    //    public CreateOrEditMemberViewModel Outgoing { get; set; }
    //    public Member Actual { get; set; }
    //}
}