namespace Ch0601.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedTimestamp : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Albums", "Timestamp", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AlterStoredProcedure(
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
                      
                      SELECT t0.[AlbumId], t0.[Timestamp]
                      FROM [dbo].[Albums] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[AlbumId] = @AlbumId"
            );
            
            AlterStoredProcedure(
                "dbo.Album_Update",
                p => new
                    {
                        AlbumId = p.Int(),
                        Title = p.String(),
                        Price = p.Decimal(precision: 18, scale: 2),
                        Timestamp_Original = p.Binary(maxLength: 8, fixedLength: true, storeType: "rowversion"),
                    },
                body:
                    @"UPDATE [dbo].[Albums]
                      SET [Title] = @Title, [Price] = @Price
                      WHERE (([AlbumId] = @AlbumId) AND (([Timestamp] = @Timestamp_Original) OR ([Timestamp] IS NULL AND @Timestamp_Original IS NULL)))
                      
                      SELECT t0.[Timestamp]
                      FROM [dbo].[Albums] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[AlbumId] = @AlbumId"
            );
            
            AlterStoredProcedure(
                "dbo.Album_Delete",
                p => new
                    {
                        AlbumId = p.Int(),
                        Timestamp_Original = p.Binary(maxLength: 8, fixedLength: true, storeType: "rowversion"),
                    },
                body:
                    @"DELETE [dbo].[Albums]
                      WHERE (([AlbumId] = @AlbumId) AND (([Timestamp] = @Timestamp_Original) OR ([Timestamp] IS NULL AND @Timestamp_Original IS NULL)))"
            );
            
        }
        
        public override void Down()
        {
            DropColumn("dbo.Albums", "Timestamp");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
