using Maxsys.MediaManager.MusicContext.Domain.Entities;
using Maxsys.MediaManager.MusicContext.Infra.DataExporter.SQLValuesScript.Scripts.Base;

namespace Maxsys.MediaManager.MusicContext.Infra.DataExporter.SQLValuesScript.Scripts
{
    public class PlaylistItemScripts : InsertScriptBase<PlaylistItem>
    {
        public PlaylistItemScripts() : base("INSERT INTO [dbo].[PlaylistItems] ([PlaylistId],[Id],[Order]) VALUES") { }

        protected override string InsertValuesScript(PlaylistItem obj)
            => $"({Value(obj.PlaylistId)}, {Value(obj.SongId)}, {Value(obj.Order)})";
    }
}