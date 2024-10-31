using Azure.Storage.Blobs;
using ClosedXML.Excel;
using Exata.Domain.Entities;
using Exata.Helpers.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Exata.Helpers
{
    public class BlobStorage : IBlobStorage
    {
        private readonly BlobContainerClient _blobContainerClient;

        public BlobStorage(IConfiguration configuration)
        {
            var connectionString = configuration["AzureBlobStorage:ConnectionString"];
            var containerName = configuration["AzureBlobStorage:ContainerName"];

            var blobServiceClient = new BlobServiceClient(connectionString);
            _blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName);
        }

        public async Task<Stream> DownloadFileAsync(string fileName)
        {
            var blobClient = _blobContainerClient.GetBlobClient(fileName);
            var downloadResponse = await blobClient.DownloadAsync();
            return downloadResponse.Value.Content;
        }

        public async Task<List<Dictionary<string, string>>> ReadExcelFileAsync(string fileName, Upload upload)
        {
            var fileStream = await DownloadFileAsync(fileName);

            using (var workbook = new XLWorkbook(fileStream))
            {
                var worksheet = workbook.Worksheets.Worksheet(1); // Considera que os dados estão na primeira aba
                var rows = worksheet.RangeUsed().RowsUsed();

                var headers = new List<string>();
                var data = new List<Dictionary<string, string>>();

                //int linhaCabecalho = 0;
                
                //for (int linha = 0; linha < rows.Count(); linha++)
                //{
                //    foreach (var headerCell in rows.First().Cells())
                //    {
                //        if (headerCell.GetValue<string>() == "CLIENTE")
                //        {
                //            linhaCabecalho = linha;
                //            break;
                //        }                            
                //    }
                //}
                
                // Lê os cabeçalhos da primeira linha
                foreach (var headerCell in rows.First().Cells())
                {
                    headers.Add(headerCell.GetValue<string>());
                }

                //string[] colunasEsperadas = { "NOME", "CPF", "E-MAIL", "NOME TITULAR", "CPF/CNPJ TITULAR", "BANCO", "AGENCIA", "CONTA", "DIGITO CONTA", "CHAVE PIX", "VALOR" };

                //var containsAll = colunasEsperadas.All(item => headers.Contains(item));

                //if (!containsAll)
                //    throw new Exception("Arquivo não contem todas as colunas necessárias");

                // Lê os dados a partir da segunda linha
                foreach (var dataRow in rows.Skip(2))
                {
                    var rowData = new Dictionary<string, string>();
                    int columnIndex = 0;

                    foreach (var cell in dataRow.Cells())
                    {
                        rowData[headers[columnIndex]] = cell.GetValue<string>();

                        columnIndex++;
                    }

                    data.Add(rowData);
                }

                if (rows.Skip(1).Count() == 0)
                    throw new Exception("Arquivo não contém registros");

                upload.QtdeRegistros = rows.Skip(1).Count();

                return data;
            }
        }

        public async Task<string> UploadFileAsync(Stream fileStream, string fileName)
        {
            // Pega uma referência para o blob
            var blobClient = _blobContainerClient.GetBlobClient(fileName);

            // Faz upload do arquivo para o Blob Storage
            await blobClient.UploadAsync(fileStream);

            // Retorna a URL do arquivo
            return blobClient.Uri.ToString();
        }
    }
}
