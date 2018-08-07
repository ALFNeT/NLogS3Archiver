using System.IO;
using System.IO.Compression;
using NLog.Targets;
using Amazon.S3.Transfer;
using Amazon.S3;
using Amazon;
using System;
using System.Threading.Tasks;

namespace nlog_test
{
    /// <summary>
    /// Builtin IFileCompressor implementation utilizing the .Net4.5 specific <see cref="GZipStream"/> 
    /// and is used as an optional value for <see cref="FileTarget.FileCompressor"/> on .Net4.5.
    /// So log files created via <see cref="FileTarget"/> can be zipped when archived
    /// w/o 3rd party zip library when run on .Net4.5 or higher.
    /// </summary>
    internal class GZipArchiveFileCompressor : IFileCompressor
    {
        private const string bucketName = "testrino";
        private const string keyNamePrefix = "nlog/";
        private const string accessKey = "";
        private const string secretKey = "";

        // Specify your bucket region (an example region is shown).
        private static readonly RegionEndpoint bucketRegion = RegionEndpoint.APSoutheast2;
        private static IAmazonS3 s3Client;

        /// <summary>
        /// Implements <see cref="IFileCompressor.CompressFile(string, string)"/> using the .Net4.5 specific <see cref="ZipArchive"/>
        /// </summary>
        public void CompressFile(string fileName, string archiveFileName)
        {
            using (FileStream originalFileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (FileStream compressedFileStream = File.Create(archiveFileName))
                {
                    using (GZipStream compressionStream = new GZipStream(compressedFileStream,CompressionMode.Compress))
                    {
                        originalFileStream.CopyTo(compressionStream);
                    }
                }
            }
            UploadCompressedFile(archiveFileName);
        }

        private static async void UploadCompressedFile(string archiveFileName)
        {
            var keyName = Path.GetFileName(archiveFileName);
            try
            {
                var amazonS3Client = new AmazonS3Client(accessKey, secretKey, bucketRegion);
                var fileTransferUtility = new TransferUtility(amazonS3Client);

                var fileTransferUtilityRequest = new TransferUtilityUploadRequest
                {
                    BucketName = bucketName,
                    Key = keyNamePrefix + keyName,
                    FilePath = archiveFileName
                };
                await fileTransferUtility.UploadAsync(fileTransferUtilityRequest);
            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine("Error encountered on server. Message:'{0}' when writing an object", e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unknown encountered on server. Message:'{0}' when writing an object", e.Message);
            }
        }
    }
}