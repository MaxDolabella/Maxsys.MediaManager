using System;
using Maxsys.Core.Services;
using Maxsys.MediaManager.CoreDomain.Helpers;
using Maxsys.MediaManager.MusicContext.Domain.DTO;
using Maxsys.MediaManager.MusicContext.Domain.Interfaces.Mp3;

namespace Maxsys.MediaManager.MusicContext.AppTagLibMono.Services;

/// <summary>
/// Implements <see cref="ITagService"/> using <see href="https://github.com/mono/taglib-sharp">Taglib-Sharp</see>
/// </summary>
/// <inheritdoc cref="ITagService"/>
public sealed class TagLibService : ServiceBase, ITagService
{
    public ValueTask<OperationResult> WriteTagsAsync(Id3v2DTO id3)
    {
        var result = new OperationResult();
        var filePath = new Uri(id3.FullPath);

        try
        {
            IOHelper2.RemoveReadOnlyAttribute(filePath);

            using (var mp3 = TagLib.File.Create(id3.FullPath))
            {
                // clear old tags
                mp3.RemoveTags(TagLib.TagTypes.AllTags);

                // Getting the tags (Mpeg\File)
                var tags = Id3v2Facade.Create(mp3, true);

                // Not nullable values
                //mp3Tags.TrackId = id3Tags.TrackId;
                tags.Title = id3.Title;
                tags.Artist = id3.Artist;
                tags.Album = id3.Album;
                tags.Genre = id3.Genre;
                tags.Stars10 = id3.Rating10;

                // Nullable values
                tags.TrackNumber = id3.TrackNumber;
                tags.Year = id3.Year;
                tags.Composers = id3.Composers;
                tags.Comments = id3.Comments;
                tags.Lyrics = id3.Lyrics;
                tags.OriginalArtist = id3.CoveredArtist;
                tags.InvolvedPeople = id3.FeaturedArtist;
                tags.CoverPicture = id3.CoverPicture;

                tags.SetInfoTag();

                mp3.Save();
            }

            IOHelper2.InsertReadOnlyAttribute(filePath);
        }
        catch (Exception ex)
        {
            result.AddException(ex, AppMessages.ERROR_WHILE_TAGGING);
        }

        return ValueTask.FromResult(result);
    }

    public ValueTask<OperationResult<Id3v2DTO?>> ReadTagsAsync(Uri filePath)
    {
        using (var mp3 = TagLib.File.Create(filePath.AbsolutePath))
        {
            var tags = Id3v2Facade.Create(mp3, false);

            var id3 = tags is not null
                ? new Id3v2DTO(fullPath: filePath.AbsolutePath
                    , title: tags.Title
                    , trackNumber: tags.TrackNumber
                    , stars10: tags.Stars10
                    , album: tags.Album
                    , genre: tags.Genre
                    , artist: tags.Artist
                    , year: tags.Year
                    , comments: tags.Comments
                    , lyrics: tags.Lyrics
                    , coveredArtist: tags.OriginalArtist
                    , featuredArtist: tags.InvolvedPeople
                    , composers: tags.Composers
                    , coverPicture: tags.CoverPicture)
                : Id3v2DTO.Empty;

            // TODO melhorar essa lógica. Se não tem id3, retorna notification?
            var result = new OperationResult<Id3v2DTO?>(id3);

            return ValueTask.FromResult(result);
        }
    }

    public ValueTask<OperationResult> WriteRatingAsync(Uri filePath, byte stars10)
    {
        var result = new OperationResult();

        try
        {
            IOHelper2.RemoveReadOnlyAttribute(filePath);

            using (var mp3 = TagLib.File.Create(filePath.AbsolutePath))
            {
                var tags = Id3v2Facade.Create(mp3, true);

                tags.Stars10 = stars10;

                mp3.Save();
            }

            IOHelper2.InsertReadOnlyAttribute(filePath);
        }
        catch (Exception ex)
        {
            result.AddException(ex, AppMessages.ERROR_WHILE_TAGGING);
        }

        return ValueTask.FromResult(result);
    }

    public ValueTask<OperationResult> WritePlaylistTagsAsync(Id3v2PlaylistItemDTO id3)
    {
        var result = new OperationResult();
        var filePath = new Uri(id3.FullPath);

        try
        {
            IOHelper2.RemoveReadOnlyAttribute(filePath);

            using (var mp3 = TagLib.File.Create(id3.FullPath))
            {
                // clear old tags
                mp3.RemoveTags(TagLib.TagTypes.AllTags);

                // Getting the tags
                var tags = Id3v2Facade.Create(mp3, true);

                // Applying values
                tags.Title = id3.Title;
                tags.Artist = id3.PlaylistName;
                tags.Album = id3.PlaylistName;
                tags.TrackNumber = (int?)id3.TrackNumber;
                tags.Genre = id3.Genre;
                tags.CoverPicture = id3.CoverPicture;
                tags.Stars10 = id3.Stars10;

                tags.SetInfoTag();

                mp3.Save();
            }

            IOHelper2.InsertReadOnlyAttribute(filePath);
        }
        catch (Exception ex)
        {
            result.AddException(ex, AppMessages.ERROR_WHILE_TAGGING);
        }

        return ValueTask.FromResult(result);
    }
}