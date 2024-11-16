using Maxsys.Core.Data;
using Maxsys.MediaManager.MusicContext.Infra.DataEFCore.Context;
using Microsoft.Extensions.Logging;

namespace Maxsys.MediaManager.MusicContext.Infra.DataEFCore;

public class AppUnitOfWork : UnitOfWorkBase<MusicAppContext>
{
    public AppUnitOfWork(ILogger<AppUnitOfWork> logger, MusicAppContext context) : base(logger, context)
    { }
}