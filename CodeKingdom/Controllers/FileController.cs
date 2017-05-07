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
using CodeKingdom.Repositories;
using CodeKingdom.Models.ViewModels;

namespace CodeKingdom.Controllers
{
    public class FileController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private FileRepository repository = new FileRepository();

        // GET: File
        public ActionResult Index()
        {
            var files = db.Files.Include(f => f.Folder).Include(f => f.Owner);
            return View(files.ToList());
        }

        // GET: File/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            File file = db.Files.Find(id);
            if (file == null)
            {
                return HttpNotFound();
            }
            return View(file);
        }

        // GET: File/Create
        public ActionResult Create()
        {
            ViewBag.FolderID = new SelectList(db.Folders, "ID", "Name");
            ViewBag.ApplicationUserID = new SelectList(db.Users, "Id", "Email");
            return View();
        }

        // POST: File/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Content,Type,FolderID,ApplicationUserID")] File file)
        {
            if (ModelState.IsValid)
            {
                db.Files.Add(file);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.FolderID = new SelectList(db.Folders, "ID", "Name", file.FolderID);
            ViewBag.ApplicationUserID = new SelectList(db.Users, "Id", "Email", file.ApplicationUserID);
            return View(file);
        }

        // GET: File/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            File file = repository.GetById(id.Value);

            if (file == null)
            {
                return HttpNotFound();
            }
            ViewBag.FolderID = new SelectList(db.Folders, "ID", "Name", file.FolderID);
            ViewBag.ApplicationUserID = new SelectList(db.Users, "Id", "Email", file.ApplicationUserID);
            return View(file);
        }

        // POST: File/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Content,Type,FolderID,ApplicationUserID")] File file)
        {
            if (ModelState.IsValid)
            {
                db.Entry(file).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.FolderID = new SelectList(db.Folders, "ID", "Name", file.FolderID);
            ViewBag.ApplicationUserID = new SelectList(db.Users, "Id", "Email", file.ApplicationUserID);
            return View(file);
        }

        // GET: File/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            File file = db.Files.Find(id);
            if (file == null)
            {
                return HttpNotFound();
            }
            return View(file);
        }

        // POST: File/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            File file = db.Files.Find(id);
            db.Files.Remove(file);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(EditorViewModel model)
        {
            FileViewModel fileModel = new FileViewModel
            {
                ID = model.FileID,
                Content = model.Content
            };
            repository.Update(fileModel);
            return RedirectToAction("Index", "Project", null);
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
