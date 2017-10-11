namespace Siyouku.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class vicky10 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tbApplet",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CategoryName = c.String(),
                        Name = c.String(),
                        SaomiaoUrl = c.String(),
                        Summary = c.String(),
                        LogoUrl = c.String(),
                        SortNum = c.Double(nullable: false),
                        Content = c.String(),
                        CreateTime = c.DateTime(),
                        UpdateTime = c.DateTime(),
                        CreateUser = c.String(),
                        UpdateUser = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.tbApplet");
        }
    }
}
