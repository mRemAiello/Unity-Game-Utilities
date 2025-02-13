using System;
using System.IO;
using System.Text;

namespace GameUtils
{
    public class Compression
    {
        public static string Compress(string value, CompressionMode compressionMode)
        {
            if (compressionMode == CompressionMode.None)
            {
                return value;
            }

            var bytes = Encoding.UTF8.GetBytes(value);

            using (var streamIn = new MemoryStream(bytes))
            using (var streamOut = new MemoryStream())
            {
                using (var compressionStream = new Ionic.Zlib.GZipStream(streamOut, Ionic.Zlib.CompressionMode.Compress))
                {
                    streamIn.CopyTo(compressionStream);
                }

                return Convert.ToBase64String(streamOut.ToArray());
            }
        }

        public static string Decompress(string value, CompressionMode compressionMode)
        {
            if (compressionMode == CompressionMode.None)
            {
                return value;
            }

            var bytes = Convert.FromBase64String(value);

            using (var streamIn = new MemoryStream(bytes))
            using (var streamOut = new MemoryStream())
            {
                using (var compressionStream = new Ionic.Zlib.GZipStream(streamIn, Ionic.Zlib.CompressionMode.Decompress))
                {
                    compressionStream.CopyTo(streamOut);
                }

                return Encoding.UTF8.GetString(streamOut.ToArray());
            }
        }
    }
}