using CodeKingdom.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using CodeKingdom.Models.ViewModels;
using CodeKingdom.Models.Entities;
using System.Web.Mvc;

namespace CodeKingdom.Business
{
    public class ProjectStructure
    {
        private ProjectRepository projectRepository;
        private FolderRepository folderRepository;
        private FileRepository fileRepository;
        private CollaboratorRepository collaboratorRepository;
        private ChatRepository chatRepository;

        public ProjectStructure()
        {
            projectRepository = new ProjectRepository();
            folderRepository = new FolderRepository();
            fileRepository = new FileRepository();
            collaboratorRepository = new CollaboratorRepository();
            chatRepository = new ChatRepository();
        }

        public string GetUserId()
        {
            return HttpContext.Current.User.Identity.GetUserId();
        }

        public Project GetProject(int id)
        {
            return projectRepository.getById(id);
        }

        public List<Project> GetListOfProjects()
        {
            return projectRepository.getByUserId(GetUserId());
        }
        
        public List<CollaboratorViewModel> GetListOfCollaboratorViewModels(Project project)
        {
            List<CollaboratorViewModel> collabViewModels = new List<CollaboratorViewModel>();
            foreach (var collaborator in project.Collaborators)
            {
                List<CollaboratorRole> roles = collaboratorRepository.GetAllRoles();
                List<SelectListItem> roleList = new List<SelectListItem>();
                foreach (var role in roles)
                {
                    roleList.Add(new SelectListItem() { Value = role.ID.ToString(), Text = role.Name });
                }

                collabViewModels.Add(
                    new CollaboratorViewModel
                    {
                        ID = collaborator.ID,
                        ProjectID = collaborator.ProjectID,
                        RoleID = collaborator.CollaboratorRoleID,
                        UserName = collaborator.User.UserName,
                        RoleName = collaborator.Role.Name,
                        Roles = roleList
                    }
                );
            }
            return collabViewModels;
        }

        public List<ProjectViewModel> GetListOfProjectViewModels(List<Project> projects)
        {
            List<ProjectViewModel> viewModels = new List<ProjectViewModel>();

            foreach (var project in projects)
            {
                string userID = GetUserId();
                Collaborator collaborator = collaboratorRepository.GetByUserIdAndProjectId(userID, project.ID);
                List<CollaboratorViewModel> collabViewModels = GetListOfCollaboratorViewModels(project); 
                viewModels.Add(
                    new ProjectViewModel
                    {
                        ID = project.ID,
                        Name = project.Name,
                        Collaborators = collabViewModels,
                        Role = collaborator.Role.Name
                    }
                );
            }
            return viewModels;
        }

        public EditorViewModel GetEditorViewModel(int projectId, int? fileId = null)
        {
            Project project = GetProject(projectId);
            int folderID = project.FolderID;
            File file = null;

            if (fileId.HasValue)
            {
                //file = project.Root.Files.Where(f => f.ID == fileId).FirstOrDefault();
                file = fileRepository.GetByFileInProject(fileId.Value, projectId);
            }
            
            if ((fileId.HasValue && file == null) || !fileId.HasValue)
            {
                file = project.Root.Files.FirstOrDefault();
            }

            if (file == null)
            {
                // Project does not contain any files in the root of the project. 
                // We have to create a default one the solution does not throw 
                // yellow screen of death
                FileViewModel fileViewModel = new FileViewModel
                {
                    Name = "index.js",
                    FolderID = folderID,
                    Content = "",
                    Type = "Javascript",
                    ApplicationUserID = GetUserId(),
                };

                file = fileRepository.Create(fileViewModel);
            }
            List<Chat> chats = chatRepository.GetByProjectId(projectId);
            List<ChatViewModel> chatViewModels = new List<ChatViewModel>();
            foreach(var chat in chats)
            {
                chatViewModels.Add(new ChatViewModel
                {
                    Message = chat.Message,
                    ProjectID = chat.ProjectID,
                    Username = chat.User.UserName,
                    DateTime = chat.DateTime
                });
            }

            // List<File> files = new List<File>();
            List<File> files = fileRepository.GetByFolderId(folderID);
            List<Folder> folders = folderRepository.GetChildrenById(folderID);
            foreach (Folder folder in folders)
            {
                files.AddRange(fileRepository.GetByFolderId(folder.ID));
            }

            EditorViewModel viewModel = new EditorViewModel
            {
                Name = project.Name,
                ProjectID = project.ID,
                FileID = file.ID,
                Content = file.Content,
                Collaborators = project.Collaborators,
                Folders = folders,
                Files = files,
                Chats = chatViewModels
            };

            return viewModel;
        }

        public void CreateProject(ProjectViewModel viewModel)
        {
            viewModel.ApplicationUserID = GetUserId();
            projectRepository.Create(viewModel);
        }
        
        public ProjectViewModel CreateProjectViewModel(Project project)
        {
            List<CollaboratorViewModel> collaborators = GetListOfCollaboratorViewModels(project);
            Collaborator collaborator = collaboratorRepository.GetByUserIdAndProjectId(GetUserId(), project.ID);
            ProjectViewModel viewModel = new ProjectViewModel
            {
                ID = project.ID,
                Name = project.Name,
                Role = collaborator.Role.Name,
                Collaborators = collaborators
            };
            return viewModel;
        }

        public void Update(ProjectViewModel viewModel)
        {
            viewModel.ApplicationUserID = GetUserId();
            projectRepository.Update(viewModel);
        }

        public void DeleteById(int id)
        {
            projectRepository.DeleteById(id);
        }
       
        public File GetFileByID(int id, int projectId)
        {
            return fileRepository.GetByFileInProject(id, projectId);
        }
    }
}