using Maxsys.MediaManager.MusicContext.Domain.Entities;
using Maxsys.MediaManager.MusicContext.Infra.DataExporter.SQLValuesScript.Scripts.Base;

namespace Maxsys.MediaManager.MusicContext.Infra.DataExporter.SQLValuesScript.Scripts
{
    public class MusicCatalogScripts : InsertScriptBase<Catalog>
    {
        public MusicCatalogScripts() : base("INSERT INTO [dbo].[Catalogs] ([Id], [Name]) VALUES") { }
        
        
        // (N'b29f66f9-35ff-3520-6f24-39f8ef4a0c4d', N'CAT 1')
        protected override string InsertValuesScript(Catalog obj)
            => $"({Value(obj.Id)}, {Value(obj.Name)})";
    }
}