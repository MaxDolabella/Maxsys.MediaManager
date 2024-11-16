using System.IO;
using FluentValidation.Results;
using Maxsys.Core.Helpers;
using Maxsys.Core.Services;
using Maxsys.MediaManager.MusicContext.Domain.Interfaces.Repositories;
using Maxsys.MediaManager.MusicContext.Domain.Interfaces.Services;

namespace Maxsys.MediaManager.MusicContext.Domain.Services;

/// <inheritdoc cref="IAlbumService"/>
public class AlbumService : ServiceBase, IAlbumService
{
    private const string COVER_FILE_NAME = "cover.jpg";
    private readonly IAlbumRepository _repository;

    #region CONTRUCTORS

    public AlbumService(IAlbumRepository repository)
    {
        _repository = repository;
    }

    #endregion CONTRUCTORS

    public async ValueTask<ValidationResult> SaveCoverPictureAsync(string directory, byte[] albumCover, CancellationToken token = default)
    {
        ValidationResult validationResult = new();

        if (string.IsNullOrWhiteSpace(directory))
            return validationResult.AddError($"{nameof(directory)} value is invalid (cannot be empty or null).");

        // Check wether is directory too??

        if (albumCover is null)
            return validationResult.AddError($"{nameof(albumCover)} is invalid (cannot be empty or null).");

        var imageFilePath = Path.Combine(directory, COVER_FILE_NAME);

        return await ImageHelper.SaveByteArrayImageIntoJpgFileAsync(albumCover, imageFilePath);
    }
}