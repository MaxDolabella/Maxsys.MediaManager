using FluentValidation.Results;
using Maxsys.Core.Helpers;
using Maxsys.MediaManager.MusicContext.Domain.Entities;
using Maxsys.MediaManager.MusicContext.Domain.Interfaces.Repositories;
using Maxsys.MediaManager.MusicContext.Domain.Interfaces.Services;
using Maxsys.ModelCore.Services;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Maxsys.MediaManager.MusicContext.Domain.Services
{
    /// <inheritdoc cref="IAlbumService"/>
    public class AlbumService : ServiceBase<Album>, IAlbumService
    {
        private readonly IAlbumRepository _repository;

        #region CONTRUCTORS

        public AlbumService(IAlbumRepository repository) : base(repository)
        {
            _repository = repository;
        }

        #endregion CONTRUCTORS

        public async Task<ValidationResult> SaveCoverPictureAsync(string directory, byte[] albumCover)
        {
            if (string.IsNullOrWhiteSpace(directory)) throw new ArgumentNullException(nameof(directory));

            // Check wether is directory too??

            if (albumCover is null) throw new ArgumentNullException(nameof(directory));

            var imageFilePath = Path.Combine(directory, "cover.jpg");

            var validationResult = await ImageHelper.SaveByteArrayImageIntoJpgFileAsync(albumCover, imageFilePath);

            return validationResult;
        }
    }
}