namespace Sitnik160225.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateToDoModel : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ToDoes", "Bezeichnung", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ToDoes", "Bezeichnung", c => c.String());
        }
    }
}
