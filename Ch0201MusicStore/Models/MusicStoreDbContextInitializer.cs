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
            context.Albums.Add(new Album { Artist = artist, Title = "First Album" });
            context.Albums.Add(new Album { Artist = artist, Title = "Second Album" });

            context.Albums.Add(new Album
            {
                Artist = context.Artists.Add(new Artist { Name = "Third Artist"}),
                Title = "Third Album"
            });

            context.Artists.Add(new SoloArtist {
                Name = "Solo Artist",
                Instrument = "Guitar"
            });

            context.SaveChanges();
        }
    }
}