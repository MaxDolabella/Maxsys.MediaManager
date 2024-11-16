//using Maxsys.MediaManager.MusicContext.Domain.Factories;
//using Maxsys.MediaManager.MusicContext.Domain.Interfaces.Repositories;
//using Maxsys.MediaManager.MusicContext.Tests.Mock.Repositories;
//using System;

//namespace Maxsys.MediaManager.MusicContext.Tests.Mock;

//internal class MockContext
//{
//    public MockContext()
//    {
//        MusicCatalogs = new MusicCatalogMockRepository();
//        Artists = new ArtistMockRepository();
//        Composers = new ComposerMockRepository();

//        // MusicCatalogs
//        var CAT1 = CatalogFactory.Create("CAT1");
//        var CAT2 = CatalogFactory.Create("CAT2");

//        // Artists
//        var CAT1_ART1 = ArtistFactory.Create(CAT1.Id, "CAT1_ART1");
//        var CAT1_ART2 = ArtistFactory.Create(CAT1.Id, "CAT1_ART2");
//        var CAT2_ART1 = ArtistFactory.Create(CAT2.Id, "CAT2_ART1");
//        var CAT2_ART2 = ArtistFactory.Create(CAT2.Id, "CAT2_ART2");

//        CAT1_ART1.SetCatalog(CAT1);
//        CAT1_ART2.SetCatalog(CAT1);
//        CAT2_ART1.SetCatalog(CAT2);
//        CAT2_ART2.SetCatalog(CAT2);

//        // Adding to repositories
//        MusicCatalogs.Add(CAT1);
//        MusicCatalogs.Add(CAT2);

//        Artists.Add(CAT1_ART1);
//        Artists.Add(CAT1_ART2);
//        Artists.Add(CAT2_ART1);
//        Artists.Add(CAT2_ART2);
//    }

//    public ICatalogRepository MusicCatalogs { get; }
//    public IArtistRepository Artists { get; }
//    public IComposerRepository Composers { get; }
//}