using System;
using Maxsys.Core.Entities;
using Maxsys.MediaManager.CoreDomain.Interfaces.Services;

namespace Maxsys.MediaManager.CoreDomain;

public abstract class MediaFile : Entity<Guid>
{
    #region PROPERTIES

    /// <summary>
    /// Path limit is 248 characters.
    /// Path+Filename limit is 260 characters.
    /// </summary>
    public string FullPath { get; protected set; }

    public string OriginalFileName { get; protected set; } // private set?

    public long FileSize { get; protected set; } // private set?

    /// <summary>
    /// Automatic value
    /// </summary>
    public DateTime CreatedDate { get; protected set; }

    /// <summary>
    /// Automatic value
    /// </summary>
    public DateTime UpdatedDate { get; protected set; }

    #endregion PROPERTIES

    #region CONSTRUCTORS

    protected MediaFile()
    { }

    protected MediaFile(Guid id, string fullPath, string originalFileName, long fileSize)
    {
        Id = id;
        FullPath = fullPath;
        OriginalFileName = originalFileName;
        FileSize = fileSize;
    }

    #endregion CONSTRUCTORS

    public void SetCreatedDate(DateTime date)
        => CreatedDate = date;

    public void SetUpdatedDate(DateTime date)
     => UpdatedDate = date;

    public void UpdateFilePropertiesFrom(IFilePropertiesReader propertiesReader, string fileToUpdateFrom)
    {
        OriginalFileName = propertiesReader.GetFileNameWithoutExtension(fileToUpdateFrom);
        FileSize = propertiesReader.GetFileSize(fileToUpdateFrom);
    }
}