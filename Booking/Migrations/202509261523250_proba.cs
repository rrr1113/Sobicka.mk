namespace Booking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class proba : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Hotels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        ImageURL = c.String(),
                        Address = c.String(nullable: false),
                        PhoneNumber = c.String(),
                        TownId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Towns", t => t.TownId, cascadeDelete: true)
                .Index(t => t.TownId);
            
            CreateTable(
                "dbo.Rooms",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Number = c.Int(nullable: false),
                        Capacity = c.Int(nullable: false),
                        ImageURL = c.String(),
                        Price = c.Int(nullable: false),
                        Status = c.String(),
                        HotelId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Hotels", t => t.HotelId, cascadeDelete: true)
                .Index(t => t.HotelId);
            
            CreateTable(
                "dbo.Towns",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Country = c.String(nullable: false),
                        NumberOfHotels = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Hotels", "TownId", "dbo.Towns");
            DropForeignKey("dbo.Rooms", "HotelId", "dbo.Hotels");
            DropIndex("dbo.Rooms", new[] { "HotelId" });
            DropIndex("dbo.Hotels", new[] { "TownId" });
            DropTable("dbo.Towns");
            DropTable("dbo.Rooms");
            DropTable("dbo.Hotels");
        }
    }
}
