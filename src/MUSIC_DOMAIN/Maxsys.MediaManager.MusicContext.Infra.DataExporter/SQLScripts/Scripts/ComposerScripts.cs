using Maxsys.MediaManager.MusicContext.Domain.Entities;
using Maxsys.MediaManager.MusicContext.Infra.DataExporter.SQLValuesScript.Scripts.Base;

namespace Maxsys.MediaManager.MusicContext.Infra.DataExporter.SQLValuesScript.Scripts
{
    public class ComposerScripts : InsertScriptBase<Composer>
    {
        public ComposerScripts() : base("INSERT INTO [dbo].[Composers] ([Id], [Name]) VALUES") { }


        // (N'b29f66f9-35ff-3520-6f24-39f8ef4a0c4d', N'Composer name')
        protected override string InsertValuesScript(Composer obj)
            => $"({Value(obj.Id)}, {Value(obj.Name)})";
    }
}