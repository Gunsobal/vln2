using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CodeKingdom.Models.Entities;
using CodeKingdom.Models.ViewModels;
using CodeKingdom.Repositories;

namespace CodeKingdom.Controllers
{
    [Authorize]
    public class FolderController : Controller
    {
        ProjectRepository projectRepository = new ProjectRepository();
        FolderRepository folderRepository = new FolderRepository();
       
        /// <summary>
        /// Prepares viewmodel and returns view for creating new folder
        /// </summary>
        /// <param name="id">Project ID</param>
        public ActionResult Create(int? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Project project = projectRepository.GetById(id.Value);

            if(project == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            List<Folder> folders = folderRepository.GetCascadingChildrenById(project.Root.ID);
            List<SelectListItem> folderList = new List<SelectListItem>();
            
            foreach (var folder in folders)
            {
                folderList.Add(new SelectListItem
                {
                    Value = folder.ID.ToString(), Text = folder.Name
                });
            }

            FolderViewModel viewModel = new FolderViewModel
            {
                ProjectID = project.ID,
                Folders = folderList
            };

            return View(viewModel);
        }

        /// <summary>
        /// Creates new folder
        /// </summary>
        /// <param name="model">F</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProjectID,FolderID,Name")] FolderViewModel model)
        {
            if (ModelState.IsValid)
            {
                Folder folder = new Folder { Name = model.Name, FolderID = model.FolderID };
                folder = folderRepository.Create(folder);
                return RedirectToAction("Details", "Project", new { id = model.ProjectID });
            }

            return View(model);
        }
    }
}
