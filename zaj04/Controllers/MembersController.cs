using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using zaj04.Models;

namespace zaj04.Controllers
{
    public class MembersController : Controller
    {
        private CompanyContext db = new CompanyContext();

        // GET: Members
        public ActionResult Index()
        {
            return View(db.Members.ToList());
        }

        // GET: Members/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = db.Members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }

        public ActionResult CreateOrEdit(int? id)
        {
            var teams = db.Teams.ToList();
            TempData["TeamsListGivenMC"] = teams;
            ViewBag.Id = id;

            var members = db.Members.ToList();
            TempData["MembersListGivenMC"] = members;
            ViewBag.MembersList = members;

            int teamIdReceived;
            try
            {
                teamIdReceived = (members.Find(n => n.Id == id).Team.Id);
            }
            catch (Exception)
            {
                teamIdReceived = -1;
            } 
            List<SelectListItem> selectList = new List<SelectListItem>();
            for (int i = 0; i < teams.Count; i++)
            {
                selectList.Add(new SelectListItem
                {
                    Selected = id != null ? (teams[i].Id == teamIdReceived) : false,
                    Text = teams[i].Name,
                    Value = i.ToString()
                });
            }            
            ViewBag.TeamsList = selectList;

            List<SelectListItem> membersSelectList = new List<SelectListItem>();
            membersSelectList.Add(new SelectListItem
            {
                Selected =  false,
                Text = "None",
                Value = "-1"
            });
            for (int i = 0; i < members.Count; i++)
            {
                membersSelectList.Add(new SelectListItem
                {
                    Selected = id != null ? (members[i].Id == id) : false,
                    Text = members[i].Name,
                    Value = i.ToString()
                });
            }

            CreateOrEditMemberViewModel ToGive = new CreateOrEditMemberViewModel { membersList = membersSelectList, teamsList = selectList };
            var member = members.Find(n => n.Id == id);
            if (member != null)
            {
                ToGive.Name = member.Name;
                ToGive.MemberType = member.MemberType;
            }                
            return View(ToGive);
        }

        // POST: Teams/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateOrEdit([Bind(Include = "Name,MemberType,Index,TeamIndex")] CreateOrEditMemberViewModel member)
        {
            Member memberActual = new Member { Name = member.Name, MemberType = member.MemberType };
            if (ModelState.IsValid)
            {
                memberActual.Team = member.TeamIndex == "-1" ? null :
                 ((List<Team>)TempData["TeamsListGivenMC"])
                   [int.Parse(member.TeamIndex)];
                db.Teams.Attach(memberActual.Team);


                if (member.Index == null || member.Index == "-1")
                {
                    int i = 0;
                    Random r = new Random();
                    do
                    {
                        memberActual.Id = r.Next();
                        i++;
                        if (i > 1000)
                        {
                            return View(memberActual);
                        }
                    } while (db.Members.Any(o => o.Id == memberActual.Id));

                    db.Members.AddOrUpdate(memberActual);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                memberActual.Id = ((List<Member>)TempData["MembersListGivenMC"])
                    [int.Parse(member.Index)]
                    .Id;
                db.Entry(memberActual).State = EntityState.Modified;

                //db.Members.AddOrUpdate(memberActual);
                db.SaveChanges();
                var m = db.Members.Single(n => n.Id == memberActual.Id);
                var t = db.Teams.Single(n => n.Id == memberActual.Team.Id);
                m.Team = t;
                db.Entry(m).State = EntityState.Unchanged;
                db.Entry(t).State = EntityState.Unchanged;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(member);
        }

        // GET: Members/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Members/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,MemberType")] Member member)
        {
            if (ModelState.IsValid)
            {
                db.Members.Add(member);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(member);
        }

        // GET: Members/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = db.Members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }

        // POST: Members/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,MemberType")] Member member)
        {
            if (ModelState.IsValid)
            {
                db.Entry(member).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(member);
        }

        // GET: Members/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = db.Members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }

        // POST: Members/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Member member = db.Members.Find(id);
            db.Members.Remove(member);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
