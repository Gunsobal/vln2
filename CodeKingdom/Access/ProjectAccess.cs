using CodeKingdom.Models;
using CodeKingdom.Models.Entities;
using CodeKingdom.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeKingdom.Access
{
    /// <summary>
    /// Helper class that determinies roles of user in a project.
    /// Could be used to drop request were user does not have permission
    /// to change or invite into a project
    /// </summary>
    public class ProjectAccess
    {
        private readonly CollaboratorRepository collaboratorRepository = new CollaboratorRepository();
        private readonly ProjectRepository projectRepository = new ProjectRepository();
        private readonly Project project;

        public ProjectAccess(int projectID)
        {
            project = projectRepository.getById(projectID);
        }

        public bool IsOwner(string userID)
        {
            if (!IsUser(userID))
            {
                return false;
            }

            Collaborator collaborator = collaboratorRepository.GetByUserIdAndProjectId(userID, project.ID);
            if (!IsCollaborator(collaborator))
            {
                return false;
            }
            return collaborator.Role.Name == "Owner";
        }

        public bool IsMember(string userID)
        {
            if (!IsUser(userID))
            {
                return false;
            }

            Collaborator collaborator = collaboratorRepository.GetByUserIdAndProjectId(userID, project.ID);

            if (!IsCollaborator(collaborator))
            {
                return false;
            }

            return collaborator.Role.Name == "Owner" || collaborator.Role.Name == "Member";
        }

        private bool IsCollaborator(Collaborator collaborator)
        {
            if (collaborator == null)
            {
                return false;
            }
            return true;
        }

        private bool IsUser(string userID)
        {
            if (userID == null)
            {
                return false;
            }
            return true;
        }
    }
}