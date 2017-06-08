namespace Ch0402.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AlbumInfo",
                c => new
                    {
                        AlbumId = c.Guid(nullable: false, identity: true),
                        Album_Title = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.AlbumId);
            
            CreateTable(
                "MusicStore.AlbumDetails",
                c => new
                    {
                        AlbumId = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        Album_AlbumId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.AlbumId)
                .ForeignKey("dbo.AlbumInfo", t => t.Album_AlbumId)
                .Index(t => t.Album_AlbumId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("MusicStore.AlbumDetails", "Album_AlbumId", "dbo.AlbumInfo");
            DropIndex("MusicStore.AlbumDetails", new[] { "Album_AlbumId" });
            DropTable("MusicStore.AlbumDetails");
            DropTable("dbo.AlbumInfo");
        }
    }
}
