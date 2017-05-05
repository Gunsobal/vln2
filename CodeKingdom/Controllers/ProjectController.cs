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
using Microsoft.AspNet.Identity;
using CodeKingdom.Models.ViewModels;
using Microsoft.AspNet.Identity.EntityFramework;

namespace CodeKingdom.Controllers
{
    [Authorize]
    public class ProjectController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();
   
        private ProjectRepository repository = new ProjectRepository();

        private FolderRepository folderRepo = new FolderRepository();

        private FileRepository fileRepo = new FileRepository();
        
        public ActionResult Index()
        {
            string userID = User.Identity.GetUserId();
            List<Project> projects = repository.getByUserId(userID);
            List<ProjectViewModel> viewModels = new List<ProjectViewModel>();

            foreach(var project in projects)
            {
                viewModels.Add(
                    new ProjectViewModel
                    {
                        ID = project.ID,
                        Name = project.Name,
                    }
                );
            }
            
            return View(viewModels);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Project project = repository.getById(id.Value);
            int folderID = project.FolderID;     

            if (project == null)
            {
                return HttpNotFound();
            }

            /* ProjectViewModel viewModel = new ProjectViewModel
             {
                 ID = project.ID,
                 Name = project.Name,
                 Collaborators = project.Collaborators
             };*/

            EditorViewModel viewModel = new EditorViewModel
            {
                Name = project.Name,
                ProjectID = project.ID,
                Collaborators = project.Collaborators,
                Folders = folderRepo.GetChildrenById(folderID),
                Files = fileRepo.GetByFolderId(folderID),
            };

            ViewBag.leftMenuButton = true;

            return View(viewModel);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name")] ProjectViewModel project)
        {
            if (ModelState.IsValid)
            {
                string userID = User.Identity.GetUserId();
                repository.Create(project, userID);
                return RedirectToAction("Index");
            }

            return View(project);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Project project = repository.getById(id.Value);

            if (project == null)
            {
                return HttpNotFound();
            }

            List<CollaboratorViewModel> collaborators = new List<CollaboratorViewModel>();

            foreach (var collaborator in project.Collaborators)
            {
                collaborators.Add(
                    new CollaboratorViewModel
                    {
                        ID = collaborator.ID,
                        UserName = collaborator.User.UserName,
                        RoleName = collaborator.Role.Name,
                        RoleID = collaborator.CollaboratorRoleID,
                        ProjectID = id.Value
                    }
                );
            }

            ProjectViewModel viewModel = new ProjectViewModel
            {
                ID = project.ID,
                Name = project.Name,
                Collaborators = collaborators
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name")] ProjectViewModel project)
        {
            if (ModelState.IsValid)
            {
                repository.Update(project);
                return RedirectToAction("Index");
            }
            return View(project);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Project project = repository.getById(id.Value);

            if (project == null)
            {
                return HttpNotFound();
            }

            ProjectViewModel viewModel = new ProjectViewModel
            {
                ID = project.ID,
                Name = project.Name
            };

            return View(viewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ViewBag.LeftButton = true;
            repository.DeleteById(id);
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
