﻿using Maxsys.Core.Extensions;

namespace Maxsys.MediaManager.MusicContext.Domain.DTO;

public struct Id3v2PlaylistItemDTO : IEquatable<Id3v2PlaylistItemDTO?>, IEquatable<Id3v2DTO?>
{
    #region CTOR

    /// <summary>
    /// Data transfer object to write tags on playlist musics.
    /// </summary>
    /// <param name="fullPath">is the path where is the file.</param>
    /// <param name="playlistName">is the playlist name.</param>
    /// <param name="songTitle">is the music title.</param>
    /// <param name="trackOrder">is the music order in playlist.</param>
    /// <param name="stars10">is the music rating number (0-10).</param>
    /// <param name="coverPicture">is the cover picture from music album.</param>
    public Id3v2PlaylistItemDTO(string fullPath, string playlistName, string songTitle, uint trackOrder, byte stars10, string genre, byte[] coverPicture)
    {
        FullPath = fullPath;

        Title = songTitle; // $"{trackOrder:000} {songTitle}";
        PlaylistName = "_" + playlistName;
        Stars10 = stars10;
        TrackNumber = trackOrder;
        Genre = genre;
        CoverPicture = coverPicture;
    }

    public static readonly Id3v2PlaylistItemDTO Empty = new Id3v2PlaylistItemDTO();

    #endregion CTOR

    #region PROPS

    public string FullPath { get; }

    public string Title { get; }
    public string PlaylistName { get; }
    public byte Stars10 { get; }
    public uint TrackNumber { get; }
    public string Genre { get; }
    public byte[] CoverPicture { get; }

    #endregion PROPS

    #region Equals & operators

    public bool Equals(Id3v2PlaylistItemDTO? other)
    {
        return other is not null
            && FullPath == other?.FullPath
            && Title == other?.Title
            && PlaylistName == other?.PlaylistName
            && Stars10 == other?.Stars10
            && TrackNumber == other?.TrackNumber
            && Genre == other?.Genre
            && CoverPicture.ArrayEquals(other?.CoverPicture);
    }

    public bool Equals(Id3v2DTO? other)
    {
        return other is not null
            && FullPath == other?.FullPath
            && Title == other?.Title
            && PlaylistName == other?.Artist
            && Stars10 == other?.Rating10
            && TrackNumber == other?.TrackNumber
            && Genre == other?.Genre
            && CoverPicture.ArrayEquals(other?.CoverPicture);
    }

    public static bool operator ==(Id3v2PlaylistItemDTO a, Id3v2PlaylistItemDTO b) => a.Equals(b);

    public static bool operator !=(Id3v2PlaylistItemDTO a, Id3v2PlaylistItemDTO b) => !(a == b);

    public static bool operator ==(Id3v2PlaylistItemDTO a, Id3v2DTO b) => a.Equals(b);

    public static bool operator !=(Id3v2PlaylistItemDTO a, Id3v2DTO b) => !(a == b);

    public static bool operator ==(Id3v2DTO a, Id3v2PlaylistItemDTO b) => b.Equals(a);

    public static bool operator !=(Id3v2DTO a, Id3v2PlaylistItemDTO b) => !(b == a);

    public override int GetHashCode() => HashCode.Combine(FullPath, Title, PlaylistName, Stars10, TrackNumber, Genre, CoverPicture);

    public override bool Equals(object? obj) => obj is Id3v2PlaylistItemDTO tags && Equals(tags);

    public override string ToString() => GetType().Name + " [Title=" + Title + "]";

    #endregion Equals & operators
}