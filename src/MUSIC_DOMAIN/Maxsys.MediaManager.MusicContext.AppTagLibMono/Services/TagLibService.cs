using System;
using FluentValidation.Results;
using Maxsys.Core.Helpers;
using Maxsys.Core.Services;
using Maxsys.MediaManager.MusicContext.Domain.DTO;
using Maxsys.MediaManager.MusicContext.Domain.Interfaces.Mp3;
using TaglibCore;

namespace Maxsys.MediaManager.MusicContext.AppTagLibMono.Services;

/// <summary>
/// Implements <see cref="ITagService"/> using <see href="https://github.com/mono/taglib-sharp">Taglib-Sharp</see>
/// </summary>
/// <inheritdoc cref="ITagService"/>
public class TagLibService : ServiceBase, ITagService
{
    public ValidationResult WriteTags(Id3v2DTO dto)
    {
        var validationResult = new ValidationResult();
        var fileFullPath = dto.FullPath;

        IOHelper.RemoveReadOnlyAttribute(fileFullPath);

        try
        {
            using (var mp3 = TagLib.File.Create(fileFullPath))
            {
                // clear old tags
                mp3.RemoveTags(TagLib.TagTypes.AllTags);

                // Getting the tags (Mpeg\File)
                var tags = Id3v2Facade.Create(mp3, true);

                // Not nullable values
                //mp3Tags.TrackId = id3Tags.TrackId;
                tags.Title = dto.Title;
                tags.Artist = dto.Artist;
                tags.Album = dto.Album;
                tags.Genre = dto.Genre;
                tags.Stars10 = dto.Rating10;

                // Nullable values
                tags.TrackNumber = dto.TrackNumber;
                tags.Year = dto.Year;
                tags.Composers = dto.Composers;
                tags.Comments = dto.Comments;
                tags.Lyrics = dto.Lyrics;
                tags.OriginalArtist = dto.CoveredArtist;
                tags.InvolvedPeople = dto.FeaturedArtist;
                tags.CoverPicture = dto.CoverPicture;

                tags.SetInfoTag();

                mp3.Save();
            }
        }
        catch (Exception ex)
        {
            validationResult.AddException(ex, $"Error at tagging Mp3 File: {ex.Message}");
        }

        IOHelper.InsertReadOnlyAttribute(fileFullPath);

        return validationResult;
    }

    public Id3v2DTO ReadTags(string filePath)
    {
        using (var mp3 = TagLib.File.Create(filePath))
        {
            var tags = Id3v2Facade.Create(mp3, false);

            return tags is null
                ? Id3v2DTO.Empty
                : new Id3v2DTO(fullPath: filePath
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
                    , coverPicture: tags.CoverPicture);
        }
    }

    public ValidationResult WriteRating(string filePath, byte stars10)
    {
        var validationResult = new ValidationResult();

        IOHelper.RemoveReadOnlyAttribute(filePath);

        try
        {
            using (var mp3 = TagLib.File.Create(filePath))
            {
                var tags = Id3v2Facade.Create(mp3, true);

                tags.Stars10 = stars10;

                mp3.Save();
            }
            //using (var mp3 = TagLib.File.Create(filePath))
            //{
            //    // Getting the tags
            //    var v2 = mp3.GetTag(TagLib.TagTypes.Id3v2, true) as TagLib.Id3v2.Id3v2Tag;

            //    PopularimeterHelper.WriteFromStars10(v2, stars10);

            //    mp3.Save();
            //}
        }
        catch (Exception ex)
        {
            validationResult.AddException(ex, $"Error at tagging Mp3 File: {ex.Message}");
        }

        IOHelper.InsertReadOnlyAttribute(filePath);

        return validationResult;
    }

    public ValidationResult WritePlaylistTags(Id3v2PlaylistItemDTO dto)
    {
        var validationResult = new ValidationResult();

        IOHelper.RemoveReadOnlyAttribute(dto.FullPath);

        try
        {
            using (var mp3 = TagLib.File.Create(dto.FullPath))
            {
                // clear old tags
                mp3.RemoveTags(TagLib.TagTypes.AllTags);

                // Getting the tags
                var tags = Id3v2Facade.Create(mp3, true);

                // Applying values
                tags.Title = dto.Title;
                tags.Artist = dto.PlaylistName;
                tags.Album = dto.PlaylistName;
                tags.TrackNumber = (int?)dto.TrackNumber;
                tags.Genre = dto.Genre;
                tags.CoverPicture = dto.CoverPicture;
                tags.Stars10 = dto.Stars10;

                tags.SetInfoTag();

                mp3.Save();
            }
        }
        catch (Exception ex)
        {
            validationResult.AddException(ex, $"Error at tagging Mp3 File: {ex.Message}");
        }

        IOHelper.InsertReadOnlyAttribute(dto.FullPath);

        return validationResult;
    }
}