using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CodeKingdom.Models;
using CodeKingdom.Models.Entities;

namespace CodeKingdom.Controllers
{
    public class CollaboratorController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Collaborators
        public ActionResult Index()
        {
            return View(db.Collaborators.ToList());
        }

        // GET: Collaborators/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Collaborator collaborator = db.Collaborators.Find(id);
            if (collaborator == null)
            {
                return HttpNotFound();
            }
            return View(collaborator);
        }

        // GET: Collaborators/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Collaborators/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID")] Collaborator collaborator)
        {
            if (ModelState.IsValid)
            {
                db.Collaborators.Add(collaborator);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(collaborator);
        }

        // GET: Collaborators/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Collaborator collaborator = db.Collaborators.Find(id);
            if (collaborator == null)
            {
                return HttpNotFound();
            }
            return View(collaborator);
        }

        // POST: Collaborators/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID")] Collaborator collaborator)
        {
            if (ModelState.IsValid)
            {
                db.Entry(collaborator).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(collaborator);
        }

        // GET: Collaborators/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Collaborator collaborator = db.Collaborators.Find(id);
            if (collaborator == null)
            {
                return HttpNotFound();
            }
            return View(collaborator);
        }

        // POST: Collaborators/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Collaborator collaborator = db.Collaborators.Find(id);
            db.Collaborators.Remove(collaborator);
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
