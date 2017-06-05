using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ch0101
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var context = new MisicContext())
            {
                int count = context.Albums.Count();
                Console.WriteLine(count);

                Console.WriteLine(context.Database.Connection.ConnectionString);

                context.Albums.Add(new Album
                {
                    Title = "Wish",
                    Price = 9.99m
                });
                context.SaveChanges();

                count = context.Albums.Count();
                var albums = context.Albums
                    .Where(a => a.Title.Contains("Wish"))
                    .ToList();

                Console.WriteLine(albums.Count);
                Console.ReadLine();
            }
        }
    }

    public class Album
    {
        public int AlbumId { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
    }

    public class MisicContext : DbContext
    {
        public DbSet<Album> Albums { get; set; }
    }
}
