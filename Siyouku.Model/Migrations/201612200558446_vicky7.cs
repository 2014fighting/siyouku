namespace Siyouku.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class vicky7 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Tags", "Article_Id", "dbo.tbArticle");
            DropIndex("dbo.Tags", new[] { "Article_Id" });
            CreateTable(
                "dbo.TagArticles",
                c => new
                    {
                        Tag_Id = c.Int(nullable: false),
                        Article_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Tag_Id, t.Article_Id })
                .ForeignKey("dbo.Tags", t => t.Tag_Id, cascadeDelete: true)
                .ForeignKey("dbo.tbArticle", t => t.Article_Id, cascadeDelete: true)
                .Index(t => t.Tag_Id)
                .Index(t => t.Article_Id);
            
            AddColumn("dbo.Tags", "CreateTime", c => c.DateTime());
            AddColumn("dbo.Tags", "UpdateTime", c => c.DateTime());
            DropColumn("dbo.Tags", "Article_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Tags", "Article_Id", c => c.Int());
            DropForeignKey("dbo.TagArticles", "Article_Id", "dbo.tbArticle");
            DropForeignKey("dbo.TagArticles", "Tag_Id", "dbo.Tags");
            DropIndex("dbo.TagArticles", new[] { "Article_Id" });
            DropIndex("dbo.TagArticles", new[] { "Tag_Id" });
            DropColumn("dbo.Tags", "UpdateTime");
            DropColumn("dbo.Tags", "CreateTime");
            DropTable("dbo.TagArticles");
            CreateIndex("dbo.Tags", "Article_Id");
            AddForeignKey("dbo.Tags", "Article_Id", "dbo.tbArticle", "Id");
        }
    }
}
