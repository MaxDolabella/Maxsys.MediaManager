using Maxsys.MediaManager.MusicContext.Domain.Interfaces.Repositories;
using Maxsys.MediaManager.MusicContext.Infra.DataEFCore.Context;
using Maxsys.MediaManager.MusicContext.Infra.DataEFCore.Repositories.Common;

namespace Maxsys.MediaManager.MusicContext.Infra.DataEFCore.Repositories;

public class ComposerRepository : RepositoryBase<Composer>, IComposerRepository
{
    public ComposerRepository(MusicAppContext dbContext) : base(dbContext)
    {
    }

    public async Task<Composer?> GetByNameAsync(string name, bool @readonly = false, CancellationToken token = default)
    {
        return await DbSet.AsNoTracking(!@readonly)
            .FirstOrDefaultAsync(x => x.Name == name, token);
    }
}