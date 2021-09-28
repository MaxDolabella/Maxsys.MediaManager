namespace Maxsys.MediaManager.MusicContext.Infra.DataExporter.Interfaces.SQLScripts
{
    interface IInsertScript<T>
    {
        /// <summary>
        /// Ex.: <code>INSERT [dbo].[MusicCatalogs] ([Id], [Name]) VALUES</code>
        /// </summary>
        string InsertScript { get; }


        /// <summary>
        /// Ex.: <code>(N'c1ee4b5c-cc4f-663d-58b2-39f0f7efb2be', N'ESTILOS')</code>
        /// </summary>
        string GetInsertValues(T obj);
    }
}
