SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[view_Song]
AS
SELECT 
    cat.Name [Catalog],
	art.Name Artist,
    alb.Name Album,
    CASE alb.AlbumType 
        WHEN 0 THEN 'Undefined'
        WHEN 1 THEN 'Studio'
        WHEN 2 THEN 'Live'
        WHEN 3 THEN 'Compilation'
        WHEN 4 THEN 'Bootleg'
        WHEN 9 THEN 'Various'
        WHEN 10 THEN 'Others'
    END AS AlbumType,
    sng.TrackNumber Track,
	sng.Title Song, 
    CASE sng.SongDetails_VocalGender 
        WHEN 0 THEN 'Undefined'
        WHEN 1 THEN 'Male'
        WHEN 2 THEN 'Female'
        WHEN 3 THEN 'Mixed'
    END AS VocalGender,
	sng.Classification_Rating Rating,
	sng.SongProperties_Duration Duration,
	sng.MediaFile_FileSize FileSize,
	-- Ids
    cat.Id CatalogId,
	art.Id ArtistId,
	alb.Id AlbumId,
	sng.Id SongId
FROM [Songs] sng
JOIN [Albums] alb ON alb.Id = sng.AlbumId
JOIN [Artists] art ON art.Id = alb.ArtistId
JOIN [Catalogs] cat ON cat.Id = art.CatalogId
GO
