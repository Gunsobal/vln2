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
        private ProjectRepository projectRepository = new ProjectRepository();

        public ActionResult Create(int? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FileViewModel model = new FileViewModel
            {
                ProjectID = id.Value
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ProjectID,Name,Type")] FileViewModel file)
        {
            if (ModelState.IsValid)
            {
                Project project = projectRepository.getById(file.ProjectID);
                file.FolderID = project.Root.ID;
                File newfile = repository.Create(file);
                return RedirectToAction("Details", "Project", new { id = file.ProjectID, fileID = newfile.ID});
            }

            return View(file);
        }

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
            
            return View(file);
        }

        // POST: File/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Type")] FileViewModel file)
        {
            if (ModelState.IsValid)
            {
                repository.Rename(file);
                return RedirectToAction("Index");
            }
            
            return View(file);
        }

        public ActionResult Delete(int? id)
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

            FileViewModel model = new FileViewModel
            {
                ID = file.ID,
                Name = file.Name,
                Type = file.Type,
                Content = file.Content,
            };

            return View(model);
        }

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
            repository.UpdateContent(fileModel);
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
