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
    public class CollaboratorController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private CollaboratorRepository repository = new CollaboratorRepository();

        /// <summary>
        /// Takes in id of the project end returns all collaborators for that project
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Index(int? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            List<Collaborator> collaborators = repository.GetByProjectId(id.Value);
            List<CollaboratorViewModel> models = new List<CollaboratorViewModel>();

            foreach (var collaborator in collaborators)
            {
                models.Add(
                    new CollaboratorViewModel
                    {
                        ID = collaborator.ID,
                        UserName = collaborator.User.UserName,
                        RoleName = collaborator.Role.Name,
                        RoleID = collaborator.CollaboratorRoleID,
                    }
                );
            }
            
            return View(models);
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

        public ActionResult Edit(int? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Collaborator collaborator = repository.GetById(id.Value);

            if (collaborator == null)
            {
                return HttpNotFound();
            }

            List<CollaboratorRole> roles = repository.GetAllRoles();
            List<SelectListItem> roleList = new List<SelectListItem>();

            foreach (var role in roles)
            {
                roleList.Add(new SelectListItem() { Value = role.ID.ToString(), Text = role.Name });
            }

            CollaboratorViewModel model = new CollaboratorViewModel
            {
                ID = collaborator.ID,
                UserName = collaborator.User.UserName,
                RoleName = collaborator.Role.Name,
                RoleID = collaborator.CollaboratorRoleID,
                Roles = roleList
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID")] Collaborator collaborator)
        {
            if (ModelState.IsValid)
            {
                //repository.Update(collaborator);
                return RedirectToAction("Index");
            }
            return View(collaborator);
        }

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
