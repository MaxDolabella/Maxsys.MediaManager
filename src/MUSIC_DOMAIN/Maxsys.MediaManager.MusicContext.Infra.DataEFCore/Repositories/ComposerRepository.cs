using AutoMapper;
using Maxsys.Core.Data;
using Maxsys.MediaManager.MusicContext.Domain.Interfaces.Repositories;
using Maxsys.MediaManager.MusicContext.Infra.DataEFCore.Context;

namespace Maxsys.MediaManager.MusicContext.Infra.DataEFCore.Repositories;

public class ComposerRepository : RepositoryBase<Composer>, IComposerRepository
{
    public ComposerRepository(MusicAppContext context, IMapper mapper) : base(context, mapper)
    {
    }

    public async Task<Composer?> GetByNameAsync(string name, bool @readonly = false, CancellationToken token = default)
    {
        return await DbSet.FirstOrDefaultAsync(x => x.Name == name, token);
    }
}