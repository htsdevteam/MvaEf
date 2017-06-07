using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Ch0201MusicStore.Models
{
    public class MusicStoreDbContextInitializer : DropCreateDatabaseAlways<MusicStoreDbContext>
    {
        protected override void Seed(MusicStoreDbContext context)
        {
            var artist = new Artist { Name = "First Artist" };
            context.Artists.Add(artist);
            context.Artists.Add(new Artist { Name = "Second Artist" });

            context.SaveChanges();
        }
    }
}