using System;
using System.IO;
using System.Text.RegularExpressions;

namespace Maxsys.Core;

// Created by https://claude.ai/
public struct SysPath : IEquatable<SysPath>
{
    public const int MAXIMUM_LENGTH = 256;
    private readonly string _path;
    private readonly string _originalString;

    public SysPath(string path)
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

    public SysPath Combine(string path)
    {
        if (string.IsNullOrEmpty(path))
            return this;

        var normalizedPath = NormalizePath(path);

        if (Path.IsPathRooted(normalizedPath) || Regex.IsMatch(normalizedPath, @"^[a-zA-Z]:/"))
            return new SysPath(normalizedPath);

        return new SysPath(string.IsNullOrEmpty(_path) ? normalizedPath : $"{_path}/{normalizedPath}");
    }

    public SysPath Combine(SysPath path)
    {
        return Combine(path.ToString());
    }

    public override string ToString() => _path;

    public bool Exists() => Path.Exists(_path);

    public int Length() => _path.Length;

    public string ToSystemPath() => _path.Replace('/', Path.DirectorySeparatorChar);

    public override bool Equals(object? obj) => obj is SysPath other && Equals(other);

    public bool Equals(SysPath other) => string.Equals(_path, other._path, StringComparison.OrdinalIgnoreCase);

    public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(_path);

    public static bool operator ==(SysPath left, SysPath right) => left.Equals(right);

    public static bool operator !=(SysPath left, SysPath right) => !left.Equals(right);

    public static explicit operator string(SysPath path) => path.ToString();
                  
    public static explicit operator SysPath(string path) => new SysPath(path);
}