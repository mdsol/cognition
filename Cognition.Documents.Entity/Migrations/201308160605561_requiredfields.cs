namespace Cognition.Documents.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class requiredfields : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.DynamicTypeDefinitions", "Tenant", c => c.String(nullable: false));
            AlterColumn("dbo.DynamicTypeDefinitions", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.DynamicTypeDefinitions", "Code", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.DynamicTypeDefinitions", "Code", c => c.String());
            AlterColumn("dbo.DynamicTypeDefinitions", "Name", c => c.String());
            AlterColumn("dbo.DynamicTypeDefinitions", "Tenant", c => c.String());
        }
    }
}
