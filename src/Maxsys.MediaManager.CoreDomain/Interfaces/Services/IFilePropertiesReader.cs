using System;

namespace Maxsys.MediaManager.CoreDomain.Interfaces.Services
{
    public interface IFilePropertiesReader : IDisposable
    {
        /// <summary>
        /// Get the file name without extension.
        /// </summary>
        /// <example>'D:\SomeFolder\SomeFile.jpg' returns 'SomeFile'</example>
        /// <param name="fullPath">the full path of the file</param>
        /// <returns><see cref="string"/> that represents a name of a file</returns>
        string GetFileNameWithoutExtension(string fullPath);

        /// <summary>
        /// Returns the extension (including the period ".") of the specified path string.
        /// </summary>
        /// <param name="fullPath">The path string from which to get the extension.</param>
        /// <returns>The extension of the specified path (including the period "."), or null, or Empty. If path is null, GetExtension(String) returns null. If path does not have extension information, GetExtension(String) returns Empty.</returns>
        string GetFileExtension(string fullPath);

        /// <summary>
        /// Get the file size in bytes
        /// </summary>
        /// <param name="fullPath">the full path of the file</param>
        /// <returns>a file size in bytes</returns>
        long GetFileSize(string fullPath);
    }
}