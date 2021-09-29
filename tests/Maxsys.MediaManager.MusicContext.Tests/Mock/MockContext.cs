using Maxsys.MediaManager.MusicContext.Domain.Interfaces.Repositories;
using Maxsys.MediaManager.MusicContext.Tests.Mock.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maxsys.MediaManager.MusicContext.Tests.Mock
{
    class MockContext
    {
        public MockContext()
        {
            MusicCatalogs = new MusicCatalogMockRepository();
        }
        public IMusicCatalogRepository MusicCatalogs { get; }
    }
}
