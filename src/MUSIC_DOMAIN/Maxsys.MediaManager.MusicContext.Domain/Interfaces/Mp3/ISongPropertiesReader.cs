using Maxsys.MediaManager.CoreDomain.Interfaces.Services;

namespace Maxsys.MediaManager.MusicContext.Domain.Interfaces.Mp3;

public interface ISongPropertiesReader : IFilePropertiesReader
{
    /// <summary>
    /// Gets the music duration given a mp3 path
    /// </summary>
    /// <param name="mp3Path">is the mp3 file path</param>
    /// <returns>a <see cref="TimeSpan"/> with music duration</returns>
    ValueTask<TimeSpan> GetMusicDurationAsync(Uri mp3Path);

    /// <summary>
    /// Gets the music bitrate given a mp3 path
    /// </summary>
    /// <param name="mp3Path">is the mp3 file path</param>
    /// <returns>an <see cref="int"/> that represents the bitrate of the music</returns>
    ValueTask<int> GetMusicBitrateAsync(Uri mp3Path);
}