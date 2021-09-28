using Maxsys.MediaManager.MusicContext.Domain.Entities;
using Maxsys.ModelCore.Interfaces.Repositories;
using System.Threading.Tasks;

namespace Maxsys.MediaManager.MusicContext.Domain.Interfaces.Repositories
{
    public interface IComposerRepository : IRepositoryBase<Composer>
    {
        Task<Composer> GetByNameAsync(string name, bool @readonly = false);
    }
}