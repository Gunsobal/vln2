using CodeKingdom.Models.Entities;
using CodeKingdom.Models.ViewModels;
using CodeKingdom.Repositories;
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

        public CollaboratorStructure()
        {
            collaboratorRepository = new CollaboratorRepository();
        }

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
            List<SelectListItem> roleList = CreateRoleSelectListForViewModel(viewModel);
            viewModel.Roles = roleList;
            return viewModel;
        }

        public Collaborator GetCollaboratorById(int id)
        {
            return collaboratorRepository.GetById(id);
        }

        public List<SelectListItem> CreateRoleSelectListForViewModel(CollaboratorViewModel viewModel)
        {
            List<CollaboratorRole> roles = collaboratorRepository.GetAllRoles();
            List<SelectListItem> roleList = new List<SelectListItem>();
            foreach(var role in roles)
            {
                roleList.Add(new SelectListItem { Value = role.ID.ToString(), Text = role.Name });
            }
            return roleList;
        }

        public void Update(CollaboratorViewModel viewModel)
        {
            collaboratorRepository.Update(viewModel);
        }

        public void Delete(int id)
        {
            collaboratorRepository.Delete(id);
        }

    }
}