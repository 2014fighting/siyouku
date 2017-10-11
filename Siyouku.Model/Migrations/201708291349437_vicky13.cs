namespace Siyouku.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class vicky13 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbPicture", "Md5Value", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tbPicture", "Md5Value");
        }
    }
}
