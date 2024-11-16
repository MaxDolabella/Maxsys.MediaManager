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
    public string GetFileNameWithoutExtension(string fullPath)
        => Path.GetFileNameWithoutExtension(fullPath);

    /// <summary>
    /// Get the file size in bytes
    /// </summary>
    /// <param name="fullPath">the full path of the file</param>
    /// <returns>a file size in bytes</returns>
    public long GetFileSize(string fullPath)
        => File.Exists(fullPath) ? _ = new FileInfo(fullPath).Length : 0;

    /// <summary>
    /// Returns the extension (including the period ".") of the specified path string.
    /// </summary>
    /// <param name="fullPath">The path string from which to get the extension.</param>
    /// <returns>The extension of the specified path (including the period "."), or null, or Empty. If path is null, GetExtension(String) returns null. If path does not have extension information, GetExtension(String) returns Empty.</returns>
    public string GetFileExtension(string fullPath)
        => Path.GetExtension(fullPath);
}