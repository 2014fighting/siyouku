namespace Siyouku.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class vicky15 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.tbArticle", "Pviews", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.tbArticle", "Pviews", c => c.String());
        }
    }
}
