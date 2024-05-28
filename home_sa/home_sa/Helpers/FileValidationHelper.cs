using System.IO;
using System.Linq;


namespace home_sa.Helpers
{
    public static class FileValidationHelper
    {
        private static readonly byte[] DOCX_MAGIC_BYTES = { 0x50, 0x4B, 0x03, 0x04 };
        private static readonly byte[] PDF_MAGIC_BYTES = { 0x25, 0x50, 0x44, 0x46 };

        public static bool IsValidDocx(Stream fileStream)
        {
            return HasMagicBytes(fileStream, DOCX_MAGIC_BYTES);
        }

        public static bool IsValidPdf(Stream fileStream)
        {
            return HasMagicBytes(fileStream, PDF_MAGIC_BYTES);
        }

        private static bool HasMagicBytes(Stream fileStream, byte[] magicBytes)
        {
            byte[] buffer = new byte[magicBytes.Length];
            fileStream.Read(buffer, 0, magicBytes.Length);
            fileStream.Seek(0, SeekOrigin.Begin); // Reset the stream position no nothing decides to disapear
            return buffer.SequenceEqual(magicBytes);
        }
    }

}
