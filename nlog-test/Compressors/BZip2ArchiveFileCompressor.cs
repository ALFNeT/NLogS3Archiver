using System;
using System.IO;
using System.IO.Compression;
using ICSharpCode.SharpZipLib.BZip2;
using NLog.Targets;

namespace nlog_test
{
    /// <summary>
    /// Builtin IFileCompressor implementation utilizing the .Net4.5 specific <see cref="GZipStream"/> 
    /// and is used as an optional value for <see cref="FileTarget.FileCompressor"/> on .Net4.5.
    /// So log files created via <see cref="FileTarget"/> can be zipped when archived
    /// w/o 3rd party zip library when run on .Net4.5 or higher.
    /// </summary>
    class BZip2ArchiveFileCompressor : IFileCompressor
    {
        /// <summary>
        /// Implements <see cref="IFileCompressor.CompressFile(string, string)"/> using the .Net4.5 specific <see cref="ZipArchive"/>
        /// </summary>
        public void CompressFile(string fileName, string archiveFileName)
        {
            using (FileStream fileToBeZippedAsStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (FileStream compressedFileStream = File.Create(archiveFileName))
                {
                    try
                    {
                        BZip2.Compress(fileToBeZippedAsStream, compressedFileStream, true, 5);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            S3Uploader.UploadCompressedFile(archiveFileName);
        }
    }
}
