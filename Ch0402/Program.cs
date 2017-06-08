using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ch0402
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var db = new MusicContext())
            {
                var album = new Album
                {
                    Price = 9.95m,
                    Title = "Wish"
                };
                db.Albums.Add(album);
                db.SaveChanges();
            }
        }
    }

    public class Album
    {
        public Guid AlbumId { get; set; }
        //varchar(100)
        public string Title { get; set; }
        public decimal Price { get; set; }

        public virtual AlbumDetail AlbumDetail { get; set; }
    }

    public class AlbumDetail
    {
        public string Description { get; set; }
        public int AlbumId { get; set; }
        public virtual Album Album { get; set; }
    }

    public class MusicContext : DbContext
    {
        public MusicContext()
        {
            Database.SetInitializer(new DropCreateDatabaseAlways<MusicContext>());
        }

        public DbSet<Album> Albums { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("MusicStore");
            modelBuilder.Entity<Album>().HasKey(t => t.AlbumId);
            // multi-column key
            //modelBuilder.Entity<Album>().HasKey(t => new { t.AlbumId, t.Title});

            modelBuilder.Entity<Album>().Property(t => t.Title).IsUnicode(false);

            // Not having IDENTITY column
            //modelBuilder.Entity<Album>()
            //    .Property(t => t.AlbumId)
            //    .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            modelBuilder.Entity<Album>()
                .Property(t => t.AlbumId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            // [Key]
            modelBuilder.Entity<AlbumDetail>().HasKey(t => t.AlbumId);

            //[ForeignKey]
            modelBuilder.Entity<Album>()
                .HasOptional(t => t.AlbumDetail)
                .WithRequired(t => t.Album);
            //modelBuilder.Entity<Album>()
            //    .HasRequired(t => t.AlbumDetail)
            //    .WithRequiredPrincipal(t => t.Album);

            modelBuilder.Entity<Album>()
                .ToTable("AlbumInfo", "dbo");
            modelBuilder.Entity<Album>()
                .Property(e => e.Title)
                .HasColumnName("Album_Title");

            base.OnModelCreating(modelBuilder);
        }
    }

}
