namespace Siyouku.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class vicky11 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.tbArticle", "Title", c => c.String(maxLength: 360));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.tbArticle", "Title", c => c.String(maxLength: 36));
        }
    }
}
