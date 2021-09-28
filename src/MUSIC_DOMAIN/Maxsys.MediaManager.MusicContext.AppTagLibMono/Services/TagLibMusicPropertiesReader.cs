using Maxsys.MediaManager.CoreDomain.Interfaces.Services;
using Maxsys.MediaManager.MusicContext.Domain.Interfaces.Services.Mp3;
using System;
using System.IO;

namespace Maxsys.MediaManager.MusicContext.AppTagLibMono.Services
{
    /// <summary>
    /// Implements <see cref="IMusicPropertiesReader"/> using <see href="https://github.com/mono/taglib-sharp">Taglib-Sharp</see>
    /// </summary>
    public class TagLibMusicPropertiesReader : IMusicPropertiesReader
    {
        private readonly IFilePropertiesReader _filePropertiesReader;

        public TagLibMusicPropertiesReader(IFilePropertiesReader filePropertiesReader)
        {
            _filePropertiesReader = filePropertiesReader;
        }

        #region IMusicPropertiesReader Implementation

        /// <inheritdoc/>
        public TimeSpan GetMusicDuration(string mp3Path)
        {
            if (!File.Exists(mp3Path)) return TimeSpan.Zero;

            TimeSpan duration;
            using (var mp3 = TagLib.File.Create(mp3Path))
                duration = mp3.Properties.Duration;

            return duration;
        }

        /// <inheritdoc/>
        public int GetMusicBitrate(string mp3Path)
        {
            if (!File.Exists(mp3Path)) return 0;

            int bitrate;
            using (var mp3 = TagLib.File.Create(mp3Path))
                bitrate = mp3.Properties.AudioBitrate;

            return bitrate;
        }

        #endregion IMusicPropertiesReader Implementation

        #region IFilePropertiesReader Implementation

        /// <inheritdoc/>
        public string GetFileNameWithoutExtension(string fullPath)
            => _filePropertiesReader.GetFileNameWithoutExtension(fullPath);

        /// <inheritdoc/>
        public string GetFileExtension(string fullPath)
            => _filePropertiesReader.GetFileExtension(fullPath);

        /// <inheritdoc/>
        public long GetFileSize(string fullPath)
            => _filePropertiesReader.GetFileSize(fullPath);

        #endregion IFilePropertiesReader Implementation

        #region DIPOSABLE IMPLEMENTATION

        protected bool _disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _filePropertiesReader?.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion DIPOSABLE IMPLEMENTATION
    }
}