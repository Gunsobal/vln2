using CodeKingdom.Business;
using CodeKingdom.Models;
using CodeKingdom.Models.Entities;
using CodeKingdom.Models.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;

namespace CodeKingdom.Controllers
{
    [Authorize]
    public class ProjectController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();
        private ProjectStructure projectStructure = new ProjectStructure();
   
        public ActionResult Index()
        {
            List<Project> projects = projectStructure.GetListOfProjects();
            List<ProjectViewModel> viewModels = projectStructure.GetListOfProjectViewModels(projects);
            return View(viewModels);
        }

        public ActionResult Details(int? id, int? fileId)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            EditorViewModel viewModel = projectStructure.GetEditorViewModel(id.Value, fileId);
            string userID = User.Identity.GetUserId();
            ViewBag.leftMenuButton = true;
            ViewBag.newColorscheme = projectStructure.GetColorscheme(userID);
            ViewBag.newKeyBinding = projectStructure.GetKeyBinding(userID);
            return View(viewModel);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name")] ProjectViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                projectStructure.CreateProject(viewModel);
                return RedirectToAction("Index");
            }

            return View(viewModel);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Project project = projectStructure.GetProject(id.Value);

            if (project == null)
            {
                return HttpNotFound();
            }

            ProjectViewModel viewModel = projectStructure.CreateProjectViewModel(project);
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name")] ProjectViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                projectStructure.Update(viewModel);
                return RedirectToAction("Index");
            }
            return View(viewModel);
        }

        public ActionResult ClearChat(int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            projectStructure.ClearChatForProject(id.Value);

            return RedirectToAction("Index", "Project");
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Project project = projectStructure.GetProject(id.Value);

            if (project == null)
            {
                return HttpNotFound();
            }

            ProjectViewModel viewModel = projectStructure.CreateProjectViewModel(project);
            return View(viewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ViewBag.LeftButton = true;
            projectStructure.DeleteById(id);
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
