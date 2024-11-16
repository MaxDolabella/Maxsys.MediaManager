using Maxsys.MediaManager.MusicContext.Domain.Entities;
using Maxsys.MediaManager.MusicContext.Infra.DataExporter.SQLValuesScript.Scripts.Base;

namespace Maxsys.MediaManager.MusicContext.Infra.DataExporter.SQLValuesScript.Scripts
{
    public class PlaylistScripts : InsertScriptBase<Playlist>
    {
        public PlaylistScripts() : base("INSERT INTO [dbo].[Playlists] ([Id],[Name]) VALUES")
        {
        }

        protected override string InsertValuesScript(Playlist obj)
            => $"({Value(obj.Id)}, {Value(obj.Name)})";
    }
}