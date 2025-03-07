using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace Maxsys.MediaManager.CoreDomain.Helpers;

// TODO move to CORE (update IOHelper)
/// <summary>
/// Provides static methods for file operations like Copy, Move and Delete
/// </summary>
public static class IOHelper2
{
    #region Attributes

    public static void InsertAttribute(ref FileAttributes attributesFlags, FileAttributes insert)
    {
        attributesFlags |= insert;
    }

    public static void RemoveAttribute(ref FileAttributes attributesFlags, FileAttributes remove)
    {
        attributesFlags &= ~remove;
    }

    /// <summary>
    /// Removes the <see cref="FileAttributes.ReadOnly">ReadOnly attribute</see> from the file
    /// </summary>
    /// <param name="filePath">Is the path of the file</param>
    public static void RemoveReadOnlyAttribute(string filePath)
    {
        var attributes = File.GetAttributes(filePath);

        RemoveAttribute(ref attributes, FileAttributes.ReadOnly);

        File.SetAttributes(filePath, attributes);
    }

    /// <summary>
    /// Inserts the <see cref="FileAttributes.ReadOnly">ReadOnly attribute</see> from the file
    /// </summary>
    /// <param name="filePath">Is the path of the file</param>
    public static void InsertReadOnlyAttribute(string filePath)
    {
        var attributes = File.GetAttributes(filePath);

        InsertAttribute(ref attributes, FileAttributes.ReadOnly);

        File.SetAttributes(filePath, attributes);
    }

    #endregion Attributes

    #region File Operations

    /// <summary>
    /// Moves an existing file to a new file and sets <see cref="FileAttributes.ReadOnly">ReadOnly attribute</see>.
    /// Overwritting a file of the same name is not allowed.<para/>
    /// Creates the directory of the destination file name if it doesn't exists.
    /// </summary>
    /// <param name="sourceFileName">The file to move.</param>
    /// <param name="destFileName">The name of the destination file. This cannot be a directory or an existing file.</param>
    /// <param name="setAsReadOnly">if true, sets destination file to ReadOnly Attribute.
    /// if false, then the ReadOnly Attribute will be not changed. Default is true.</param>
    /// <returns>a <see cref="OperationResult"/> of the operation.</returns>
    public static OperationResult MoveFile(string sourceFileName, string destFileName, bool setAsReadOnly = true)
    {
        if (sourceFileName == destFileName)
        {
            return new(new Notification("Origin and destination files are the same.", ResultTypes.Warning));
        }

        var result = new OperationResult();

        try
        {
            RemoveReadOnlyAttribute(sourceFileName);

            var copyResult = CopyFile(sourceFileName, destFileName, setAsReadOnly);
            if (copyResult.IsValid)
            {
                var delete = DeleteFile(sourceFileName);
                if (!delete.IsValid)
                {
                    var notification = delete.Notifications.First();
                    result.AddNotification(new Notification(notification.Message, notification.Details, ResultTypes.Info));
                }
            }
            else
            {
                result.AddNotifications(copyResult.Notifications);
            }
        }
        catch (Exception ex)
        {
            result.AddException(ex);
        }

        return result;
    }

    /// <summary>
    /// Moves or overwrite an existing file to a new file and sets <see cref="FileAttributes.ReadOnly">ReadOnly attribute</see>.
    /// If destination file exists, will be deleted.<para/>
    /// Creates the directory of the destination file name if it doesn't exists.
    /// </summary>
    /// <param name="sourceFileName">The file to move.</param>
    /// <param name="destFileName">The name of the destination file. This cannot be a directory or an existing file.</param>
    /// <param name="setAsReadOnly">if true, sets destination file to ReadOnly Attribute.
    /// if false, then the ReadOnly Attribute will be not changed. Default is true.</param>
    /// <returns>a <see cref="OperationResult"/> of the operation.</returns>
    public static OperationResult MoveOrOverwriteFile(string sourceFileName, string destFileName, bool setAsReadOnly = true)
    {
        if (sourceFileName == destFileName)
        {
            return new(new Notification("Origin and destination files are the same.", ResultTypes.Warning));
        }

        var deleteResult = DeleteFile(destFileName);

        return deleteResult.IsValid
            ? MoveFile(sourceFileName, destFileName, setAsReadOnly)
            : deleteResult;
    }

    /// <summary>
    /// Deletes the specified file if exists. Ignores <see cref="FileAttributes.ReadOnly">ReadOnly attribute</see>
    /// </summary>
    /// <param name="fileName">The name of the file to be deleted. Wildcard characters are not supported.</param>
    /// <returns></returns>
    public static OperationResult DeleteFile(string fileName)
    {
        var result = new OperationResult();
        try
        {
            if (File.Exists(fileName))
            {
                RemoveReadOnlyAttribute(fileName);
                File.Delete(fileName);
            }
        }
        catch (Exception ex)
        {
            result.AddException(ex);
        }

        return result;
    }

