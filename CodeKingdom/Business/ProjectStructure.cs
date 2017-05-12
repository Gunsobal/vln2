using CodeKingdom.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using CodeKingdom.Models.ViewModels;
using CodeKingdom.Models.Entities;
using System.Web.Mvc;
using CodeKingdom.Exceptions;

namespace CodeKingdom.Business
{
    public class ProjectStructure
    {
        private ProjectRepository projectRepository;
        private FolderRepository folderRepository;
        private FileRepository fileRepository;
        private CollaboratorRepository collaboratorRepository;
        private ChatRepository chatRepository;
        private UserRepository userRepository;

        public ProjectStructure()
        {
            projectRepository = new ProjectRepository();
            folderRepository = new FolderRepository();
            fileRepository = new FileRepository();
            collaboratorRepository = new CollaboratorRepository();
            chatRepository = new ChatRepository();
            userRepository = new UserRepository();
        }

        /// <summary>
        /// Helper function to get user ID string
        /// </summary>
        public string GetUserId()
        {
            return HttpContext.Current.User.Identity.GetUserId();
        }

        /// <summary>
        /// Returns project from project repository by id
        /// </summary>
        /// <param name="id">Project ID</param>
        public Project GetProject(int id)
        {
            return projectRepository.GetById(id);
        }

        /// <summary>
        /// Returns list of projects by current user ID
        /// </summary>
        public List<Project> GetListOfProjects()
        {
            return projectRepository.GetByUserId(GetUserId());
        }
        
        /// <summary>
        /// Returns list of a collaborator view models for a specific project
        /// </summary>
        /// <param name="project">Project</param>
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

        /// <summary>
        /// Convert list of projects to list of project view models
        /// </summary>
        /// <param name="projects">Project list</param>
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

        /// <summary>
        /// Build editor view model from project ID and optional file ID
        /// </summary>
        /// <param name="projectId">Project ID</param>
        /// <param name="fileId">File ID</param>
        public EditorViewModel GetEditorViewModel(int projectId, int? fileId = null)
        {
            Project project = GetProject(projectId);

            if (project == null)
            {
                throw new ProjectNotFoundException();
            }

            int folderID = project.FolderID;
            File file = null;

            // If file id is specified, find the specific file
            if (fileId.HasValue)
            {
                file = fileRepository.GetByFileInProject(fileId.Value, projectId);
            }
            
            // If file was not found or not specified, find default root file for project
            if (file == null || !fileId.HasValue)
            {
                file = project.Root.Files.FirstOrDefault();
            }

            // If root file was missing
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
                    Type = "javascript",
                    ApplicationUserID = GetUserId(),
                    ProjectID = projectId,
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

            Folder root = folderRepository.GetById(folderID);

            EditorViewModel viewModel = new EditorViewModel
            {
                Name = project.Name,
                ProjectID = project.ID,
                FileID = file.ID,
                Type = file.Type,
                Content = file.Content,
                Collaborators = project.Collaborators,
                Root = root,
                Chats = chatViewModels
            };

            return viewModel;
        }

        /// <summary>
        /// Create project from project view model
        /// </summary>
        /// <param name="viewModel">Project View Model</param>
        public void CreateProject(ProjectViewModel viewModel)
        {
            viewModel.ApplicationUserID = GetUserId();
            projectRepository.Create(viewModel);
        }
        
        /// <summary>
        /// Create project view model from project
        /// </summary>
        /// <param name="project">Project</param>
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

        /// <summary>
        /// Update project by project view model
        /// </summary>
        /// <param name="viewModel">Project View Model</param>
        public void Update(ProjectViewModel viewModel)
        {
            viewModel.ApplicationUserID = GetUserId();
            projectRepository.Update(viewModel);
        }

        /// <summary>
        /// Delete project and project root folder
        /// </summary>
        /// <param name="id">Project ID</param>
        public void DeleteById(int id)
        {
            Project project = projectRepository.GetById(id);
            if (project != null)
            {
                int root = project.ID;

                projectRepository.DeleteById(id);
                folderRepository.DeleteById(root);
            }
        }
       
        /// <summary>
        /// Returns file by file and project ID
        /// </summary>
        /// <param name="id">File ID</param>
        /// <param name="projectId">Project ID</param>
        public File GetFileByID(int id, int projectId)
        {
            return fileRepository.GetByFileInProject(id, projectId);
        }

        /// <summary>
        /// Clears chat entries for a specific project
        /// </summary>
        /// <param name="id">Project ID</param>
        public void ClearChatForProject(int id)
        {
            chatRepository.ClearChat(id);
        }

        /// <summary>
        /// Returns color scheme configuration for a specific user
        /// </summary>
        /// <param name="id">User ID</param>
        public string GetColorscheme(string id)
        {
            return userRepository.GetColorScheme(id);
        }

        /// <summary>
        /// Returns key binding configuration for a specific user
        /// </summary>
        /// <param name="id">User ID</param>
        public string GetKeyBinding(string id)
        {
            return userRepository.GetKeyBinding(id);
        }
    }
}