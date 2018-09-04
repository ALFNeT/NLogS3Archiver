using System.IO;
using System.IO.Compression;
using NLog.Targets;

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
            S3Uploader.UploadCompressedFile(archiveFileName);
        }
    }
}