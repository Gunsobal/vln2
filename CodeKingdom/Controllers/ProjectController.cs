using CodeKingdom.Business;
using CodeKingdom.Exceptions;
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

        private ProjectStructure projectStructure = new ProjectStructure();
   
        /// <summary>
        /// Project main view. Show every project user is a part of
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            List<Project> projects = projectStructure.GetListOfProjects();
            List<ProjectViewModel> viewModels = projectStructure.GetListOfProjectViewModels(projects);
            return View(viewModels);
        }

        /// <summary>
        /// Editor view. Loads ace editor with signal r
        /// </summary>
        /// <param name="id"></param>
        /// <param name="fileId"></param>
        /// <returns></returns>
        public ActionResult Details(int? id, int? fileId)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            try
            {
                EditorViewModel viewModel = projectStructure.GetEditorViewModel(id.Value, fileId);
                string userID = User.Identity.GetUserId();
                ViewBag.leftMenuButton = true;
                ViewBag.newColorscheme = projectStructure.GetColorscheme(userID);
                ViewBag.newKeyBinding = projectStructure.GetKeyBinding(userID);
                return View(viewModel);
            }
            catch(ProjectNotFoundException)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            catch (ProjectAccessDeniedException)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
        }

        /// <summary>
        /// Returns new view for creating project
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Creates new project
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Returns edit view for project. It shows project name and every collaborator in it and each role.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Changes the name of the project
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Ask user for confirmation before deleting a project
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Deletes a project
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ViewBag.LeftButton = true;
            projectStructure.DeleteById(id);
            return RedirectToAction("Index");
        }
    }
}
