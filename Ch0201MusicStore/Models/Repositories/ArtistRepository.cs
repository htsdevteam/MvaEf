﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ch0201MusicStore.Models.Repositories
{
    public class ArtistRepository : Repository<Artist>
    {
        public List<Artist> GetByName(string name)
        {
            return DbSet.Where(a => a.Name.Contains(name)).ToList();
        }

        public List<SoloArtist> GetSoloArtists()
        {
            return DbSet.OfType<SoloArtist>().ToList();
        }

        // for the self-made implementation of concurrency
        public override void Update(Artist entity)
        {
            base.Update(entity);
            SaveChanges();
            ++entity.Version;
            base.Update(entity);
            SaveChanges();
        }
    }
}