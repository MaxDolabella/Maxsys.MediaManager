using System;

namespace Maxsys.MediaManager.CoreDomain.Interfaces;

public interface IAuditableEntity
{
    /// <summary>
    /// Automatic value
    /// </summary>
    DateTime CreatedAt { get; }

    /// <summary>
    /// Automatic value
    /// </summary>
    DateTime? LastUpdateAt { get; }

    void SetCreatedAt(DateTime date);

    void SetLastUpdateAt(DateTime date);
}