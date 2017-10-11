namespace Siyouku.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class vicky9 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbUser", "Mobile", c => c.String(maxLength: 18));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tbUser", "Mobile");
        }
    }
}
