using Maxsys.MediaManager.MusicContext.Domain.Entities;
using Maxsys.MediaManager.MusicContext.Domain.Interfaces.Repositories;
using Maxsys.MediaManager.MusicContext.Infra.DataEFCore.Context;
using Maxsys.MediaManager.MusicContext.Infra.DataEFCore.Repositories.Common;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Maxsys.MediaManager.MusicContext.Infra.DataEFCore.Repositories
{
    public class ComposerRepository : RepositoryBase<Composer>, IComposerRepository
    {
        public ComposerRepository(MusicAppContext dbContext) : base(dbContext) { }


        public async Task<Composer> GetByNameAsync(string name, bool @readonly = false)
        {
            return await DbSet.AsNoTracking(!@readonly)
                .FirstOrDefaultAsync(x => x.Name == name);
        }
    }
}
