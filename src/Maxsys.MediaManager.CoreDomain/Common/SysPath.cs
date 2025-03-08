using System;
using IOPath = System.IO.Path;
using System.Text.RegularExpressions;

namespace Maxsys.Core;

// Created by https://claude.ai/
public struct SysPath : IEquatable<SysPath>
{
    #region Fields & Consts

    public const int MAXIMUM_LENGTH = 256;
    private readonly string _path;
    private readonly string _originalString;

    #endregion Fields & Consts

    public SysPath(string path)
    {
        _originalString = path;
        _path = NormalizePath(path ?? string.Empty);
    }

    #region Properties

    public readonly bool IsFile => !IsPathEmpty() && IOPath.HasExtension(_path);

    public readonly bool IsFullPath
        => !IsPathEmpty()
        && (IOPath.IsPathRooted(_path) || Regex.IsMatch(_path, @"^[a-zA-Z]:/"));

    public readonly string Path => _path;
    public readonly string Extension => IsFile ? IOPath.GetExtension(_path) : string.Empty;

    public readonly string FileName => IsFile ? IOPath.GetFileName(_path) : string.Empty;

    public readonly string DirectoryName => IOPath.GetDirectoryName(_path)?.Replace('\\', '/') ?? string.Empty;
    public readonly string OriginalString => _originalString;

    #endregion Properties

    #region Methods - Class

    private readonly bool IsPathEmpty() => string.IsNullOrWhiteSpace(_path);

    private static string NormalizePath(string path)
    {
        // Replace backslashes with forward slashes
        return path.Replace('\\', '/').TrimEnd('/');
    }

    public SysPath Combine(string path)
    {
        if (IsPathEmpty())
            return this;

        var normalizedPath = NormalizePath(path);

        if (IOPath.IsPathRooted(normalizedPath) || Regex.IsMatch(normalizedPath, @"^[a-zA-Z]:/"))
            return new SysPath(normalizedPath);

        return new SysPath(IsPathEmpty() ? normalizedPath : $"{_path}/{normalizedPath}");
    }

    public SysPath Combine(SysPath path)
    {
        return Combine(path.ToString());
    }

    public readonly bool Exists() => IOPath.Exists(_path);

    public readonly int Length() => _path.Length;

    #endregion Methods - Class

    #region Methods - System

    public override readonly string ToString() => _path.Replace('/', IOPath.DirectorySeparatorChar);

    public override readonly bool Equals(object? obj) => obj is SysPath other && Equals(other);

    public readonly bool Equals(SysPath other) => string.Equals(_path, other._path, StringComparison.OrdinalIgnoreCase);

    public override readonly int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(_path);

    #endregion Methods - System

    #region Operators

    public static bool operator ==(SysPath left, SysPath right) => left.Equals(right);

    public static bool operator !=(SysPath left, SysPath right) => !left.Equals(right);

    public static explicit operator string(SysPath path) => path.ToString();

    public static explicit operator SysPath(string path) => new SysPath(path);

    #endregion Operators
}