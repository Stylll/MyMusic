using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyMusic.Core.Models;

namespace MyMusic.Data.Configurations
{
    public class MusicConfiguration : IEntityTypeConfiguration<Music>
    {
        public void Configure(EntityTypeBuilder<Music> builder)
        {
            builder.HasKey(m => m.Id);

            builder.Property(m => m.Id).UseSqlServerIdentityColumn();

            builder.Property(m => m.Name).HasMaxLength(50).IsRequired();

            builder.HasOne(m => m.Artist)
                .WithMany(m => m.Musics)
                .HasForeignKey(m => m.ArtistId);

            builder.ToTable("Music");
        }
    }
}
