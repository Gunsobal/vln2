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
    [Authorize]
    public class FileController : Controller
    {
        private FileRepository repository = new FileRepository();
        private FolderRepository folderRepository = new FolderRepository();
        private ProjectRepository projectRepository = new ProjectRepository();

        /// <summary>
        /// Prepares Viewmodel for creating new file
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Create(int? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Project project = projectRepository.getById(id.Value);

            if (project == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            FileViewModel model = new FileViewModel
            {
                ProjectID = project.ID,
                Folders = GetFolders(project.Root.ID)
            };

            return View(model);
        }

        /// <summary>
        ///  Creates new file for project
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ProjectID,FolderID,Name,Type")] FileViewModel file)
        {
            if (ModelState.IsValid)
            {
                File newfile = repository.Create(file);
                return RedirectToAction("Details", "Project", new { id = file.ProjectID, fileID = newfile.ID});
            }

            return View(file);
        }

        /// <summary>
        /// Prepares and returns edit view
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

            Project project = projectRepository.getById(id.Value);

            if (project == null)
            {
                return HttpNotFound();
            }

            FileViewModel model = new FileViewModel
            {
                ID = file.ID,
                Name = file.Name,
                ProjectID = project.ID,
                Type = file.Type,
                Folders = GetFolders(project.Root.ID)
            };

            return View(model);
        }

        /// <summary>
        /// Edits name and type of file
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Asks for confirmation before deleting a file
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Deletes a file from project
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            repository.DeleteById(id);
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Deletes file with ajax
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DeleteFile(int id)
        {
            File file = repository.GetById(id);
            Folder root = folderRepository.GetRoot(file.FolderID);
            Project project = projectRepository.GetByRootId(root.ID);
            repository.DeleteById(id);
            List<File> files = repository.GetByFolderId(root.ID);
            List<int> fileIDs = new List<int>();
            List<string> fileNames = new List<string>();
            foreach (File f in files)
            {
                fileIDs.Add(f.ID);
                fileNames.Add(f.Name);
            }

            return Json(new { ProjectID = project.ID, FileIDs = fileIDs, FileNames = fileNames }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Renames file with ajax
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult RenameFile(FileViewModel file)
        {
            File newFile = repository.Rename(file);
            return Json(new { Name = newFile.Name, ID = newFile.ID }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Saves file, used with Signal R
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Helper function for creating select list for dropdown
        /// </summary>
        /// <param name="rootID"></param>
        /// <returns></returns>
        private List<SelectListItem> GetFolders(int rootID)
        {
            List<Folder> folders = new List<Folder>();
            folders.AddRange(folderRepository.GetCascadingChildrenById(rootID));
            List<SelectListItem> folderList = new List<SelectListItem>();

            foreach (var folder in folders)
            {
                folderList.Add(new SelectListItem { Value = folder.ID.ToString(), Text = folder.Name });
            }

            return folderList;
        }
    }
}
