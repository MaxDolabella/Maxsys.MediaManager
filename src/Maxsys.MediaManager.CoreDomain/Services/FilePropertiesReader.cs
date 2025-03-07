using System;
using System.IO;
using Maxsys.Core.Services;
using Maxsys.MediaManager.CoreDomain.Interfaces.Services;

namespace Maxsys.MediaManager.CoreDomain.Services;

public class FilePropertiesReader : ServiceBase, IFilePropertiesReader
{
    /// <summary>
    /// Get the file name without extension.
    /// </summary>
    /// <example>'D:\SomeFolder\SomeFile.jpg' returns 'SomeFile'</example>
    /// <param name="fullPath">the full path of the file</param>
    /// <returns>string that represents a name of a file</returns>
    public Task<string> GetFileNameWithoutExtensionAsync(string fullPath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(fullPath, nameof(fullPath));

        return Task.FromResult(Path.GetFileNameWithoutExtension(fullPath));
    }

    /// <summary>
    /// Get the file size in bytes
    /// </summary>
    /// <param name="fullPath">the full path of the file</param>
    /// <returns>a file size in bytes</returns>
    public Task<long> GetFileSizeAsync(string fullPath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(fullPath, nameof(fullPath));

        if (!File.Exists(fullPath))
        {
            throw new FileNotFoundException(message: null, fileName: fullPath);
        }

        return Task.FromResult(_ = new FileInfo(fullPath).Length);
    }

    /// <summary>
    /// Returns the extension (including the period ".") of the specified path string.
    /// </summary>
    /// <param name="fullPath">The path string from which to get the extension.</param>
    /// <returns>The extension of the specified path (including the period "."), or null, or Empty. If path is null, GetExtension(String) returns null. If path does not have extension information, GetExtension(String) returns Empty.</returns>
    public Task<string> GetFileExtensionAsync(string fullPath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(fullPath, nameof(fullPath));

        return Task.FromResult(Path.GetExtension(fullPath));
    }
}