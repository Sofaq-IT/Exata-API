using Exata.Domain.Entities;
using System.Data;

namespace Exata.Helpers.Interfaces
{
    public interface IBlobStorage
    {
        Task<string> UploadFileAsync(Stream fileStream, string fileName);
        Task<Stream> DownloadFileAsync(string fileName);
        Task<List<Dictionary<string, string>>> ReadExcelFileAsync(string fileName, Upload upload);
        Task<DataTable> ReadDataTableExcelFileAsync(string fileName);
    }
}
