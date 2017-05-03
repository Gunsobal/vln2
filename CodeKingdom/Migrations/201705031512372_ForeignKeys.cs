namespace CodeKingdom.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ForeignKeys : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Chats", "Project_ID", "dbo.Projects");
            DropForeignKey("dbo.Collaborators", "Project_ID", "dbo.Projects");
            DropForeignKey("dbo.Projects", "Root_ID", "dbo.Folders");
            DropForeignKey("dbo.Collaborators", "Role_ID", "dbo.CollaboratorRoles");
            DropForeignKey("dbo.Files", "Folder_ID", "dbo.Folders");
            DropIndex("dbo.Chats", new[] { "Project_ID" });
            DropIndex("dbo.Projects", new[] { "Root_ID" });
            DropIndex("dbo.Collaborators", new[] { "Project_ID" });
            DropIndex("dbo.Collaborators", new[] { "Role_ID" });
            DropIndex("dbo.Files", new[] { "Folder_ID" });
            RenameColumn(table: "dbo.Chats", name: "Project_ID", newName: "ProjectID");
            RenameColumn(table: "dbo.Collaborators", name: "Project_ID", newName: "ProjectID");
            RenameColumn(table: "dbo.Projects", name: "Root_ID", newName: "FolderID");
            RenameColumn(table: "dbo.Collaborators", name: "Role_ID", newName: "CollaboratorRoleID");
            RenameColumn(table: "dbo.Collaborators", name: "User_Id", newName: "ApplicationUserID");
            RenameColumn(table: "dbo.Files", name: "Folder_ID", newName: "FolderID");
            RenameColumn(table: "dbo.Folders", name: "Parent_ID", newName: "FolderID");
            RenameColumn(table: "dbo.Files", name: "Owner_Id", newName: "ApplicationUserID");
            RenameIndex(table: "dbo.Collaborators", name: "IX_User_Id", newName: "IX_ApplicationUserID");
            RenameIndex(table: "dbo.Folders", name: "IX_Parent_ID", newName: "IX_FolderID");
            RenameIndex(table: "dbo.Files", name: "IX_Owner_Id", newName: "IX_ApplicationUserID");
            AddColumn("dbo.Chats", "ApplicationUserID", c => c.Int(nullable: false));
            AddColumn("dbo.UserConfigurations", "AppicationUserID", c => c.String());
            AlterColumn("dbo.Chats", "ProjectID", c => c.Int(nullable: false));
            AlterColumn("dbo.Projects", "FolderID", c => c.Int(nullable: false));
            AlterColumn("dbo.Collaborators", "ProjectID", c => c.Int(nullable: false));
            AlterColumn("dbo.Collaborators", "CollaboratorRoleID", c => c.Int(nullable: false));
            AlterColumn("dbo.Files", "FolderID", c => c.Int(nullable: false));
            CreateIndex("dbo.Chats", "ProjectID");
            CreateIndex("dbo.Projects", "FolderID");
            CreateIndex("dbo.Collaborators", "ProjectID");
            CreateIndex("dbo.Collaborators", "CollaboratorRoleID");
            CreateIndex("dbo.Files", "FolderID");
            AddForeignKey("dbo.Chats", "ProjectID", "dbo.Projects", "ID", cascadeDelete: true);
            AddForeignKey("dbo.Collaborators", "ProjectID", "dbo.Projects", "ID", cascadeDelete: true);
            AddForeignKey("dbo.Projects", "FolderID", "dbo.Folders", "ID", cascadeDelete: true);
            AddForeignKey("dbo.Collaborators", "CollaboratorRoleID", "dbo.CollaboratorRoles", "ID", cascadeDelete: true);
            AddForeignKey("dbo.Files", "FolderID", "dbo.Folders", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Files", "FolderID", "dbo.Folders");
            DropForeignKey("dbo.Collaborators", "CollaboratorRoleID", "dbo.CollaboratorRoles");
            DropForeignKey("dbo.Projects", "FolderID", "dbo.Folders");
            DropForeignKey("dbo.Collaborators", "ProjectID", "dbo.Projects");
            DropForeignKey("dbo.Chats", "ProjectID", "dbo.Projects");
            DropIndex("dbo.Files", new[] { "FolderID" });
            DropIndex("dbo.Collaborators", new[] { "CollaboratorRoleID" });
            DropIndex("dbo.Collaborators", new[] { "ProjectID" });
            DropIndex("dbo.Projects", new[] { "FolderID" });
            DropIndex("dbo.Chats", new[] { "ProjectID" });
            AlterColumn("dbo.Files", "FolderID", c => c.Int());
            AlterColumn("dbo.Collaborators", "CollaboratorRoleID", c => c.Int());
            AlterColumn("dbo.Collaborators", "ProjectID", c => c.Int());
            AlterColumn("dbo.Projects", "FolderID", c => c.Int());
            AlterColumn("dbo.Chats", "ProjectID", c => c.Int());
            DropColumn("dbo.UserConfigurations", "AppicationUserID");
            DropColumn("dbo.Chats", "ApplicationUserID");
            RenameIndex(table: "dbo.Files", name: "IX_ApplicationUserID", newName: "IX_Owner_Id");
            RenameIndex(table: "dbo.Folders", name: "IX_FolderID", newName: "IX_Parent_ID");
            RenameIndex(table: "dbo.Collaborators", name: "IX_ApplicationUserID", newName: "IX_User_Id");
            RenameColumn(table: "dbo.Files", name: "ApplicationUserID", newName: "Owner_Id");
            RenameColumn(table: "dbo.Folders", name: "FolderID", newName: "Parent_ID");
            RenameColumn(table: "dbo.Files", name: "FolderID", newName: "Folder_ID");
            RenameColumn(table: "dbo.Collaborators", name: "ApplicationUserID", newName: "User_Id");
            RenameColumn(table: "dbo.Collaborators", name: "CollaboratorRoleID", newName: "Role_ID");
            RenameColumn(table: "dbo.Projects", name: "FolderID", newName: "Root_ID");
            RenameColumn(table: "dbo.Collaborators", name: "ProjectID", newName: "Project_ID");
            RenameColumn(table: "dbo.Chats", name: "ProjectID", newName: "Project_ID");
            CreateIndex("dbo.Files", "Folder_ID");
            CreateIndex("dbo.Collaborators", "Role_ID");
            CreateIndex("dbo.Collaborators", "Project_ID");
            CreateIndex("dbo.Projects", "Root_ID");
            CreateIndex("dbo.Chats", "Project_ID");
            AddForeignKey("dbo.Files", "Folder_ID", "dbo.Folders", "ID");
            AddForeignKey("dbo.Collaborators", "Role_ID", "dbo.CollaboratorRoles", "ID");
            AddForeignKey("dbo.Projects", "Root_ID", "dbo.Folders", "ID");
            AddForeignKey("dbo.Collaborators", "Project_ID", "dbo.Projects", "ID");
            AddForeignKey("dbo.Chats", "Project_ID", "dbo.Projects", "ID");
        }
    }
}
