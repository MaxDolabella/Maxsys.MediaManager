using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using Maxsys.MediaManager.MusicContext.ApplicationMVVM.ViewModels.Catalog;

namespace Maxsys.MediaManager.MusicContext.ApplicationMVVM.Interfaces.ViewModels;

public interface ICatalogCreateViewModel : IValidableViewModel
{
    //TODO se mudar para IReadOnlyList?
    // ReadOnlyObservableCollection<CatalogDetailViewModel> Catalogs { get; }

    string CatalogName { get; }

    IReadOnlyList<CatalogDetailViewModel> Catalogs { get; }

    IAsyncRelayCommand SaveCommand { get; }

    Task LoadCatalogsAsync(CancellationToken cancellation = default);
}