using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;
using NLog.Targets;

namespace nlog_test
{
    /// <summary>
    /// Builtin IFileCompressor implementation utilizing the .Net4.5 specific <see cref="ZipArchive"/> 
    /// and is used as the default value for <see cref="FileTarget.FileCompressor"/> on .Net4.5.
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
            //using (var archiveStream = new FileStream(archiveFileName, FileMode.Create))
            //using (var archive = new ZipArchive(archiveStream, ZipArchiveMode.Create, ))
            //using (var originalFileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite ))
            //{
            //    var zipArchiveEntry = archive.CreateEntry(Path.GetFileName(fileName));
            //    using (var destination = zipArchiveEntry.Open())
            //    {
            //        originalFileStream.CopyTo(destination);
            //    }
            //}
            using (FileStream originalFileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (FileStream compressedFileStream = File.Create(Path.ChangeExtension(archiveFileName, ".gz")))
                {
                    using (GZipStream compressionStream = new GZipStream(compressedFileStream,CompressionMode.Compress))
                    {
                        originalFileStream.CopyTo(compressionStream);

                    }
                }
            }
        }
    }
}