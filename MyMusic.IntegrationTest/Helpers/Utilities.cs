using System;
using System.Collections.Generic;
using System.Text;
using MyMusic.Data;
using MyMusic.Core.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MyMusic.IntegrationTest.Helpers
{
    public static class Utilities
    {
        public static void SetupDb(MyMusicDBContext db)
        {
            using (var transactions = db.Database.BeginTransaction())
            {
                db.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].[Artist] ON");
                db.Database.ExecuteSqlCommand("INSERT INTO Artist (Id, Name) VALUES (45, 'Drake'), " +
                    "(46, 'Wizkid'), (47, 'Mac Miller'), (48, 'Bruno Mars')");
                db.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].[Artist] OFF");
                db.Database.ExecuteSqlCommand("INSERT INTO Music (Name, ArtistId) VALUES ('Closer', 45), " +
                    "('Starboy', 46), ('Circles', 47), ('Versace on the floor', 48)");
                transactions.Commit();
            }

            db.Artists.AddRange(GetArtists());
            db.SaveChanges();
        }

        public static List<Artist> GetArtists()
        {
            return new List<Artist>()
            {
                new Artist { Name = "Stephen" },
                new Artist { Name = "Chance" },
                new Artist { Name = "Alex" }

            };
        }
    }
}
