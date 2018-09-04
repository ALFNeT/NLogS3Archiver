using static System.Linq.Enumerable;


namespace nlog_test
{
    class Program
    {
        static char[] charSet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();
        static int byteSize = 256; //Labelling convenience
        static int biasZone = byteSize - (byteSize % charSet.Length);
        static string GenerateRandomString(int Length) //Configurable output string length
        {
            byte[] rBytes = new byte[Length];
            char[] rName = new char[Length];
            SecureFastRandom.GetBytes(rBytes);
            for (var i = 0; i < Length; i++)
            {
                rName[i] = charSet[rBytes[i] % charSet.Length];
            }
            return new string(rName);
        }
        static void Main(string[] args)
        {
           
            var logger = NLog.LogManager.GetCurrentClassLogger();
            // NLog.Targets.FileTarget.FileCompressor = new GZipArchiveFileCompressor();
            NLog.Targets.FileTarget.FileCompressor = new BZip2ArchiveFileCompressor();
            foreach (var index in Range(1, 10000))
            { 
                logger.Info(GenerateRandomString(2048));
            }
            System.Console.Read();
        }
    }
}
