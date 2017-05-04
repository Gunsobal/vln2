namespace CodeKingdom.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ForeignKeyOnChatUser : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Chats", new[] { "User_Id" });
            DropColumn("dbo.Chats", "ApplicationUserID");
            RenameColumn(table: "dbo.Chats", name: "User_Id", newName: "ApplicationUserID");
            AlterColumn("dbo.Chats", "ApplicationUserID", c => c.String(maxLength: 128));
            CreateIndex("dbo.Chats", "ApplicationUserID");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Chats", new[] { "ApplicationUserID" });
            AlterColumn("dbo.Chats", "ApplicationUserID", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.Chats", name: "ApplicationUserID", newName: "User_Id");
            AddColumn("dbo.Chats", "ApplicationUserID", c => c.Int(nullable: false));
            CreateIndex("dbo.Chats", "User_Id");
        }
    }
}
