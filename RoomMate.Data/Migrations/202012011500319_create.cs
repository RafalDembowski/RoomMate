namespace RoomMate.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class create : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Addresses",
                c => new
                    {
                        AddressID = c.Guid(nullable: false),
                        City = c.String(nullable: false, maxLength: 256),
                        Street = c.String(nullable: false, maxLength: 256),
                        Flat = c.String(nullable: false, maxLength: 50),
                        Lon = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Lat = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.AddressID)
                .ForeignKey("dbo.Rooms", t => t.AddressID)
                .Index(t => t.AddressID);
            
            CreateTable(
                "dbo.Rooms",
                c => new
                    {
                        RoomID = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 256),
                        Price = c.Int(nullable: false),
                        Description = c.String(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        NumberOfGuests = c.Int(nullable: false),
                        User_UserID = c.Guid(),
                    })
                .PrimaryKey(t => t.RoomID)
                .ForeignKey("dbo.Users", t => t.User_UserID)
                .Index(t => t.User_UserID);
            
            CreateTable(
                "dbo.Equipments",
                c => new
                    {
                        EquipmentID = c.Guid(nullable: false),
                        IsWifi = c.Boolean(nullable: false),
                        IsAirConditioning = c.Boolean(nullable: false),
                        IsParking = c.Boolean(nullable: false),
                        IsTelevision = c.Boolean(nullable: false),
                        IsKitchen = c.Boolean(nullable: false),
                        IsWashingMachine = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.EquipmentID)
                .ForeignKey("dbo.Rooms", t => t.EquipmentID)
                .Index(t => t.EquipmentID);
            
            CreateTable(
                "dbo.RoomImages",
                c => new
                    {
                        ImageRoomID = c.Guid(nullable: false),
                        Path = c.String(nullable: false, maxLength: 256),
                        Room_RoomID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.ImageRoomID)
                .ForeignKey("dbo.Rooms", t => t.Room_RoomID, cascadeDelete: true)
                .Index(t => t.Room_RoomID);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserID = c.Guid(nullable: false),
                        Email = c.String(nullable: false, maxLength: 256),
                        PasswordHash = c.String(nullable: false, maxLength: 256),
                        UserName = c.String(nullable: false, maxLength: 128),
                        FirsName = c.String(maxLength: 128),
                        LastName = c.String(maxLength: 128),
                        IsEmailVerified = c.Boolean(nullable: false),
                        CodeActivation = c.Guid(nullable: false),
                        CodeResetPassword = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.UserID);
            
            CreateTable(
                "dbo.UserImages",
                c => new
                    {
                        UserImageID = c.Guid(nullable: false),
                        ImageName = c.String(nullable: false, maxLength: 256),
                        ImagePath = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.UserImageID)
                .ForeignKey("dbo.Users", t => t.UserImageID)
                .Index(t => t.UserImageID);
            
            CreateTable(
                "dbo.Bookings",
                c => new
                    {
                        BookingID = c.Guid(nullable: false),
                        InDate = c.DateTime(nullable: false),
                        OutDate = c.DateTime(nullable: false),
                        NumberOfGuests = c.Int(nullable: false),
                        TotalPrice = c.Single(),
                        Room_RoomID = c.Guid(nullable: false),
                        User_UserID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.BookingID)
                .ForeignKey("dbo.Rooms", t => t.Room_RoomID, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.User_UserID, cascadeDelete: true)
                .Index(t => t.Room_RoomID)
                .Index(t => t.User_UserID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Bookings", "User_UserID", "dbo.Users");
            DropForeignKey("dbo.Bookings", "Room_RoomID", "dbo.Rooms");
            DropForeignKey("dbo.Addresses", "AddressID", "dbo.Rooms");
            DropForeignKey("dbo.UserImages", "UserImageID", "dbo.Users");
            DropForeignKey("dbo.Rooms", "User_UserID", "dbo.Users");
            DropForeignKey("dbo.RoomImages", "Room_RoomID", "dbo.Rooms");
            DropForeignKey("dbo.Equipments", "EquipmentID", "dbo.Rooms");
            DropIndex("dbo.Bookings", new[] { "User_UserID" });
            DropIndex("dbo.Bookings", new[] { "Room_RoomID" });
            DropIndex("dbo.UserImages", new[] { "UserImageID" });
            DropIndex("dbo.RoomImages", new[] { "Room_RoomID" });
            DropIndex("dbo.Equipments", new[] { "EquipmentID" });
            DropIndex("dbo.Rooms", new[] { "User_UserID" });
            DropIndex("dbo.Addresses", new[] { "AddressID" });
            DropTable("dbo.Bookings");
            DropTable("dbo.UserImages");
            DropTable("dbo.Users");
            DropTable("dbo.RoomImages");
            DropTable("dbo.Equipments");
            DropTable("dbo.Rooms");
            DropTable("dbo.Addresses");
        }
    }
}
