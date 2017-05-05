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

        public ActionResult Create(int? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            CollaboratorViewModel model = new CollaboratorViewModel();
            model.ProjectID = id.Value;

            List<CollaboratorRole> roles = repository.GetAllRoles();
            List<SelectListItem> roleList = new List<SelectListItem>();

            foreach (var role in roles)
            {
                roleList.Add(new SelectListItem() { Value = role.ID.ToString(), Text = role.Name });
            }
            model.Roles = roleList;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,UserName,RoleID,ProjectID")] CollaboratorViewModel collaborator)
        {
            if (ModelState.IsValid)
            {
                if (repository.Create(collaborator))
                {
                    return RedirectToAction("Index", "Project", new { id = collaborator.ProjectID });
                }
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
                ProjectID = collaborator.ProjectID,
                Roles = roleList
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,UserName,RoleID,ProjectID")] CollaboratorViewModel collaborator)
        {
            if (ModelState.IsValid)
            {
                repository.Update(collaborator);
                return RedirectToAction("Edit", "Project", new { id = collaborator.ProjectID });
            }
            return View(collaborator);
        }

        public ActionResult Delete(int? id)
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

            return View(collaborator);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            repository.Delete(id);
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
