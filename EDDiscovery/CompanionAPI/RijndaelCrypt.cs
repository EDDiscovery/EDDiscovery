using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EDDiscovery.CompanionAPI
{

    public class RijndaelCrypt : IDisposable
    {
        Rijndael rijndael;
        UTF8Encoding encoding;



        public RijndaelCrypt()
        {
            string vectorstr = Environment.MachineName + "sfdgty45dv45hdssde55dsdf"; // Generate machine dependant IV.
            encoding = new UTF8Encoding();

            byte[] key = { 237, 132,  42, 190,   2,  63, 222,  20, 186, 242,  36, 180, 185,  59, 175,   9,
                            120, 164, 201,  66, 189,  73, 104, 135, 190,   4, 107, 134, 146,  10, 228, 228 };

            var bytes = encoding.GetBytes(vectorstr);

            byte[] vector = new byte[16];
            for (int ii = 0; ii < 16; ii++)
                vector[ii] = bytes[ii];

            rijndael = Rijndael.Create();
            rijndael.Key = key;
            rijndael.IV = vector;
        }

        public string Encrypt(string valueToEncrypt)
        {
            var bytes = encoding.GetBytes(valueToEncrypt);
            using (var encryptor = rijndael.CreateEncryptor())
            using (var stream = new MemoryStream())
            using (var crypto = new CryptoStream(stream, encryptor, CryptoStreamMode.Write))
            {
                crypto.Write(bytes, 0, bytes.Length);
                crypto.FlushFinalBlock();
                stream.Position = 0;
                var encrypted = new byte[stream.Length];
                stream.Read(encrypted, 0, encrypted.Length);
                return Convert.ToBase64String(encrypted);
            }
        }

        public string Decrypt(string encryptedValue)
        {
            using (var decryptor = rijndael.CreateDecryptor())
            using (var stream = new MemoryStream())
            using (var crypto = new CryptoStream(stream, decryptor, CryptoStreamMode.Write))
            {
                var encrypted = Convert.FromBase64String(encryptedValue);
                crypto.Write(encrypted, 0, encrypted.Length);
                crypto.FlushFinalBlock();
                stream.Position = 0;
                var decryptedBytes = new Byte[stream.Length];
                stream.Read(decryptedBytes, 0, decryptedBytes.Length);
                return encoding.GetString(decryptedBytes);
            }
        }

        public void Dispose()
        {
            if (rijndael != null)
            {
                rijndael.Dispose();
            }
        }
    }
}
