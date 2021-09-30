using Maxsys.MediaManager.MusicContext.Domain.Factories;
using Maxsys.MediaManager.MusicContext.Domain.Interfaces.Repositories;
using Maxsys.MediaManager.MusicContext.Tests.Mock.Repositories;
using System;

namespace Maxsys.MediaManager.MusicContext.Tests.Mock
{
    internal class MockContext
    {
        public MockContext()
        {
            MusicCatalogs = new MusicCatalogMockRepository();
            Artists = new ArtistMockRepository();
            Composers = new ComposerMockRepository();

            // MusicCatalogs
            var CAT1 = MusicCatalogFactory.Create(Guid.NewGuid(), "CAT1");
            var CAT2 = MusicCatalogFactory.Create(Guid.NewGuid(), "CAT2");

            // Artists
            var CAT1_ART1 = ArtistFactory.Create(Guid.NewGuid(), "CAT1_ART1", CAT1.Id);
            var CAT1_ART2 = ArtistFactory.Create(Guid.NewGuid(), "CAT1_ART2", CAT1.Id);
            var CAT2_ART1 = ArtistFactory.Create(Guid.NewGuid(), "CAT2_ART1", CAT2.Id);
            var CAT2_ART2 = ArtistFactory.Create(Guid.NewGuid(), "CAT2_ART2", CAT2.Id);

            CAT1_ART1.SetMusicCatalog(CAT1);
            CAT1_ART2.SetMusicCatalog(CAT1);
            CAT2_ART1.SetMusicCatalog(CAT2);
            CAT2_ART2.SetMusicCatalog(CAT2);

            // Adding to repositories
            MusicCatalogs.Add(CAT1);
            MusicCatalogs.Add(CAT2);

            Artists.Add(CAT1_ART1);
            Artists.Add(CAT1_ART2);
            Artists.Add(CAT2_ART1);
            Artists.Add(CAT2_ART2);
        }

        public IMusicCatalogRepository MusicCatalogs { get; }
        public IArtistRepository Artists { get; }
        public IComposerRepository Composers { get; }
    }
}