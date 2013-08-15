namespace Cognition.Documents.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DynamicTypeDefinitions",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Tenant = c.String(),
                        Name = c.String(),
                        Code = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.DynamicTypeDefinitions");
        }
    }
}
