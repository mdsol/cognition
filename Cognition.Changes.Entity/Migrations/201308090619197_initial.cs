namespace Cognition.Changes.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DocumentChangeNotifications",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        DocumentId = c.String(),
                        Type = c.String(),
                        ChangeType = c.Int(nullable: false),
                        Title = c.String(),
                        ByUserId = c.String(),
                        ByUserName = c.String(),
                        DateTime = c.DateTime(nullable: false),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            CreateIndex("dbo.DocumentChangeNotifications", "DateTime");
            
        }
        
        public override void Down()
        {
            DropTable("dbo.DocumentChangeNotifications");
        }
    }
}
