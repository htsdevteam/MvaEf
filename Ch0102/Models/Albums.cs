using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace Ch0102.Models
{
    public class Album
    {
        public int AlbumId { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
    }

    public class MisicContext : DbContext
    {
        public MisicContext()
        {
            Database.Log = s => Debug.WriteLine(s);
        }

        public DbSet<Album> Albums { get; set; }
    }
}