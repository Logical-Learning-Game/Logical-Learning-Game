using System;
using System.Net.Sockets;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace Authentication
{
    public static class Utility
    {
        public static string RandomBase64URL(uint length)
        {
            var rng = new RNGCryptoServiceProvider();
            byte[] bytes = new byte[length];
            rng.GetBytes(bytes);

            return Base64URLEncoderNoPadding(bytes);
        }

        public static string Base64URLEncoderNoPadding(byte[] buffer)
        {
            string base64 = Convert.ToBase64String(buffer);

            // Converts base64 to base64url.
            base64 = base64.Replace("+", "-");
            base64 = base64.Replace("/", "_");
            // Strips padding.
            base64 = base64.Replace("=", "");

            return base64;
        }

        public static byte[] SHA256(string inputString)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(inputString);
            var sha256Managed = new SHA256Managed();

            return sha256Managed.ComputeHash(bytes);
        }

        public static int GetRandomUnusedPort()
        {
            var listener = new TcpListener(IPAddress.Loopback, 0);
            listener.Start();

            var port = ((IPEndPoint)listener.LocalEndpoint).Port;
            listener.Stop();

            return port;
        }

        public static string ReadJwtClaim(string token)
        {
            string[] parts = token.Split('.');
            if (parts.Length > 2)
            {
                string decode = parts[1];
                int padLength = 4 - decode.Length % 4;
                if (padLength < 4)
                {
                    decode += new string('=', padLength);
                }

                byte[] bytes = System.Convert.FromBase64String(decode);
                string userInfo = Encoding.ASCII.GetString(bytes);
                return userInfo;
            }
            return null;
        }
    }
    
}