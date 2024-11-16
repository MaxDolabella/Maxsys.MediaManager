//using Maxsys.ModelCore;
//using Maxsys.ModelCore.Interfaces.Repositories;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Threading.Tasks;

//namespace Maxsys.MediaManager.MusicContext.Tests.Mock.Repositories
//{
//    internal class MockRepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : EntityBase
//    {
//        protected readonly IList<TEntity> _entities = new List<TEntity>();

//        private void CheckIfIsIdAlreadyRegistered(TEntity obj)
//        {
//            if (IsIdAlreadyRegistered(obj))
//                throw new ArgumentException("An object with same ID already exists in the repository.");
//        }

//        private bool IsIdAlreadyRegistered(TEntity obj)
//        {
//            return _entities.Any(entity => entity.Id == obj.Id);
//        }

//        public IEnumerable<TEntity> GetAll(bool @readonly = true)
//            => _entities.ToList();

//        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate, bool @readonly = true)
//            => _entities.Where(predicate.Compile()).ToList();

//        public TEntity GetById(object key, bool @readonly = true)
//            => _entities.FirstOrDefault(entity => entity.Id == (Guid)key);

//        public bool Add(TEntity obj)
//        {
//            CheckIfIsIdAlreadyRegistered(obj);

//            _entities.Add(obj);

//            return true;
//        }

//        public bool Update(TEntity obj) => true;

//        public bool Remove(TEntity obj)
//        {
//            return _entities.Remove(obj);
//        }

//        public bool Remove(object key)
//        {
//            var obj = GetById(key);

//            return obj is not null && Remove(obj);
//        }

//        public async Task<IEnumerable<TEntity>> GetAllAsync(bool @readonly = true)
//        {
//            return await Task.Run(() => GetAll());
//        }

//        public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, bool @readonly = true)
//        {
//            return await Task.Run(() => Find(predicate));
//        }

//        public async Task<TEntity> GetByIdAsync(object key, bool @readonly = true)
//        {
//            return await Task.Run(() => GetById(key));
//        }

//        public async Task<bool> AddAsync(TEntity obj)
//        {
//            return await Task.Run(() => Add(obj));
//        }

//        public async Task<bool> UpdateAsync(TEntity obj)
//        {
//            return await Task.Run(() => Update(obj));
//        }

//        public async Task<bool> RemoveAsync(TEntity obj)
//        {
//            return await Task.Run(() => Remove(obj));
//        }

//        public async Task<bool> RemoveAsync(object key)
//        {
//            return await Task.Run(() => Remove(key));
//        }

//        public void Dispose()
//        {
//            throw new NotImplementedException();
//        }
//    }
//}