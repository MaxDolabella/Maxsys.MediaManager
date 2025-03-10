﻿using System;
using System.IO;
using System.Threading.Tasks;
using Maxsys.Core.Services;
using Maxsys.MediaManager.CoreDomain.Interfaces.Services;
using Maxsys.MediaManager.MusicContext.Domain.Interfaces.Mp3;

namespace Maxsys.MediaManager.MusicContext.AppTagLibMono.Services;

/// <summary>
/// Implements <see cref="ISongPropertiesReader"/> using <see href="https://github.com/mono/taglib-sharp">Taglib-Sharp</see>
/// </summary>
public class TagLibMusicPropertiesReader : ServiceBase, ISongPropertiesReader
{
    private readonly IFilePropertiesReader _filePropertiesReader;

    public TagLibMusicPropertiesReader(IFilePropertiesReader filePropertiesReader)
    {
        _filePropertiesReader = filePropertiesReader;
    }

    #region IMusicPropertiesReader Implementation

    public ValueTask<TimeSpan> GetMusicDurationAsync(string mp3Path)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(mp3Path, nameof(mp3Path));

        if (!File.Exists(mp3Path))
        {
            throw new FileNotFoundException(message: null, fileName: mp3Path);
        }

        TimeSpan duration;
        using (var mp3 = TagLib.File.Create(mp3Path))
        {
            duration = mp3.Properties.Duration;
        }

        return ValueTask.FromResult(duration);
    }

    public ValueTask<int> GetMusicBitrateAsync(string mp3Path)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(mp3Path, nameof(mp3Path));

        if (!File.Exists(mp3Path))
        {
            throw new FileNotFoundException(message: null, fileName: mp3Path);
        }

        int bitrate;
        using (var mp3 = TagLib.File.Create(mp3Path))
        {
            bitrate = mp3.Properties.AudioBitrate;
        }

        return ValueTask.FromResult(bitrate);
    }

    #endregion IMusicPropertiesReader Implementation

    #region IFilePropertiesReader Implementation

    public Task<string> GetFileNameWithoutExtensionAsync(string fullPath) => _filePropertiesReader.GetFileNameWithoutExtensionAsync(fullPath);

    public Task<string> GetFileExtensionAsync(string fullPath) => _filePropertiesReader.GetFileExtensionAsync(fullPath);

    public Task<long> GetFileSizeAsync(string fullPath) => _filePropertiesReader.GetFileSizeAsync(fullPath);

    #endregion IFilePropertiesReader Implementation

    #region DIPOSABLE IMPLEMENTATION

    protected override void Dispose(bool disposing)
    {
        if (!_disposed && disposing)
        {
            _filePropertiesReader?.Dispose();
        }
        _disposed = true;
    }

    #endregion DIPOSABLE IMPLEMENTATION
}