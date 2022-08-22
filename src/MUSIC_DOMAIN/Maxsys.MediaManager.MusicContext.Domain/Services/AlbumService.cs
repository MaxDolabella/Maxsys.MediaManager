using FluentValidation.Results;
using Maxsys.Core.Helpers;
using Maxsys.MediaManager.MusicContext.Domain.Interfaces.Repositories;
using Maxsys.MediaManager.MusicContext.Domain.Interfaces.Services;
using System.IO;

namespace Maxsys.MediaManager.MusicContext.Domain.Services;

/// <inheritdoc cref="IAlbumService"/>
public class AlbumService : IAlbumService
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
            return validationResult.AddErrorMessage($"{nameof(directory)} value is invalid (cannot be empty or null).");

        // Check wether is directory too??

        if (albumCover is null)
            return validationResult.AddErrorMessage($"{nameof(albumCover)} is invalid (cannot be empty or null).");

        var imageFilePath = Path.Combine(directory, COVER_FILE_NAME);

        return await ImageHelper.SaveByteArrayImageIntoJpgFileAsync(albumCover, imageFilePath);
    }
}