using CodeKingdom.Models;
using CodeKingdom.Models.Entities;
using CodeKingdom.Models.ViewModels;
using CodeKingdom.Repositories;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CodeKingdom.Business
{
    public class CollaboratorStructure
    {
        private CollaboratorRepository collaboratorRepository;
        private UserRepository userRepository;

        public CollaboratorStructure()
        {
            collaboratorRepository = new CollaboratorRepository();
            userRepository = new UserRepository();
        }

        /// <summary>
        /// Returns collaborator view model by collaborator ID
        /// </summary>
        /// <param name="id">Collaborator ID</param>
        public CollaboratorViewModel CreateCollaboratorViewModelFromID(int id)
        {
            Collaborator collaborator = GetCollaboratorById(id);
            CollaboratorViewModel viewModel = new CollaboratorViewModel
            {
                ID = collaborator.ID,
                UserName = collaborator.User.UserName,
                RoleName = collaborator.Role.Name,
                RoleID = collaborator.CollaboratorRoleID,
                ProjectID = collaborator.ProjectID,
            };
            List<SelectListItem> roleList = CreateRoleSelectListForViewModel();
            viewModel.Roles = roleList;
            return viewModel;
        }

        /// <summary>
        /// Returns collaborator view model from project and user ID or null if collaborator doesn't exist
        /// </summary>
        /// <param name="projectID">Project ID</param>
        /// <param name="userID">User ID</param>
        public CollaboratorViewModel CreateCollaboratorViewModelFromProjectAndUserID(int projectID, string userID)
        {
            Collaborator collaborator = collaboratorRepository.GetByUserIdAndProjectId(userID, projectID);
            if (collaborator == null)
            {
                return null;
            }
            CollaboratorViewModel viewModel = new CollaboratorViewModel
            {
                ID = collaborator.ID,
                UserName = collaborator.User.UserName,
                RoleName = collaborator.Role.Name,
                RoleID = collaborator.CollaboratorRoleID,
                ProjectID = collaborator.ProjectID
            };
            List<SelectListItem> roleList = CreateRoleSelectListForViewModel();
            viewModel.Roles = roleList;
            return viewModel;
        }

        /// <summary>
        /// Returns null or single instance collaborator by collaborator id
        /// </summary>
        /// <param name="id">Collaborator ID</param>
        public Collaborator GetCollaboratorById(int id)
        {
            return collaboratorRepository.GetById(id);
        }

        /// <summary>
        /// Returns roles as list of select list items
        /// </summary>
        public List<SelectListItem> CreateRoleSelectListForViewModel()
        {
            List<CollaboratorRole> roles = collaboratorRepository.GetAllRoles();
            List<SelectListItem> roleList = new List<SelectListItem>();
            foreach(var role in roles)
            {
                roleList.Add(new SelectListItem { Value = role.ID.ToString(), Text = role.Name });
            }
            return roleList;
        }

        /// <summary>
        /// Updates collaborator by collaborator view model
        /// </summary>
        public void Update(CollaboratorViewModel viewModel)
        {
            collaboratorRepository.Update(viewModel);
        }

        /// <summary>
        /// Delete collaborator by collaborator ID
        /// </summary>
        /// <param name="id">Collaborator ID</param>
        public void Delete(int id)
        {
            collaboratorRepository.DeleteById(id);
        }

        /// <summary>
        /// Creates a collaborator entry in database from collaborator view model. Returns true if successful, false otherwise
        /// </summary>
        /// <param name="viewModel"></param>
        public bool Create(CollaboratorViewModel viewModel)
        {
            ApplicationUser user = userRepository.GetByUserName(viewModel.UserName);
            if (user == null)
            {
                return false;
            }
            Collaborator collaborator = new Collaborator
            {
                ApplicationUserID = user.Id,
                ProjectID = viewModel.ProjectID,
                CollaboratorRoleID = viewModel.RoleID
            };
            return collaboratorRepository.Create(collaborator);
        }
    }
}