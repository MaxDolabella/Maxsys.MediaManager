using System;
using Maxsys.Core.Entities;
using Maxsys.MediaManager.CoreDomain.Interfaces;
using Maxsys.MediaManager.CoreDomain.Interfaces.Services;

namespace Maxsys.MediaManager.CoreDomain;

public abstract class MediaFile : Entity<Guid>, IAuditableEntity
{
    #region PROPERTIES

    /// <summary>
    /// Path limit is 248 characters.
    /// Path+Filename limit is 260 characters.
    /// </summary>
    public SysPath Path { get; protected set; }

    public string OriginalFile { get; protected set; } // private set?

    public long FileSize { get; protected set; } // private set?

    /// <summary>
    /// Automatic value
    /// </summary>
    public DateTime CreatedAt { get; protected set; }

    /// <summary>
    /// Automatic value
    /// </summary>
    public DateTime? LastUpdateAt { get; protected set; }

    #endregion PROPERTIES

    #region CONSTRUCTORS

    protected MediaFile()
    { }

    protected MediaFile(Guid id, string fullPath, string originalFileName, long fileSize)
    {
        Id = id;
        Path = new SysPath(fullPath);
        OriginalFile = new(originalFileName);
        FileSize = fileSize;
    }

    #endregion CONSTRUCTORS

    public void SetCreatedAt(DateTime date) => CreatedAt = date;
    public void SetLastUpdateAt(DateTime date) => LastUpdateAt = date;

    public async Task UpdateFilePropertiesFromAsync(IFilePropertiesReader propertiesReader, string fileToUpdateFrom)
    {
        OriginalFile = new(await propertiesReader.GetFileNameWithoutExtensionAsync(fileToUpdateFrom));
        FileSize = await propertiesReader.GetFileSizeAsync(fileToUpdateFrom);
    }
}