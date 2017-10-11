namespace Siyouku.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class vicky2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.tbArticle", "PublishTime", c => c.DateTime());
            AlterColumn("dbo.tbArticle", "LastMdifyTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.tbArticle", "LastMdifyTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.tbArticle", "PublishTime", c => c.DateTime(nullable: false));
        }
    }
}
