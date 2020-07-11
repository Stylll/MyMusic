﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MyMusic.Core;
using MyMusic.Core.Repositories;
using MyMusic.Data.Repositories;

namespace MyMusic.Data
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly MyMusicDBContext _context;
        private MusicRepository _musicRepository;
        private ArtistRepository _artistRepository;

        public UnitOfWork(MyMusicDBContext context)
        {
            this._context = context;
        }

        public IMusicRepository Music => _musicRepository = _musicRepository ?? new MusicRepository(_context);

        public IArtistRepository Artist => _artistRepository = _artistRepository ?? new ArtistRepository(_context);

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
