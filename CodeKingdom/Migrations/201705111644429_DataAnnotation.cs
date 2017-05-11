namespace CodeKingdom.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataAnnotation : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Chats", "ApplicationUserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.Collaborators", "ApplicationUserID", "dbo.AspNetUsers");
            DropIndex("dbo.Chats", new[] { "ApplicationUserID" });
            DropIndex("dbo.Collaborators", new[] { "ApplicationUserID" });
            AlterColumn("dbo.Chats", "Message", c => c.String(nullable: false));
            AlterColumn("dbo.Chats", "ApplicationUserID", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Projects", "Name", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Collaborators", "ApplicationUserID", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.CollaboratorRoles", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Folders", "Name", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Files", "Name", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Files", "Type", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.UserConfigurations", "KeyBinding", c => c.String(nullable: false));
            AlterColumn("dbo.UserConfigurations", "ColorScheme", c => c.String(nullable: false));
            CreateIndex("dbo.Chats", "ApplicationUserID");
            CreateIndex("dbo.Collaborators", "ApplicationUserID");
            AddForeignKey("dbo.Chats", "ApplicationUserID", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Collaborators", "ApplicationUserID", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Collaborators", "ApplicationUserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.Chats", "ApplicationUserID", "dbo.AspNetUsers");
            DropIndex("dbo.Collaborators", new[] { "ApplicationUserID" });
            DropIndex("dbo.Chats", new[] { "ApplicationUserID" });
            AlterColumn("dbo.UserConfigurations", "ColorScheme", c => c.String());
            AlterColumn("dbo.UserConfigurations", "KeyBinding", c => c.String());
            AlterColumn("dbo.Files", "Type", c => c.String());
            AlterColumn("dbo.Files", "Name", c => c.String());
            AlterColumn("dbo.Folders", "Name", c => c.String());
            AlterColumn("dbo.CollaboratorRoles", "Name", c => c.String());
            AlterColumn("dbo.Collaborators", "ApplicationUserID", c => c.String(maxLength: 128));
            AlterColumn("dbo.Projects", "Name", c => c.String());
            AlterColumn("dbo.Chats", "ApplicationUserID", c => c.String(maxLength: 128));
            AlterColumn("dbo.Chats", "Message", c => c.String());
            CreateIndex("dbo.Collaborators", "ApplicationUserID");
            CreateIndex("dbo.Chats", "ApplicationUserID");
            AddForeignKey("dbo.Collaborators", "ApplicationUserID", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Chats", "ApplicationUserID", "dbo.AspNetUsers", "Id");
        }
    }
}
