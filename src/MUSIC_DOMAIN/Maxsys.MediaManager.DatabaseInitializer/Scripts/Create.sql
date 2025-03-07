CREATE TABLE [Catalog] (
    [Id] uniqueidentifier NOT NULL,
    [Name] varchar(50) NOT NULL,
    CONSTRAINT [PK_Catalog] PRIMARY KEY ([Id])
);
GO


CREATE TABLE [Composer] (
    [Id] uniqueidentifier NOT NULL,
    [Name] varchar(50) NOT NULL,
    CONSTRAINT [PK_Composer] PRIMARY KEY ([Id])
);
GO


CREATE TABLE [Playlist] (
    [Id] uniqueidentifier NOT NULL,
    [Name] varchar(50) NOT NULL,
    [SpotifyID] varchar(50) NULL,
    CONSTRAINT [PK_Playlist] PRIMARY KEY ([Id])
);
GO


CREATE TABLE [Artist] (
    [Id] uniqueidentifier NOT NULL,
    [CatalogId] uniqueidentifier NOT NULL,
    [Name] varchar(50) NOT NULL,
    [SpotifyID] varchar(50) NULL,
    CONSTRAINT [PK_Artist] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Artist_Catalog_CatalogId] FOREIGN KEY ([CatalogId]) REFERENCES [Catalog] ([Id]) ON DELETE CASCADE
);
GO


CREATE TABLE [Album] (
    [Id] uniqueidentifier NOT NULL,
    [ArtistId] uniqueidentifier NOT NULL,
    [Directory] varchar(160) NOT NULL,
    [Name] varchar(50) NOT NULL,
    [Year] smallint NULL,
    [Genre] varchar(50) NOT NULL,
    [Type] tinyint NOT NULL,
    [Cover] varbinary(max) NOT NULL,
    [SpotifyID] varchar(50) NULL,
    CONSTRAINT [PK_Album] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Album_Artist_ArtistId] FOREIGN KEY ([ArtistId]) REFERENCES [Artist] ([Id]) ON DELETE CASCADE
);
GO


CREATE TABLE [Song] (
    [Id] uniqueidentifier NOT NULL,
    [AlbumId] uniqueidentifier NOT NULL,
    [Title] varchar(100) NOT NULL,
    [TrackNumber] int NULL,
    [Lyrics] varchar(5000) NULL,
    [Comments] varchar(300) NULL,
    [SpotifyID] varchar(50) NULL,
    [ISRC] char(12) NULL,
    [Classification_Rating] int NOT NULL,
    [SongDetails_IsBonusTrack] bit NOT NULL,
    [SongDetails_VocalGender] tinyint NOT NULL,
    [SongDetails_CoveredArtist] varchar(50) NULL,
    [SongDetails_FeaturedArtist] varchar(50) NULL,
    [SongProperties_Duration] time NOT NULL,
    [SongProperties_BitRate] int NOT NULL,
    [MediaFile_Path] varchar(100) NOT NULL,
    [MediaFile_OriginalFile] varchar(100) NOT NULL,
    [MediaFile_FileSize] bigint NOT NULL,
    [MediaFile_CreatedAt] datetime2 NOT NULL,
    [MediaFile_LastUpdateAt] datetime2 NULL,
    CONSTRAINT [PK_Song] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Song_Album_AlbumId] FOREIGN KEY ([AlbumId]) REFERENCES [Album] ([Id]) ON DELETE CASCADE
);
GO


CREATE TABLE [ComposerSong] (
    [ComposerId] uniqueidentifier NOT NULL,
    [SongId] uniqueidentifier NOT NULL,
    CONSTRAINT [PK_ComposerSong] PRIMARY KEY ([ComposerId], [SongId]),
    CONSTRAINT [FK_ComposerSong_Composer_ComposerId] FOREIGN KEY ([ComposerId]) REFERENCES [Composer] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_ComposerSong_Song_SongId] FOREIGN KEY ([SongId]) REFERENCES [Song] ([Id]) ON DELETE CASCADE
);
GO


CREATE TABLE [PlaylistItem] (
    [PlaylistId] uniqueidentifier NOT NULL,
    [SongId] uniqueidentifier NOT NULL,
    [Order] smallint NULL,
    CONSTRAINT [PK_PlaylistItem] PRIMARY KEY ([PlaylistId], [SongId]),
    CONSTRAINT [FK_PlaylistItem_Playlist_PlaylistId] FOREIGN KEY ([PlaylistId]) REFERENCES [Playlist] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_PlaylistItem_Song_SongId] FOREIGN KEY ([SongId]) REFERENCES [Song] ([Id]) ON DELETE CASCADE
);
GO


CREATE UNIQUE INDEX [AK_Album_ArtistAlbumName] ON [Album] ([ArtistId], [Name]);
GO


CREATE UNIQUE INDEX [AK_Artist_Name_Catalog] ON [Artist] ([Name], [CatalogId]);
GO


CREATE INDEX [IX_Artist_CatalogId] ON [Artist] ([CatalogId]);
GO


CREATE UNIQUE INDEX [AK_Catalog_Name] ON [Catalog] ([Name]);
GO


CREATE UNIQUE INDEX [AK_Composer_Name] ON [Composer] ([Name]);
GO


CREATE INDEX [IX_ComposerSong_SongId] ON [ComposerSong] ([SongId]);
GO


CREATE UNIQUE INDEX [AK_Playlist_Name] ON [Playlist] ([Name]);
GO


CREATE INDEX [IX_PlaylistItem_SongId] ON [PlaylistItem] ([SongId]);
GO


CREATE UNIQUE INDEX [AK_Music_FullPath] ON [Song] ([MediaFile_Path]);
GO


CREATE INDEX [IX_Song_AlbumId] ON [Song] ([AlbumId]);
GO