    /// <summary>
    /// Copies an existing file to a new file and sets <see cref="FileAttributes.ReadOnly">ReadOnly attribute</see>.
    /// Overwritting a file of the same name is not allowed.<para/>
    /// Creates the directory of the destination file name if it doesn't exists.
    /// </summary>
    /// <param name="sourceFileName">The file to copy.</param>
    /// <param name="destFileName">The name of the destination file. This cannot be a directory or an existing file.</param>
    /// <param name="setAsReadOnly">if true, sets destination file to ReadOnly Attribute.
    /// if false, then the ReadOnly Attribute will be not changed. Default is true.</param>
    /// <returns>a <see cref="OperationResult"/> of the operation.</returns>
    public static OperationResult CopyFile(string sourceFileName, string destFileName, bool setAsReadOnly = true)
    {
        if (sourceFileName == destFileName)
        {
            return new(new Notification("Origin and destination files are the same.", ResultTypes.Warning));
        }

        var result = new OperationResult();

        if (!File.Exists(destFileName))
        {
            try
            {
                _ = Directory.CreateDirectory(Path.GetDirectoryName(destFileName)!);
                File.Copy(sourceFileName, destFileName);
                if (setAsReadOnly)
                {
                    InsertReadOnlyAttribute(destFileName);
                }
            }
            catch (Exception ex)
            {
                result.AddException(ex);
            }
        }
        else
        {
            result.AddNotification(new("Destination file already exists."));
        }

        return result;
    }

    #endregion File Operations

    #region Async File Operations

    /// <summary>
    /// Asynchronously moves an existing file to a new file and sets <see cref="FileAttributes.ReadOnly">ReadOnly attribute</see>.
    /// Overwritting a file of the same name is not allowed.<para/>
    /// Creates the directory of the destination file name if it doesn't exists.
    /// </summary>
    /// <param name="sourceFileName">The file to move.</param>
    /// <param name="destFileName">The name of the destination file. This cannot be a directory or an existing file.</param>
    /// <param name="setAsReadOnly">if true, sets destination file to ReadOnly Attribute.
    /// if false, then the ReadOnly Attribute will be not changed. Default is true.</param>
    /// <returns>a <see cref="OperationResult"/> of the operation.</returns>
    public static async ValueTask<OperationResult> MoveFileAsync(string sourceFileName, string destFileName, bool setAsReadOnly = true, CancellationToken cancellationToken = default)
    {
        if (sourceFileName == destFileName)
        {
            return new(new Notification("Origin and destination files are the same.", ResultTypes.Warning));
        }

        var result = new OperationResult();

        try
        {
            RemoveReadOnlyAttribute(sourceFileName);

            var copyResult = await CopyFileAsync(sourceFileName, destFileName, setAsReadOnly, cancellationToken);
            if (copyResult.IsValid)
            {
                var deleteResult = await DeleteFileAsync(sourceFileName, cancellationToken);
                if (!deleteResult.IsValid)
                {
                    var notification = deleteResult.Notifications.First();
                    result.AddNotification(new Notification(notification.Message, notification.Details, ResultTypes.Info));
                }
            }
            else
            {
                result.AddNotifications(copyResult.Notifications);
            }
        }
        catch (Exception ex)
        {
            result.AddException(ex);
        }

        return result;
    }

    /// <summary>
    /// Asynchronously moves or overwrite an existing file to a new file and sets <see cref="FileAttributes.ReadOnly">ReadOnly attribute</see>.
    /// If destination file exists, will be deleted.<para/>
    /// Creates the directory of the destination file name if it doesn't exists.
    /// </summary>
    /// <param name="sourceFileName">The file to move.</param>
    /// <param name="destFileName">The name of the destination file. This cannot be a directory or an existing file.</param>
    /// <param name="setAsReadOnly">if true, sets destination file to ReadOnly Attribute.
    /// if false, then the ReadOnly Attribute will be not changed. Default is true.</param>
    /// <returns>a <see cref="OperationResult"/> of the operation.</returns>
    public static async ValueTask<OperationResult> MoveOrOverwriteFileAsync(string sourceFileName, string destFileName, bool setAsReadOnly = true)
    {
        if (sourceFileName == destFileName)
        {
            return new(new Notification("Origin and destination files are the same.", ResultTypes.Warning));
        }

        var deleteResult = await DeleteFileAsync(destFileName);

        return deleteResult.IsValid
            ? await MoveFileAsync(sourceFileName, destFileName, setAsReadOnly)
            : deleteResult;
    }

