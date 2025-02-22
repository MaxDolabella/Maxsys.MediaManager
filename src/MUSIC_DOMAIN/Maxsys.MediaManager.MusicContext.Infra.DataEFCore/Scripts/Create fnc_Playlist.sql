SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE FUNCTION [dbo].[fnc_Playlist](@playlistId UNIQUEIDENTIFIER = NULL, @playlistName VARCHAR(100) = NULL)
RETURNS @returntable TABLE 
(
    PlaylistId UNIQUEIDENTIFIER,
	Playlist VARCHAR(100),
	Artist VARCHAR(100),
    Song VARCHAR(100),
    Rating INT,
    FileSize BIGINT,
    [Order] SMALLINT
)
AS
BEGIN
    INSERT @returntable
    
    SELECT 
        v.PlaylistId,
        v.Playlist,
        v.Artist,
        v.Song,
        v.Rating,
        v.FileSize,
        COALESCE(v.[Order], 9999) [Order]
    FROM view_Playlist v
    WHERE (@playlistId IS NULL AND @playlistName IS NULL) OR (@playlistId = v.PlaylistId) OR (@playlistName LIKE v.Playlist)
    /* Não funfa dentro de Function
    ORDER BY 
        CASE WHEN v.[Order] is NULL THEN 9999 ELSE v.[Order] END ASC,
        v.Rating DESC
    */
    RETURN 
END
GO
