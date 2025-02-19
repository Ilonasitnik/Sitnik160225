namespace Sitnik160225.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateToDoModel1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ToDoes", "DueDate", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ToDoes", "DueDate", c => c.DateTime(nullable: false));
        }
    }
}
