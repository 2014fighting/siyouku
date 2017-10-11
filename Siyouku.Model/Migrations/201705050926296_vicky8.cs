namespace Siyouku.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class vicky8 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DailyEnglishes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ContentEg = c.String(),
                        ContentCn = c.String(),
                        CreateTime = c.DateTime(),
                        UpdateTime = c.DateTime(),
                        CreateUser = c.String(),
                        UpdateUser = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Tags", "CreateUser", c => c.String());
            AddColumn("dbo.Tags", "UpdateUser", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tags", "UpdateUser");
            DropColumn("dbo.Tags", "CreateUser");
            DropTable("dbo.DailyEnglishes");
        }
    }
}
