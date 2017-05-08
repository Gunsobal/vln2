using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CodeKingdom.Models;
using Microsoft.AspNet.Identity;
using CodeKingdom.Models.Entities;
using CodeKingdom.Repositories;
using CodeKingdom.Models.ViewModels;
using CodeKingdom.Access;
using CodeKingdom.Business;

namespace CodeKingdom.Controllers
{
    [Authorize]
    public class CollaboratorController : Controller
    {
        private CollaboratorStructure collaboratorStructure = new CollaboratorStructure();
        // todo: delete
        private CollaboratorRepository repository = new CollaboratorRepository();

        public ActionResult Create(int? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            CollaboratorViewModel viewModel = collaboratorStructure.CreateCollaboratorViewModelFromProjectAndUserID(id.Value, HttpContext.User.Identity.GetUserId());

            if (!isOwner(viewModel))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            collaboratorStructure.CreateRoleSelectListForViewModel(viewModel);
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,UserName,RoleID,ProjectID")] CollaboratorViewModel collaborator)
        {
            if (ModelState.IsValid)
            {
                if (!isOwner(collaborator))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }

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

            Collaborator collaborator = collaboratorStructure.GetCollaboratorById(id.Value);

            if (collaborator == null)
            {
                return HttpNotFound();
            }

            if (!isOwner(collaborator))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            CollaboratorViewModel viewModel = collaboratorStructure.CreateCollaboratorViewModelFromID(collaborator.ID);
            return View(viewModel);
        }
        // ////////////////////////////////////
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,UserName,RoleID,ProjectID")] CollaboratorViewModel collaborator)
        {
            if (!isOwner(collaborator))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            if (ModelState.IsValid)
            {
                collaboratorStructure.Update(collaborator);
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

            Collaborator collaborator = collaboratorStructure.GetCollaboratorById(id.Value);
            if (!isOwner(collaborator))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

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
            if (!isOwner(repository.GetById(id)))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            collaboratorStructure.Delete(id);
            return RedirectToAction("Index");
        }

        protected bool isOwner(Collaborator collaborator)
        {
            string userID = User.Identity.GetUserId();
            ProjectAccess access = new ProjectAccess(collaborator.ProjectID);

            if (!access.IsOwner(userID))
            {
                return false;
            }

            return true;
        }

        protected bool isOwner(CollaboratorViewModel collaborator)
        {
            string userID = User.Identity.GetUserId();
            ProjectAccess access = new ProjectAccess(collaborator.ProjectID);

            if (!access.IsOwner(userID))
            {
                return false;
            }

            return true;
        }

    }
}
