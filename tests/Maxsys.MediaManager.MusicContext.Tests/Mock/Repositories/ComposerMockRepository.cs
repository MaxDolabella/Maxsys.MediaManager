//using Maxsys.MediaManager.MusicContext.Domain.Entities;
//using Maxsys.MediaManager.MusicContext.Domain.Interfaces.Repositories;
//using System.Linq;
//using System.Threading.Tasks;

//namespace Maxsys.MediaManager.MusicContext.Tests.Mock.Repositories
//{
//    internal class ComposerMockRepository : MockRepositoryBase<Composer>, IComposerRepository
//    {
//        public async Task<Composer> GetByNameAsync(string name, bool @readonly = false)
//        {
//            return await Task.Run(() => _entities.FirstOrDefault(c => c.Name == name));
//        }
//    }
//}