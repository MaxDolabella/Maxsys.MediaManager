using Maxsys.MediaManager.MusicContext.Domain.Entities;
using Maxsys.MediaManager.MusicContext.Infra.DataExporter.SQLValuesScript.Scripts.Base;

namespace Maxsys.MediaManager.MusicContext.Infra.DataExporter.SQLValuesScript.Scripts
{
    public class ArtistScripts : InsertScriptBase<Artist>
    {
        public ArtistScripts() : base("INSERT INTO [dbo].[Artists] ([Id], [Name], [CatalogId]) VALUES") { }

        // (N'e2948c92-ebcc-47ee-587e-39f8ef4a0c4e', N'C1 Art1', N'b29f66f9-35ff-3520-6f24-39f8ef4a0c4d')
        protected override string InsertValuesScript(Artist obj)
            => $"({Value(obj.Id)}, {Value(obj.Name)}, {Value(obj.CatalogId)})";
    }
}