using Maxsys.MediaManager.CoreDomain.Interfaces.Services;
using System;

namespace Maxsys.MediaManager.MusicContext.Domain.Interfaces.Services.Mp3
{
    // TODO make methods async?
    public interface IMusicPropertiesReader : IFilePropertiesReader, IDisposable
    {
        /// <summary>
        /// Gets the music duration given a mp3 path
        /// </summary>
        /// <param name="mp3Path">is the mp3 file path</param>
        /// <returns>a <see cref="TimeSpan"/> with music duration</returns>
        TimeSpan GetMusicDuration(string mp3Path);


        /// <summary>
        /// Gets the music bitrate given a mp3 path
        /// </summary>
        /// <param name="mp3Path">is the mp3 file path</param>
        /// <returns>an <see cref="int"/> that represents the bitrate of the music</returns>
        int GetMusicBitrate(string mp3Path);
    }
}
