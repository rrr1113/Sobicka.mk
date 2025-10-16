namespace Booking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class proba1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Reservations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Surname = c.String(nullable: false),
                        DateOfReservation = c.DateTime(nullable: false),
                        EndOfReservation = c.DateTime(nullable: false),
                        RoomId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Rooms", t => t.RoomId, cascadeDelete: true)
                .Index(t => t.RoomId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Reservations", "RoomId", "dbo.Rooms");
            DropIndex("dbo.Reservations", new[] { "RoomId" });
            DropTable("dbo.Reservations");
        }
    }
}
