using System.IO;
using System.Linq.Expressions;
using AutoMapper;
using Maxsys.Core.Helpers;
using Maxsys.Core.Interfaces.Data;
using Maxsys.Core.Services;
using Maxsys.MediaManager.MusicContext.Domain.Interfaces.Repositories;
using Maxsys.MediaManager.MusicContext.Domain.Interfaces.Services;

namespace Maxsys.MediaManager.MusicContext.Domain.Services;

/// <inheritdoc cref="IAlbumService"/>
public class AlbumService : ServiceBase<Album, IAlbumRepository, Guid>, IAlbumService
{
    private const string COVER_FILE_NAME = "cover.jpg";

    public AlbumService(IAlbumRepository repository, IUnitOfWork uow, IMapper mapper)
        : base(repository, uow, mapper)
    { }

    protected override Expression<Func<Album, bool>> IdSelector(Guid id) => x => x.Id == id;

    public async ValueTask<OperationResult> SaveCoverPictureAsync(string directory, byte[] albumCover, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(directory, nameof(directory));
        ArgumentNullException.ThrowIfNull(albumCover, nameof(albumCover));

        var imageFilePath = Path.Combine(directory, COVER_FILE_NAME);

        OperationResult result = new(await ImageHelper.SaveByteArrayImageIntoJpgFileAsync(albumCover, imageFilePath));

        return result;
    }
}