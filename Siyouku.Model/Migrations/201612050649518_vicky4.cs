namespace Siyouku.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class vicky4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbArticle", "CollectTime", c => c.String());
            DropColumn("dbo.tbArticle", "Url");
        }
        
        public override void Down()
        {
            AddColumn("dbo.tbArticle", "Url", c => c.String());
            DropColumn("dbo.tbArticle", "CollectTime");
        }
    }
}