    /// <summary>
    /// Asynchronously copies an existing file to a new file and sets <see cref="FileAttributes.ReadOnly">ReadOnly attribute</see>.
    /// Overwritting a file of the same name is not allowed.<para/>
    /// Creates the directory of the destination file name if it doesn't exists.
    /// </summary>
    /// <param name="sourceFileName">The file to copy.</param>
    /// <param name="destFileName">The name of the destination file. This cannot be a directory or an existing file.</param>
    /// <param name="setAsReadOnly">if true, sets destination file to ReadOnly Attribute.
    /// if false, then the ReadOnly Attribute will be not changed. Default is true.</param>
    /// <returns>a <see cref="OperationResult"/> of the operation.</returns>
    public static async ValueTask<OperationResult> CopyFileAsync(string sourceFileName, string destFileName, bool setAsReadOnly = true, CancellationToken cancellationToken = default)
    {
        if (sourceFileName == destFileName)
        {
            return new(new Notification("Origin and destination files are the same.", ResultTypes.Warning));
        }

        var result = new OperationResult();

        if (!File.Exists(destFileName))
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(destFileName)!);

                await InternalCopyFileAsync(sourceFileName, destFileName, cancellationToken);

                if (setAsReadOnly) InsertReadOnlyAttribute(destFileName);
            }
            catch (Exception ex)
            {
                result.AddException(ex);
            }
        }
        else
        {
            result.AddNotification(new("Destination file already exists."));
        }

        return result;
    }

    /// <summary>
    /// Asynchronously deletes the specified file if exists. Ignores <see cref="FileAttributes.ReadOnly">ReadOnly attribute</see>
    /// </summary>
    /// <param name="fileName">The name of the file to be deleted. Wildcard characters are not supported.</param>
    /// <returns></returns>
    public static async ValueTask<OperationResult> DeleteFileAsync(string fileName, CancellationToken cancellationToken = default)
    {
        var result = new OperationResult();
        try
        {
            if (File.Exists(fileName))
            {
                RemoveReadOnlyAttribute(fileName);

                await InternalDeleteAsync(fileName, cancellationToken);
            }
        }
        catch (Exception ex)
        {
            result.AddException(ex);
        }

        return result;
    }

    #region Internal operations

    private static async Task InternalDeleteAsync(string fileName, CancellationToken cancellationToken)
    {
        await Task.Factory.StartNew(() =>
        {
            using (_ = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.None, 1, FileOptions.DeleteOnClose | FileOptions.Asynchronous))
            { /*delete on close*/ }
        }, cancellationToken);
    }

    private static async Task InternalCopyFileAsync(string sourceFileName, string destFileName, CancellationToken cancellationToken)
    {
        var fileOptions = FileOptions.Asynchronous | FileOptions.SequentialScan;
        var bufferSize = 4096;

        using (var srcStream = new FileStream(sourceFileName, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize, fileOptions))
        {
            using (var dstStream = new FileStream(destFileName, FileMode.CreateNew, FileAccess.Write, FileShare.None, bufferSize, fileOptions))
            {
                await srcStream.CopyToAsync(dstStream, bufferSize, cancellationToken);
            }
        }
    }

    #endregion Internal operations

    #endregion Async File Operations

    #region Path Operations

    public static string RemoveInvalidDirectoryChars(string directoryPath)
    {
        var path = new StringBuilder(directoryPath);

        var invalidPathChars = Path.GetInvalidPathChars();
        foreach (char c in invalidPathChars)
        {
            path.Replace(c.ToString(), string.Empty);
        }

        return new(path.ToString());
    }

    public static string ReplaceInvalidDirectoryChars(string directoryPath)
    {
        var path = new StringBuilder(directoryPath);
        path.Replace("<", "(")
            .Replace(">", ")")
            .Replace("|", "-")
            .Replace("/", "-")
            .Replace("\n", "")
            .Replace("\r", "")
            .Replace("\t", "")
            .Replace(":", " - ");

        return new(path.ToString());
    }

    public static string ReplaceAndRemoveInvalidDirectoryChars(string directoryPath)
        => RemoveInvalidDirectoryChars(ReplaceInvalidDirectoryChars(directoryPath));

    public static string RemoveInvalidFileNameChars(string filePath)
    {
        var path = new StringBuilder(filePath);

        var invalidFileChars = Path.GetInvalidFileNameChars();
        foreach (char c in invalidFileChars)
        {
            path.Replace(c.ToString(), string.Empty);
        }

        return new(path.ToString());
    }

    public static string ReplaceInvalidFileNameChars(string filePath)
    {
        var path = new StringBuilder(filePath);
        path.Replace("<", "(")
            .Replace(">", ")")
            .Replace("|", "-")
            .Replace("/", "-")
            .Replace("\\", "-")
            .Replace("\"", "-")
            .Replace("\n", "")
            .Replace("\r", "")
            .Replace("\t", "")
            .Replace(":", " ");

        return new(path.ToString());
    }

    public static string ReplaceAndRemoveInvalidFileNameChars(string filePath)
        => RemoveInvalidFileNameChars(ReplaceInvalidFileNameChars(filePath));

    #endregion Path Operations
}