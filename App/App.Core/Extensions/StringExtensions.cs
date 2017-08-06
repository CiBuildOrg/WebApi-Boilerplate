using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace App.Core.Extensions
{
    public static class StringExtensions
    {
        public static byte[] ToByteArray(this string str)
        {
            var strBytes = Encoding.UTF8.GetBytes(str);
            return strBytes;
        }

        /// <summary>
        /// 해쉬 문자열 반환
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string GetHash(this string input)
        {
            HashAlgorithm hashAlgorithm = new SHA256CryptoServiceProvider();
            var byteValue = input.ToByteArray();
            var byteHash = hashAlgorithm.ComputeHash(byteValue);

            return Convert.ToBase64String(byteHash);
        }

        public static string GetHash(this string input, HashType hashType)
        {
            var inputBytes = input.ToByteArray();

            switch (hashType)
            {
                case HashType.Hmac: return Convert.ToBase64String(HMAC.Create().ComputeHash(inputBytes));
                case HashType.Hmacmd5: return Convert.ToBase64String(HMAC.Create().ComputeHash(inputBytes));
                case HashType.Hmacsha1: return Convert.ToBase64String(HMAC.Create().ComputeHash(inputBytes));
                case HashType.Hmacsha256: return Convert.ToBase64String(HMAC.Create().ComputeHash(inputBytes));
                case HashType.Hmacsha384: return Convert.ToBase64String(HMAC.Create().ComputeHash(inputBytes));
                case HashType.Hmacsha512: return Convert.ToBase64String(HMAC.Create().ComputeHash(inputBytes));
                case HashType.MacTripleDes: return Convert.ToBase64String(KeyedHashAlgorithm.Create().ComputeHash(inputBytes));
                case HashType.Md5: return Convert.ToBase64String(MD5.Create().ComputeHash(inputBytes));
                case HashType.Ripemd160: return Convert.ToBase64String(RIPEMD160.Create().ComputeHash(inputBytes));
                case HashType.Sha1: return Convert.ToBase64String(SHA1.Create().ComputeHash(inputBytes));
                case HashType.Sha256: return Convert.ToBase64String(SHA256.Create().ComputeHash(inputBytes));
                case HashType.Sha384: return Convert.ToBase64String(SHA384.Create().ComputeHash(inputBytes));
                case HashType.Sha512: return Convert.ToBase64String(SHA512.Create().ComputeHash(inputBytes));
                default: return Convert.ToBase64String(inputBytes);
            }
        }

        public static string Encrypt(this string stringToEncrypt, string key)
        {
            if (string.IsNullOrEmpty(stringToEncrypt)) throw new ArgumentException("stringToEncrypt");

            if (string.IsNullOrEmpty(key)) throw new ArgumentException("key");

            var cspp = new CspParameters {KeyContainerName = key};

            var rsa = new RSACryptoServiceProvider(cspp) {PersistKeyInCsp = true};

            var bytes = rsa.Encrypt(Encoding.UTF8.GetBytes(stringToEncrypt), true);

            return BitConverter.ToString(bytes);
        }

        public static string Decrypt(this string stringToDecrypt, string key)
        {
            if (string.IsNullOrEmpty(stringToDecrypt)) throw new ArgumentException("stringToDecrypt");

            if (string.IsNullOrEmpty(key)) throw new ArgumentException("key");

            var cspp = new CspParameters {KeyContainerName = key};

            var rsa = new RSACryptoServiceProvider(cspp) {PersistKeyInCsp = true};

            var decryptArray = stringToDecrypt.Split(new[] { "-" }, StringSplitOptions.None);
            var decryptByteArray = Array.ConvertAll(decryptArray, (s => Convert.ToByte(byte.Parse(s, NumberStyles.HexNumber))));

            var bytes = rsa.Decrypt(decryptByteArray, true);

            var result = Encoding.UTF8.GetString(bytes);

            return result;
        }
       
    }
}