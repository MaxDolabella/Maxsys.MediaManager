using System;
using System.IO;
using System.Text.RegularExpressions;

namespace Maxsys.MediaManager.CoreDomain_;

// Created by https://claude.ai/
public struct SystemPath : IEquatable<SystemPath>
{
    public const int MAXIMUM_LENGTH = 256;
    private readonly string _path;
    private readonly string _originalString;

    public SystemPath(string path)
    {
        _originalString = path;
        _path = NormalizePath(path ?? string.Empty);
    }

    private static string NormalizePath(string path)
    {
        // Replace backslashes with forward slashes
        return path.Replace('\\', '/').TrimEnd('/');
    }

    public bool IsFile => !string.IsNullOrEmpty(_path) && Path.HasExtension(_path);

    public bool IsFullPath => !string.IsNullOrEmpty(_path) &&
                              (Path.IsPathRooted(_path) ||
                               Regex.IsMatch(_path, @"^[a-zA-Z]:/"));

    public string Extension => IsFile ? Path.GetExtension(_path) : string.Empty;

    public string FileName => IsFile ? Path.GetFileName(_path) : string.Empty;

    public string DirectoryName => Path.GetDirectoryName(_path)?.Replace('\\', '/') ?? string.Empty;
    public string OriginalString => _originalString;

    public SystemPath Combine(string path)
    {
        if (string.IsNullOrEmpty(path))
            return this;

        var normalizedPath = NormalizePath(path);

        if (Path.IsPathRooted(normalizedPath) || Regex.IsMatch(normalizedPath, @"^[a-zA-Z]:/"))
            return new SystemPath(normalizedPath);

        return new SystemPath(string.IsNullOrEmpty(_path) ? normalizedPath : $"{_path}/{normalizedPath}");
    }

    public SystemPath Combine(SystemPath path)
    {
        return Combine(path.ToString());
    }

    public override string ToString() => _path;

    public bool Exists() => Path.Exists(_path);

    public int Length() => _path.Length;

    public string ToSystemPath() => _path.Replace('/', Path.DirectorySeparatorChar);

    public override bool Equals(object? obj) => obj is SystemPath other && Equals(other);

    public bool Equals(SystemPath other) => string.Equals(_path, other._path, StringComparison.OrdinalIgnoreCase);

    public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(_path);

    public static bool operator ==(SystemPath left, SystemPath right) => left.Equals(right);

    public static bool operator !=(SystemPath left, SystemPath right) => !left.Equals(right);

    public static explicit operator string(SystemPath path) => path.ToString();
                  
    public static explicit operator SystemPath(string path) => new SystemPath(path);
}