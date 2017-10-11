namespace Siyouku.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class vicky : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tbArticle",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CollectId = c.String(),
                        Url = c.String(),
                        Title = c.String(maxLength: 36),
                        Content = c.String(),
                        CategoryId = c.String(maxLength: 36),
                        PublishTime = c.DateTime(nullable: false),
                        LastMdifyTime = c.DateTime(nullable: false),
                        UserId = c.String(maxLength: 36),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tbUser", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.tbUser",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 36),
                        UserName = c.String(maxLength: 18),
                        PassWord = c.String(maxLength: 18),
                        BlogName = c.String(maxLength: 36),
                        Decription = c.String(maxLength: 100),
                        Email = c.String(maxLength: 36),
                        RegisterTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tbArticle", "UserId", "dbo.tbUser");
            DropIndex("dbo.tbArticle", new[] { "UserId" });
            DropTable("dbo.tbUser");
            DropTable("dbo.tbArticle");
        }
    }
}
