using Microsoft.EntityFrameworkCore;
using MyMusic.Data.Configurations;
using MyMusic.Core.Models;

namespace MyMusic.Data
{
    public class MyMusicDBContext : DbContext
    {

        public DbSet<Music> Musics { get; set; }
        public DbSet<Artist> Artists { get; set; }

        public MyMusicDBContext(DbContextOptions<MyMusicDBContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new MusicConfiguration());
            builder.ApplyConfiguration(new ArtistConfiguration());
        }
    }
}
