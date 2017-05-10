namespace CodeKingdom.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProjectIDInFile : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Files", "ProjectID", c => c.Int());
            CreateIndex("dbo.Files", "ProjectID");
            AddForeignKey("dbo.Files", "ProjectID", "dbo.Projects", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Files", "ProjectID", "dbo.Projects");
            DropIndex("dbo.Files", new[] { "ProjectID" });
            DropColumn("dbo.Files", "ProjectID");
        }
    }
}
