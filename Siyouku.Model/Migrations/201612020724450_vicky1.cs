namespace Siyouku.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class vicky1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbArticle", "Img", c => c.String(maxLength: 300));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tbArticle", "Img");
        }
    }
}
