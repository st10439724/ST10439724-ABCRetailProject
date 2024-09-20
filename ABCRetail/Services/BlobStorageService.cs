using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System.IO;
using System.Threading.Tasks;





namespace ABCRetail.Services
{
    public class BlobStorageService
    {
        private readonly BlobServiceClient _blobServiceClient;

        public BlobStorageService(string connectionString)
        {
            _blobServiceClient = new BlobServiceClient(connectionString);
        }

        //public async Task UploadBlobAsync(string containerName, string blobName, Stream stream)
        //{
        //    var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        //    await containerClient.CreateIfNotExistsAsync();
        //    var blobClient = containerClient.GetBlobClient(blobName);
        //    await blobClient.UploadAsync(stream, new BlobHttpHeaders { ContentType = "image/jpeg" }); // not sure about content type 
        //}

        public async Task UploadBlobAsync(string containerName, string blobName, Stream stream, string contentType)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            await containerClient.CreateIfNotExistsAsync();
            var blobClient = containerClient.GetBlobClient(blobName);
            await blobClient.UploadAsync(stream, new BlobHttpHeaders { ContentType = contentType });
        }

        public async Task<Stream> DownloadBlobAsync(string containerName, string blobName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(blobName);
            var response = await blobClient.DownloadAsync();
            return response.Value.Content;
        }

        public string GetBlobUri(string containerName, string blobName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(blobName);
            return blobClient.Uri.ToString();
        }
    }
}



















































//namespace ABCRetail.Services
//{
//    public class BlobStorageService

//    {
//        private readonly BlobServiceClient _blobServiceClient;

//        public BlobStorageService(string connectionString)
//        {
//            _blobServiceClient = new BlobServiceClient(connectionString);
//        }

//        public async Task UploadBlobAsync(string containerName, string blobName, Stream stream)
//        {
//            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
//            await containerClient.CreateIfNotExistsAsync();
//            var blobClient = containerClient.GetBlobClient(blobName);
//            await blobClient.UploadAsync(stream, new BlobHttpHeaders { ContentType = "application/octet-stream" });
//        }

//        public async Task<Stream> DownloadBlobAsync(string containerName, string blobName)
//        {
//            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
//            var blobClient = containerClient.GetBlobClient(blobName);
//            var response = await blobClient.DownloadAsync();
//            return response.Value.Content;
//        }
//    }
//}


