using Maxsys.MediaManager.MusicContext.Domain.Entities;
using Maxsys.MediaManager.MusicContext.Infra.DataExporter.SQLValuesScript.Scripts.Base;

namespace Maxsys.MediaManager.MusicContext.Infra.DataExporter.SQLValuesScript.Scripts
{
    // ComposerMusic {ComposersId, MusicsId}
    public class ComposerMusicScripts : InsertScriptBase<(Composer composer, Music music)>
    {
        public ComposerMusicScripts() : base("INSERT INTO [dbo].[ComposerMusic] ([ComposersId],[MusicsId]) VALUES") { }
        
        protected override string InsertValuesScript((Composer composer, Music music) obj)
            => $"({Value(obj.composer.Id)}, {Value(obj.music.Id)})";
    }
}