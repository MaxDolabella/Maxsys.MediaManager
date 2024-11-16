using FluentValidation.Results;
using Maxsys.Core.Interfaces.Services;
using Maxsys.MediaManager.MusicContext.Domain.DTO;

namespace Maxsys.MediaManager.MusicContext.Domain.Interfaces.Mp3;

// TODO make methods async?
/// <summary>
/// Provides methods to read and write Id3 tags in files.
/// </summary>
public interface ITagService : IService
{
    /// <summary>
    /// Write rating tag on file.
    /// </summary>
    /// <param name="filePath">is the file path of the mp3</param>
    /// <param name="stars10">is the stars (0-10) to write on file</param>
    /// <returns></returns>
    ValidationResult WriteRating(string filePath, byte stars10);

    /// <summary>
    /// Write Id3 tags on file.
    /// </summary>
    /// <param name="id3v2">is the id3v2 tags to write.</param>
    ValidationResult WriteTags(Id3v2DTO id3v2);

    /// <summary>
    /// Write Id3 tags on file following a specifc pattern
    /// and moves the files to specific folder.
    /// If all operations are goods, validation will be valid.
    /// Each file not tagged (some error) will be there a failure,
    /// where <see cref="ValidationFailure.PropertyName"/>
    /// contains the path of file.
    /// </summary>
    /// <param name="playlistItem">is the id3v2 tags to write</param>
    /// <returns>a <see cref="ValidationResult"/> with operation result.</returns>
    ValidationResult WritePlaylistTags(Id3v2PlaylistItemDTO playlistItem);

    /// <summary>
    /// Read Id3v2 Tags from a file
    /// </summary>
    /// <param name="filePath">the path of the file</param>
    /// <returns><see cref="Id3v2TagsDTO"/> struct with id3v2 tag values</returns>
    Id3v2DTO ReadTags(string filePath);
}