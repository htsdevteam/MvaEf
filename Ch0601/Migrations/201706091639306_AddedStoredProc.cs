namespace Ch0601.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedStoredProc : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure(
                "dbo.Album_Insert",
                p => new
                    {
                        Title = p.String(),
                        Price = p.Decimal(precision: 18, scale: 2),
                    },
                body:
                    @"INSERT [dbo].[Albums]([Title], [Price])
                      VALUES (@Title, @Price)
                      
                      DECLARE @AlbumId int
                      SELECT @AlbumId = [AlbumId]
                      FROM [dbo].[Albums]
                      WHERE @@ROWCOUNT > 0 AND [AlbumId] = scope_identity()
                      
                      SELECT t0.[AlbumId]
                      FROM [dbo].[Albums] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[AlbumId] = @AlbumId"
            );
            
            CreateStoredProcedure(
                "dbo.Album_Update",
                p => new
                    {
                        AlbumId = p.Int(),
                        Title = p.String(),
                        Price = p.Decimal(precision: 18, scale: 2),
                    },
                body:
                    @"UPDATE [dbo].[Albums]
                      SET [Title] = @Title, [Price] = @Price
                      WHERE ([AlbumId] = @AlbumId)"
            );
            
            CreateStoredProcedure(
                "dbo.Album_Delete",
                p => new
                    {
                        AlbumId = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[Albums]
                      WHERE ([AlbumId] = @AlbumId)"
            );
            
        }
        
        public override void Down()
        {
            DropStoredProcedure("dbo.Album_Delete");
            DropStoredProcedure("dbo.Album_Update");
            DropStoredProcedure("dbo.Album_Insert");
        }
    }
}
