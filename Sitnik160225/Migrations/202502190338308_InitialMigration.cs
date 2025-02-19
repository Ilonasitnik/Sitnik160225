namespace Sitnik160225.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ToDoes",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Bezeichnung = c.String(),
                        Beschreibung = c.String(),
                        Prioritaet = c.Int(nullable: false),
                        IstAbgeschlossen = c.Boolean(nullable: false),
                        FotoPath = c.String(),
                        DueDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ToDoes");
        }
    }
}
