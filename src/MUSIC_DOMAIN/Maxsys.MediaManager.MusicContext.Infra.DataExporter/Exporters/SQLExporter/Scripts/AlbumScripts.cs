using Maxsys.MediaManager.MusicContext.Domain.Entities;
using Maxsys.MediaManager.MusicContext.Infra.DataExporter.SQLValuesScript.Scripts.Base;

namespace Maxsys.MediaManager.MusicContext.Infra.DataExporter.SQLValuesScript.Scripts
{
    public class AlbumScripts : InsertScriptBase<Album>
    {
        public AlbumScripts() : base("INSERT INTO [dbo].[Albums] ([Id], [AlbumDirectory], [Name], [Year], [Genre], [AlbumType], [AlbumCover], [ArtistId]) VALUES") { }

        // (N'c10bcd7f-f45e-bd68-8bdc-39f8ef4a0d4c', N'D:\TEMP\_DEV\TESTES_MM\Song\CAT 1\C1 Art1\Studio\(2000) C1A1 Alb1', N'C1A1 Alb1', 2000, N'Heavy Metal', 1, 0x, N'e2948c92-ebcc-47ee-587e-39f8ef4a0c4e')
        protected override string InsertValuesScript(Album obj)
        {
            return $"({Value(obj.Id)}, {Value(obj.AlbumDirectory)}, {Value(obj.Name)}, {Value(obj.Year)}, {Value(obj.Genre)}, {Value((byte)obj.AlbumType)}, {Value(obj.AlbumCover)}, {Value(obj.ArtistId)})";
        }
    }
}