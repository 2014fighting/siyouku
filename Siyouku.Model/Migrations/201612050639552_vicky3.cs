namespace Siyouku.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class vicky3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbArticle", "CollectUser", c => c.String());
            AddColumn("dbo.tbArticle", "Summary", c => c.String());
            AddColumn("dbo.tbArticle", "Pviews", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tbArticle", "Pviews");
            DropColumn("dbo.tbArticle", "Summary");
            DropColumn("dbo.tbArticle", "CollectUser");
        }
    }
}
