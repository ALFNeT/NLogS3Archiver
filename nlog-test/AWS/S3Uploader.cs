using Amazon.S3.Transfer;
using Amazon.S3;
using Amazon;
using System.IO;
using System;
using Amazon.Runtime.CredentialManagement;
using Amazon.Runtime;

namespace nlog_test
{
    public class S3Uploader
    {
        private const string bucketName = "testrino";
        private const string keyNamePrefix = "nlog/";
        // Specify your bucket region (an example region is shown).
        private static readonly RegionEndpoint bucketRegion = RegionEndpoint.APSoutheast2;

        public static async void UploadCompressedFile(string archiveFileName)
        {
            var keyName = Path.GetFileName(archiveFileName);
            try
            {
                var chain = new CredentialProfileStoreChain();
                AWSCredentials awsCredentials;
                if (chain.TryGetAWSCredentials("playpen", out awsCredentials))
                {
                    var amazonS3Client = new AmazonS3Client(awsCredentials, bucketRegion);
                    var fileTransferUtility = new TransferUtility(amazonS3Client);
                    var fileTransferUtilityRequest = new TransferUtilityUploadRequest
                    {
                        BucketName = bucketName,
                        Key = keyNamePrefix + keyName,
                        FilePath = archiveFileName
                    };
                    await fileTransferUtility.UploadAsync(fileTransferUtilityRequest);
                }
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