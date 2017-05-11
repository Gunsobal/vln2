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
using CodeKingdom.Notify;

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

            CollaboratorViewModel viewModel = new CollaboratorViewModel
            {
                ProjectID = id.Value,
                Roles = collaboratorStructure.CreateRoleSelectListForViewModel(),
            };

            // collaboratorStructure.CreateCollaboratorViewModelFromProjectAndUserID(id.Value, HttpContext.User.Identity.GetUserId());

            if (!isOwner(viewModel))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

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

                if (collaboratorStructure.Create(collaborator))
                {
                    var builder = new UriBuilder(Request.Url.AbsoluteUri)
                    {
                        Path = Url.Action("Index", "Project", new { id = collaborator.ProjectID }),
                        Query = null,
                    };

                    string url = builder.ToString();
                    string message = String.Format("You have been added to a new project, go to {0} to view the project", url);
                    Mailer mail = new Mailer(collaborator.UserName, "Code Kingdom", message);
                    mail.Send();
                    return RedirectToAction("Index", "Project", new { id = collaborator.ProjectID });
                }
            }

            collaborator.Roles = collaboratorStructure.CreateRoleSelectListForViewModel();

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
            //return RedirectToAction("Index");
            return RedirectToAction("Index", "Project");
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
