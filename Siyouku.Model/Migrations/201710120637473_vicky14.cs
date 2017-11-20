namespace Siyouku.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class vicky14 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Links",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LinkName = c.String(maxLength: 200),
                        LinkUrl = c.String(maxLength: 500),
                        LinkImg = c.String(maxLength: 500),
                        LinkSort = c.Int(nullable: false),
                        CreateTime = c.DateTime(),
                        UpdateTime = c.DateTime(),
                        CreateUser = c.String(),
                        UpdateUser = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Links");
        }
    }
}
