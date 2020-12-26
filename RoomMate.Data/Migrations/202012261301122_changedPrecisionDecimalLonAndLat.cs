namespace RoomMate.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changedPrecisionDecimalLonAndLat : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Addresses", "Lon", c => c.Decimal(nullable: false, precision: 8, scale: 6));
            AlterColumn("dbo.Addresses", "Lat", c => c.Decimal(nullable: false, precision: 9, scale: 6));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Addresses", "Lat", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Addresses", "Lon", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
