using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Ch0601.Models
{
    public class Album
    {
        public int AlbumId { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }

        [Timestamp]
        public byte[] Timestamp { get; set; }
    }

    public class MusicContext : DbContext
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Album>()
                .MapToStoredProcedures();

            //modelBuilder.Entity<Album>()
            //    .MapToStoredProcedures(o =>
            //    o.Insert(p => p.HasName("Proc_InsertAlbum"))
            //        .Update(u => u.HasName("Proc_UpdateAlbum"))
            //        .Delete(d => d.HasName("Proc_DeleteAlbum")));

            //modelBuilder.Entity<Album>()
            //    .MapToStoredProcedures(
            //        o => o.Insert(proc => proc.HasName("Proc_AddAlbum")
            //        .Result(p => p.AlbumId, "id")));

            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Album> Albums { get; set; }
    }
}