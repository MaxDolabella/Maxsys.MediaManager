using Maxsys.MediaManager.MusicContext.Domain.Entities;
using Maxsys.MediaManager.MusicContext.Infra.DataExporter.SQLValuesScript.Scripts.Base;

namespace Maxsys.MediaManager.MusicContext.Infra.DataExporter.SQLValuesScript.Scripts
{
    public class MusicScripts : InsertScriptBase<Song>
    {
        public MusicScripts() : base("INSERT INTO [dbo].[Songs] ([Id], [MediaFile_FullPath], [MediaFile_OriginalFileName], [MediaFile_FileSize], [MediaFile_CreatedDate], [MediaFile_UpdatedDate], [Title], [TrackNumber], [Lyrics], [Comments], [Classification_Rating], [MusicDetails_IsBonusTrack], [MusicDetails_VocalGender], [MusicDetails_CoveredArtist], [MusicDetails_FeaturedArtist], [MusicProperties_Duration], [MusicProperties_BitRate], [AlbumId]) VALUES")
        {
        }

        //(N'c922bf3f-c22f-165b-7d03-39f8f45d5440', N'D:\TEMP\_DEV\TESTES_MM\Song\CAT 1\C1 Art1\Studio\(2000) C1A1 Alb1\01 [Katy Perry] Firework (80s Remix).mp3', N'01 [Katy Perry] Firework (80s Remix)', 4926969, CAST(N'2020-11-19T17:27:10.4045379' AS DateTime2), CAST(N'2020-11-19T17:27:10.4045379' AS DateTime2), N'[Katy Perry] Firework (80s Remix)', 1, NULL, NULL, 0, 0, 2, NULL, NULL, CAST(N'00:04:25.0383673' AS Time), 149, N'c10bcd7f-f45e-bd68-8bdc-39f8ef4a0d4c')
        protected override string InsertValuesScript(Song obj)
            => $"({Value(obj.Id)}, {Value(obj.FullPath)}, {Value(obj.OriginalFileName)}, {Value(obj.FileSize)}, {Value(obj.CreatedDate)}, {Value(obj.UpdatedDate)}, {Value(obj.Title)}, {Value(obj.TrackNumber)}, {Value(obj.Lyrics)}, {Value(obj.Comments)}, {Value(obj.Classification.Rating)}, {Value(obj.SongDetails.IsBonusTrack)}, {Value((byte)obj.SongDetails.VocalGender)}, {Value(obj.SongDetails.CoveredArtist)}, {Value(obj.SongDetails.FeaturedArtist)}, {Value(obj.SongProperties.Duration)}, {Value(obj.SongProperties.BitRate)}, {Value(obj.AlbumId)})";
    }
}