using CommunityToolkit.Mvvm.ComponentModel;
using Maxsys.MediaManager.MusicContext.Domain.DTO;
using System;

namespace Maxsys.MediaManager.MusicContext.ApplicationMVVM.ViewModels.Catalog
{
    /// <summary>
    /// Observable wrapper to <see cref="CatalogDetailDTO"/>.
    /// </summary>
    public class CatalogDetailViewModel : ObservableObject
    {
        private readonly CatalogDetailDTO _catalog;

        public CatalogDetailViewModel(CatalogDetailDTO musicCatalog)
        {
            _catalog = musicCatalog;
        }

        public Guid CatalogId => _catalog.Id;
        public string CatalogName => _catalog.Name;
    }
}