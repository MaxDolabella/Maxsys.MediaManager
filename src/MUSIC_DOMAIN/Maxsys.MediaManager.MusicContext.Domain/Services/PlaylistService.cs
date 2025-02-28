using System.IO;
using System.Linq.Expressions;
using AutoMapper;
using Maxsys.Core.Interfaces.Data;
using Maxsys.Core.Services;
using Maxsys.MediaManager.MusicContext.Domain.Interfaces.Mp3;
using Maxsys.MediaManager.MusicContext.Domain.Interfaces.Repositories;
using Maxsys.MediaManager.MusicContext.Domain.Interfaces.Services;

namespace Maxsys.MediaManager.MusicContext.Domain.Services;

public class PlaylistService : ServiceBase<Playlist, IPlaylistRepository, Guid>, IPlaylistService
{
    private readonly ITagService _tagService;
    private readonly IPathService _pathService;

    public PlaylistService(IPlaylistRepository repository, IUnitOfWork uow, IMapper mapper,
        ITagService tagService,
        IPathService pathService)
        : base(repository, uow, mapper)
    {
        _tagService = tagService;
        _pathService = pathService;
    }

    protected override Expression<Func<Playlist, bool>> IdSelector(Guid id) => x => x.Id == id;

    /*
    public ValidationResult ExportPlaylist(in Playlist playlist, string destRootFolder)
    {
        var validationResult = new ValidationResult();

        // Creates a Playlist Folder to put the mp3 files
        var playlistName = playlist.Name.ToUpper();
        var playlistFolder = Path.Combine(destRootFolder, $"_PL {playlistName}");

        Directory.CreateDirectory(playlistFolder);

        playlist.Items.Sort();

        for (int index = 0; index < playlist.Items.Count; index++)
        {
            #region Setting

            var music = playlist.Items[index].Song;

            var trackOrder = Convert.ToUInt32(index + 1);
            var songTitle = $"{trackOrder:000} {music.Title}";
            var stars10 = music.Classification.GetStars10();
            var genre = music.Album.Genre;
            var coverPicture = music.Album.AlbumCover;

            var fullPath = Path.Combine(playlistFolder, $"{trackOrder:000} {music.Id}.mp3");

            var tags = new Id3v2PlaylistItemDTO(fullPath, playlistName, songTitle, trackOrder, stars10, genre, coverPicture);

            #endregion Setting

            #region Copying

            // Copy mp3 files to the Playlist folder. Returns validation if some item fails.
            var srcFile = music.FullPath;
            var cpyFile = tags.FullPath;

            var copyingResult = IOHelper.CopyFile(srcFile, cpyFile);
            if (!copyingResult.IsValid) return copyingResult;

            #endregion Copying

            #region Tagging

            // Sets the tags to the copied file. Returns validation if some item fails
            var taggingResult = _tagService.WritePlaylistTags(tags);
            if (!taggingResult.IsValid) return taggingResult;

            #endregion Tagging
        }

        return validationResult;
    }

    /// <inheritdoc/>
    public IEnumerable<PlaylistDTO> GetPlaylistsByMusic(Song music)
    {
        var playlists = _repository
            .Find(pl => pl.Items.Contains(music), @readonly: true)
            .Select(pl => new PlaylistDTO
            {
                PlaylistId = pl.Id,
                PlaylistName = pl.Name
            });

        return playlists;
    }
    */

    /// <inheritdoc/>
    public async Task<OperationResult> ExportPlaylistFileAsync(Guid id,
        IPlaylistFileExporter playlistFileExporter,
        Uri? destRootFolder = null,
        CancellationToken cancellationToken = default)
    {
        var playlist = await _repository.GetToExportAsync(id, cancellationToken);
        if (playlist is null)
        {
            return new(GenericMessages.ITEM_NOT_FOUND);
        }

        destRootFolder ??= _pathService.GetDefaultPlaylistDirectory();
        var dir = Directory.CreateDirectory(destRootFolder.AbsolutePath);

        var playlistName = playlist.Name;
        var songFiles = playlist.Items
            .Select(item => item.Song.FullPath.AbsolutePath)
            .Reverse()
            .ToList();

        return await playlistFileExporter.ExportFileAsync(songFiles, new Uri(dir.FullName), playlistName, cancellationToken);
    }
}