namespace Ch0601.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedProcs : DbMigration
    {
        public override void Up()
        {
            RenameStoredProcedure(name: "dbo.Album_Insert", newName: "Proc_AddAlbum");
            AlterStoredProcedure(
                "dbo.Proc_AddAlbum",
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
                      
                      SELECT t0.[AlbumId] AS id
                      FROM [dbo].[Albums] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[AlbumId] = @AlbumId"
            );
            
        }
        
        public override void Down()
        {
            RenameStoredProcedure(name: "dbo.Proc_AddAlbum", newName: "Album_Insert");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
