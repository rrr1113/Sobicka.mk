namespace Booking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class total_pricr : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reservations", "TotalPrice", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Reservations", "TotalPrice");
        }
    }
}
