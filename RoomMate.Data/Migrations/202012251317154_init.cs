namespace RoomMate.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
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
                        PostCode = c.String(nullable: false, maxLength: 50),
                        Flat = c.String(nullable: false, maxLength: 50),
                        Lon = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Lat = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.AddressID);
            
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
                        Address_AddressID = c.Guid(),
                        Equipment_EquipmentID = c.Guid(),
                        User_UserID = c.Guid(),
                    })
                .PrimaryKey(t => t.RoomID)
                .ForeignKey("dbo.Addresses", t => t.Address_AddressID)
                .ForeignKey("dbo.Equipments", t => t.Equipment_EquipmentID)
                .ForeignKey("dbo.Users", t => t.User_UserID)
                .Index(t => t.Address_AddressID)
                .Index(t => t.Equipment_EquipmentID)
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
                .PrimaryKey(t => t.EquipmentID);
            
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
                        UserImage_UserImageID = c.Guid(),
                    })
                .PrimaryKey(t => t.UserID)
                .ForeignKey("dbo.UserImages", t => t.UserImage_UserImageID)
                .Index(t => t.UserImage_UserImageID);
            
            CreateTable(
                "dbo.UserImages",
                c => new
                    {
                        UserImageID = c.Guid(nullable: false),
                        ImageName = c.String(nullable: false, maxLength: 256),
                        ImagePath = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.UserImageID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Bookings", "User_UserID", "dbo.Users");
            DropForeignKey("dbo.Bookings", "Room_RoomID", "dbo.Rooms");
            DropForeignKey("dbo.Users", "UserImage_UserImageID", "dbo.UserImages");
            DropForeignKey("dbo.Rooms", "User_UserID", "dbo.Users");
            DropForeignKey("dbo.RoomImages", "Room_RoomID", "dbo.Rooms");
            DropForeignKey("dbo.Rooms", "Equipment_EquipmentID", "dbo.Equipments");
            DropForeignKey("dbo.Rooms", "Address_AddressID", "dbo.Addresses");
            DropIndex("dbo.Users", new[] { "UserImage_UserImageID" });
            DropIndex("dbo.RoomImages", new[] { "Room_RoomID" });
            DropIndex("dbo.Rooms", new[] { "User_UserID" });
            DropIndex("dbo.Rooms", new[] { "Equipment_EquipmentID" });
            DropIndex("dbo.Rooms", new[] { "Address_AddressID" });
            DropIndex("dbo.Bookings", new[] { "User_UserID" });
            DropIndex("dbo.Bookings", new[] { "Room_RoomID" });
            DropTable("dbo.UserImages");
            DropTable("dbo.Users");
            DropTable("dbo.RoomImages");
            DropTable("dbo.Equipments");
            DropTable("dbo.Rooms");
            DropTable("dbo.Bookings");
            DropTable("dbo.Addresses");
        }
    }
}
