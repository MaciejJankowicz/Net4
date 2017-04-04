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
    public class TeamsController : Controller
    {
        private CompanyContext db = new CompanyContext();

        // GET: Teams
        public ActionResult Index()
        {
            return View(db.Teams.ToList());
        }

        // GET: Teams/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Team team = db.Teams.Find(id);
            if (team == null)
            {
                return HttpNotFound();
            }
            return View(team);
        }

        public ActionResult CreateOrEdit(int? id)
        {
            var teams = db.Teams.ToList();
            TempData["TeamsListGiven"] = teams;
            ViewBag.List = teams;
            ViewBag.Id = id;
            return View(teams.Find(n => n.Id == id));
        }

        // POST: Teams/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateOrEdit([Bind(Include = "Name,Index")] CreateOrEditTeamViewModel team)
        {
            Team teamActual = new Team { Name = team.Name };
            if (ModelState.IsValid)
            {   
                if (team.Index == null)
                {
                    int i = 0;
                    Random r = new Random();
                    do
                    {
                        teamActual.Id = r.Next();
                        i++;
                        if (i > 1000)
                        {
                            return View(teamActual);
                        }
                    } while (db.Teams.Any(o => o.Id == teamActual.Id));

                    db.Teams.AddOrUpdate(teamActual);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                teamActual.Id = ((List<Team>)TempData["TeamsListGiven"])
                    [int.Parse(team.Index.Substring(team.Index.LastIndexOf(' ') + 1))]
                    .Id;
                
                db.Teams.AddOrUpdate(teamActual);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(teamActual);
        }

        // GET: Teams/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Teams/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name")] Team team)
        {
            if (ModelState.IsValid)
            {
                int i = 0;
                Random r = new Random();
                do
                {
                    team.Id = r.Next();
                    i++;
                    if (i > 1000)
                    {
                        return View(team);
                    }
                } while (db.Teams.Any(o => o.Id == team.Id));

                db.Teams.AddOrUpdate(team);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(team);
        }

        // GET: Teams/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Team team = db.Teams.Find(id);
            if (team == null)
            {
                return HttpNotFound();
            }
            return View(team);
        }

        // POST: Teams/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] Team team)
        {
            if (ModelState.IsValid)
            {
                db.Entry(team).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(team);
        }

        // GET: Teams/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Team team = db.Teams.Find(id);
            if (team == null)
            {
                return HttpNotFound();
            }
            return View(team);
        }

        // POST: Teams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Team team = db.Teams.Find(id);
            foreach (var member in team.Members)
            {
                member.Team = null;
            }
            db.Teams.Remove(team);
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
