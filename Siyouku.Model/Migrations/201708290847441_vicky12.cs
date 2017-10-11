namespace Siyouku.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class vicky12 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tbPicture",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ImgName = c.String(),
                        CreateTime = c.DateTime(),
                        UpdateTime = c.DateTime(),
                        CreateUser = c.String(),
                        UpdateUser = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.tbPicture");
        }
    }
}
