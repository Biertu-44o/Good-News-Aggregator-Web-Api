using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;

namespace Services
{
    internal class PictureBase64EncoderDecoder
    {
        internal async Task<String?> PictureEncoder(String Path)
        {
            try
            {
                return Convert.ToBase64String(
                    await File.ReadAllBytesAsync(@Path));
            }
            catch
            {
                return null;
            }
        }
        internal Byte[]? PictureDecoder(String byteString)
        {
            try
            {
                return Convert.FromBase64String(byteString);
            }
            catch
            {
                return null;
            }
        }
    }
}
