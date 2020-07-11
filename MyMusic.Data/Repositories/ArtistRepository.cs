using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyMusic.Core.Models;
using MyMusic.Core.Repositories;

namespace MyMusic.Data.Repositories
{
    public class ArtistRepository : Repository<Artist>, IArtistRepository
    {

        public ArtistRepository(MyMusicDBContext context) : base(context) { }

        private MyMusicDBContext MyMusicDBContext
        {
            get { return Context as MyMusicDBContext; }
        }

        public async Task<IEnumerable<Artist>> GetAllWithMusicsAsync()
        {
            return await MyMusicDBContext.Artists.Include(a => a.Musics).ToListAsync();
        }

        public async Task<Artist> GetWithMusicsByIdAsync(int id)
        {
            return await MyMusicDBContext.Artists.Include(a => a.Musics).SingleOrDefaultAsync(x => x.Id == id);
        }
    }
}
