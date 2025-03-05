SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[view_Playlist]
AS
SELECT
	ply.Name Playlist,
	art.Name Artist,
	sng.Title Song,
	sng.Classification_Rating Rating,
	sng.SongProperties_Duration Duration,
	sng.MediaFile_FileSize FileSize,
	pit.[Order] [Order],
	-- Ids
	ply.Id PlaylistId,
	art.Id ArtistId,
	alb.Id AlbumId,
	sng.Id SongId
FROM [Playlists] ply
JOIN [PlaylistItems] pit ON pit.PlaylistId = ply.Id
JOIN [Songs] sng ON sng.Id = pit.SongId
JOIN [Albums] alb ON alb.Id = sng.AlbumId
JOIN [Artists] art ON art.Id = alb.ArtistId
GO
