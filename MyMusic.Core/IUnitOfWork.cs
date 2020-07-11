using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyMusic.Core.Repositories;
using MyMusic.Core.Models;

namespace MyMusic.Core
{
    public interface IUnitOfWork : IDisposable
    {
        IMusicRepository Music { get; }
        IArtistRepository Artist { get; }
        Task<int> CommitAsync();
    }
}
