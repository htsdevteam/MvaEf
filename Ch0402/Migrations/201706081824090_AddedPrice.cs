namespace Ch0402.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedPrice : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AlbumInfo", "Price", c => c.Decimal(nullable: false, precision: 18, scale: 2, defaultValue: 9.95m));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AlbumInfo", "Price");
        }
    }
}
