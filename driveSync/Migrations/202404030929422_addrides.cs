namespace driveSync.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addrides : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Rides", "BagQuantity", c => c.String());
            AddColumn("dbo.Rides", "BagWeight", c => c.String());
            AddColumn("dbo.Rides", "BagSize", c => c.String());
            AddColumn("dbo.Rides", "LuggageQuantity", c => c.String());
            AddColumn("dbo.Rides", "LuggageWeight", c => c.String());
            AddColumn("dbo.Rides", "LuggageSize", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Rides", "LuggageSize");
            DropColumn("dbo.Rides", "LuggageWeight");
            DropColumn("dbo.Rides", "LuggageQuantity");
            DropColumn("dbo.Rides", "BagSize");
            DropColumn("dbo.Rides", "BagWeight");
            DropColumn("dbo.Rides", "BagQuantity");
        }
    }
}
