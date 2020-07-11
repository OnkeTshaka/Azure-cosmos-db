using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace FirewallsAzureProject.Models
{
    public class BlobOperations
    {
        private static CloudBlobContainer profileBlobContainer;

        public BlobOperations()
        {
            var storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["webjobstorage"].ToString());

            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Get the blob container reference.
            profileBlobContainer = blobClient.GetContainerReference("profiles");
            //Create Blob Container if not exist
            profileBlobContainer.CreateIfNotExists();
        }

        public async Task<CloudBlockBlob> UploadBlob(HttpPostedFileBase profileFile)
        {
            string blobName = Guid.NewGuid().ToString() + Path.GetExtension(profileFile.FileName);
            // GET a blob reference. 
            CloudBlockBlob profileBlob = profileBlobContainer.GetBlockBlobReference(blobName);
            // Uploading a local file and Create the blob.
            using (var fs = profileFile.InputStream)
            {
                await profileBlob.UploadFromStreamAsync(fs);
            }
            return profileBlob;
        }

    }
}