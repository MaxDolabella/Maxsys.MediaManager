using System.Threading.Tasks;
using FluentValidation.Results;

namespace Maxsys.MediaManager.MusicContext.Infra.DataExporter
{
    public interface IDataExporter
    {
        Task<ValidationResult> ExportAsync(string exportFolder);
    }
}