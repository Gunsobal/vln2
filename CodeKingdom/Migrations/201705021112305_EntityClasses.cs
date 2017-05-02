namespace CodeKingdom.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EntityClasses : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Chats",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Message = c.String(),
                        DateTime = c.DateTime(nullable: false),
                        Project_ID = c.Int(),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Projects", t => t.Project_ID)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.Project_ID)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.Projects",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Frozen = c.Boolean(nullable: false),
                        Root_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Folders", t => t.Root_ID)
                .Index(t => t.Root_ID);
            
            CreateTable(
                "dbo.Collaborators",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Project_ID = c.Int(),
                        Role_ID = c.Int(),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Projects", t => t.Project_ID)
                .ForeignKey("dbo.CollaboratorRoles", t => t.Role_ID)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.Project_ID)
                .Index(t => t.Role_ID)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.CollaboratorRoles",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Folders",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Parent_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Folders", t => t.Parent_ID)
                .Index(t => t.Parent_ID);
            
            CreateTable(
                "dbo.Files",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Content = c.String(),
                        Type = c.String(),
                        Folder_ID = c.Int(),
                        Owner_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Folders", t => t.Folder_ID)
                .ForeignKey("dbo.AspNetUsers", t => t.Owner_Id)
                .Index(t => t.Folder_ID)
                .Index(t => t.Owner_Id);
            
            CreateTable(
                "dbo.UserConfigurations",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        keybinding = c.String(),
                        colorscheme = c.String(),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.User_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserConfigurations", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Chats", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Chats", "Project_ID", "dbo.Projects");
            DropForeignKey("dbo.Projects", "Root_ID", "dbo.Folders");
            DropForeignKey("dbo.Folders", "Parent_ID", "dbo.Folders");
            DropForeignKey("dbo.Files", "Owner_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Files", "Folder_ID", "dbo.Folders");
            DropForeignKey("dbo.Collaborators", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Collaborators", "Role_ID", "dbo.CollaboratorRoles");
            DropForeignKey("dbo.Collaborators", "Project_ID", "dbo.Projects");
            DropIndex("dbo.UserConfigurations", new[] { "User_Id" });
            DropIndex("dbo.Files", new[] { "Owner_Id" });
            DropIndex("dbo.Files", new[] { "Folder_ID" });
            DropIndex("dbo.Folders", new[] { "Parent_ID" });
            DropIndex("dbo.Collaborators", new[] { "User_Id" });
            DropIndex("dbo.Collaborators", new[] { "Role_ID" });
            DropIndex("dbo.Collaborators", new[] { "Project_ID" });
            DropIndex("dbo.Projects", new[] { "Root_ID" });
            DropIndex("dbo.Chats", new[] { "User_Id" });
            DropIndex("dbo.Chats", new[] { "Project_ID" });
            DropTable("dbo.UserConfigurations");
            DropTable("dbo.Files");
            DropTable("dbo.Folders");
            DropTable("dbo.CollaboratorRoles");
            DropTable("dbo.Collaborators");
            DropTable("dbo.Projects");
            DropTable("dbo.Chats");
        }
    }
}
