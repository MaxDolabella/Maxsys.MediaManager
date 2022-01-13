using FluentValidation.Results;
using System.Threading.Tasks;

namespace Maxsys.MediaManager.MusicContext.Infra.DataExporter
{
    public interface IDataExporter
    {
        Task<ValidationResult> ExportAsync(string exportFolder);
    }
}