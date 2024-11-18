using Azure.Storage.Blobs;
using ClosedXML.Excel;
using Exata.Domain.Entities;
using Exata.Helpers.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Data;

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

                // Lê os cabeçalhos da primeira linha
                foreach (var headerCell in rows.First().Cells())
                {
                    headers.Add(headerCell.GetValue<string>());
                }

                string[] colunasPrincipais = { "ID AMOSTRA LAB", "FAZENDA", "TALHÃO", "GLEBA", "PROFUNDIDADE", "PONTO DE COLETA" };
                string[] colunasElementos = { "pH H2O", "pH CaCl", "pH SMP", "P meh", "P rem", "P res", "P total", "Na", "K", "S", "Ca", "Mg", "Al", "H + Al", "MO", "CO", "B", "Cu", "Fe", "Mn", "Zn", "SB", "t", "T ", "V", "m", "Ca/Mg", "Ca/K", "Mg/K", "(Ca+Mg)/K", "Ca/t ", "Mg/t ", "Ca/T", "Mg/T", "K/T ", "Na/T", "(H+Al)/T", "(Ca+Mg)/T", "(Ca+Mg+K)/T", "(Ca+Mg+K+Na)/T", "Argila", "Silte", "Areia Total", "Areia Grossa", "Areia Fina" };

                var contemPrincipais = colunasPrincipais.All(item => headers.Contains(item));

                if (!contemPrincipais)
                    throw new Exception("Arquivo não contem todas as colunas obrigatórias. Não é possível seguir com a importação");

                List<string> validaElementos = headers.Except(colunasPrincipais).Except(colunasElementos).ToList();

                if (validaElementos.Count > 0)
                    throw new Exception($"Arquivo contém colunas não mapeadas. Não é possível seguir com a importação. (Colunas incorretas: {string.Join(", ", validaElementos)})");


                int linha = 3;

                string[] colunasObrigatorias = { "ID AMOSTRA LAB", "FAZENDA", "TALHÃO", "PROFUNDIDADE", "PONTO DE COLETA" };

                var rowData = new Dictionary<string, string>();

                // Lê os dados a partir da segunda linha
                foreach (var dataRow in rows.Skip(2))
                {
                    int columnIndex = 0;

                    foreach (var cell in dataRow.Cells())
                    {
                        string cellValue = cell.GetValue<string>();

                        rowData[headers[columnIndex]] = cellValue;

                        if (colunasObrigatorias.Contains(headers[columnIndex]) && string.IsNullOrEmpty(cellValue))
                            throw new Exception($"Arquivo contém informações obrigatórias não preenchidas. (Linha: {linha}, Coluna: {headers[columnIndex]})");

                        columnIndex++;
                    }

                    data.Add(rowData);

                    linha++;
                }

                if (rows.Skip(2).Count() == 0)
                    throw new Exception("Arquivo não contém registros");

                upload.QtdeRegistros = rows.Skip(2).Count();

                return data;
            }
        }

        public async Task<DataTable> ReadDataTableExcelFileAsync(string fileName)
        {
            var fileStream = await DownloadFileAsync(fileName);

            using (var workbook = new XLWorkbook(fileStream))
            {
                IXLWorksheet worksheet = workbook.Worksheets.First();

                // Converter para DataTable
                DataTable dataTable = new DataTable();
                bool isFirstRow = true;

                foreach (IXLRow row in worksheet.RowsUsed())
                {
                    if (isFirstRow)
                    {
                        // Adiciona as colunas
                        foreach (IXLCell cell in row.Cells())
                        {
                            dataTable.Columns.Add(cell.Value.ToString());
                        }
                        isFirstRow = false;
                    }
                    else
                    {
                        // Adiciona as linhas
                        DataRow dataRow = dataTable.NewRow();
                        int columnIndex = 0;

                        foreach (IXLCell cell in row.Cells(1, dataTable.Columns.Count))
                        {
                            dataRow[columnIndex] = cell.Value.ToString();
                            columnIndex++;
                        }

                        dataTable.Rows.Add(dataRow);
                    }
                }

                return dataTable;
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
