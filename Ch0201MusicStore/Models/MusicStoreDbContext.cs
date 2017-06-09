using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Ch0201MusicStore.Models
{
    public class MusicStoreDbContext : DbContext
    {
        public DbSet<Artist> Artists { get; set; }
        public DbSet<Album> Albums { get; set; }

        public System.Data.Entity.DbSet<Ch0201MusicStore.Models.ArtistDetails> ArtistDetails { get; set; }
    }
}