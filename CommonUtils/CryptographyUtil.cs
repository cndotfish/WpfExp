using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace CommonUtils
{
    public class CryptographyUtil
    {
        /// <summary>
        /// 使用HASH加密，不能解密
        /// </summary>
        /// <param name="hashAlgorithm"></param>
        /// <param name="dataToHash"></param>
        /// <param name="head">头部信息</param>
        /// <param name="tail">尾部信息</param>
        /// <returns></returns>
        public static string Encrypt(HashAlgorithm hashAlgorithm, string dataToHash,string head = null,string tail = null)
        {
            if (!string.IsNullOrEmpty(head))
                dataToHash += head;
            if (!string.IsNullOrEmpty(tail))
                dataToHash += tail;
            UTF8Encoding UTF8 = new UTF8Encoding();
            byte[] data = UTF8.GetBytes(dataToHash);
            byte[] result = hashAlgorithm.ComputeHash(data);
            StringBuilder hexResult = new StringBuilder(result.Length);

            for (int i = 0; i < result.Length; i++)
            {
                //// Convert to hexadecimal
                hexResult.Append(result[i].ToString("X2"));
            }
            return hexResult.ToString();
        }

        public static bool IsHashMatch(HashAlgorithm hashAlgorithm, string hashedText, string unhashedText)
        {
            string hashedTextToCompare = Encrypt(hashAlgorithm, unhashedText);
            return (String.Compare(hashedText, hashedTextToCompare, false) == 0);
        }

        public static string Encrypt(SymmetricAlgorithm algorithm, string plainText, byte[] key,byte[] iv,CipherMode cipherMode, PaddingMode paddingMode)
        {
            byte[] plainBytes;
            byte[] cipherBytes;
            algorithm.Key = key;
            algorithm.Mode = cipherMode;
            algorithm.Padding = paddingMode;
            algorithm.IV = iv;

            BinaryFormatter bf = new BinaryFormatter();

            using (MemoryStream stream = new MemoryStream())
            {
                bf.Serialize(stream, plainText);
                plainBytes = stream.ToArray();
            }

            using (MemoryStream ms = new MemoryStream())
            {
                // Defines a stream for cryptographic transformations
                CryptoStream cs = new CryptoStream(ms, algorithm.CreateEncryptor(), CryptoStreamMode.Write);

                // Writes a sequence of bytes for encrption
                cs.Write(plainBytes, 0, plainBytes.Length);

                // Closes the current stream and releases any resources 
                cs.Close();
                // Save the ciphered message into one byte array
                cipherBytes = ms.ToArray();
                // Closes the memorystream object
                ms.Close();
            }
            string base64Text = Convert.ToBase64String(cipherBytes);

            return base64Text;
        }

        public static string Decrypt(SymmetricAlgorithm algorithm, string base64Text, byte[] key,byte[] iv,CipherMode cipherMode, PaddingMode paddingMode)
        {
            byte[] plainBytes;
            //// Convert the base64 string to byte array. 
            byte[] cipherBytes = Convert.FromBase64String(base64Text);
            algorithm.Key = key;
            algorithm.Mode = cipherMode;
            algorithm.Padding = paddingMode;
            algorithm.IV = iv;

            using (MemoryStream memoryStream = new MemoryStream(cipherBytes))
            {
                using (CryptoStream cs = new CryptoStream(memoryStream,
                    algorithm.CreateDecryptor(), CryptoStreamMode.Read))
                {
                    plainBytes = new byte[cipherBytes.Length];
                    cs.Read(plainBytes, 0, cipherBytes.Length);
                }
            }

            string recoveredMessage;
            using (MemoryStream stream = new MemoryStream(plainBytes, false))
            {
                BinaryFormatter bf = new BinaryFormatter();
                recoveredMessage = bf.Deserialize(stream).ToString();
            }

            return recoveredMessage;

        }

        public static void GenerateRSAKey(RSACryptoServiceProvider algorithm)
        {
            string RSAPrivateKey = algorithm.ToXmlString(true);

            using (StreamWriter streamWriter = new StreamWriter("PublicPrivateKey.xml"))
            {
                streamWriter.Write(RSAPrivateKey);
            }

            string RSAPubicKey = algorithm.ToXmlString(false);
            using (StreamWriter streamWriter = new StreamWriter("PublicOnlyKey.xml"))
            {
                streamWriter.Write(RSAPubicKey);
            }
        }

        public static string Encrypt(RSACryptoServiceProvider algorithm, string plainText)
        {
            string publicKey;
            List<Byte[]> cipherArray = new List<byte[]>();

            //// read the public key.
            using (StreamReader streamReader = new StreamReader("PublicOnlyKey.xml"))
            {
                publicKey = streamReader.ReadToEnd();
            }
            algorithm.FromXmlString(publicKey);

            BinaryFormatter binaryFormatter = new BinaryFormatter();
            byte[] plainBytes = null;


            //// Use BinaryFormatter to serialize plain text.
            using (MemoryStream memoryStream = new MemoryStream())
            {
                binaryFormatter.Serialize(memoryStream, plainText);
                plainBytes = memoryStream.ToArray();
            }

            int totLength = 0;
            int index = 0;

            //// Encrypt plain text by public key.
            if (plainBytes.Length > 80)
            {
                byte[] partPlainBytes;
                byte[] cipherBytes;
                while (plainBytes.Length - index > 0)
                {
                    partPlainBytes = plainBytes.Length - index > 80 ? new byte[80] : new byte[plainBytes.Length - index];

                    for (int i = 0; i < 80 && (i + index) < plainBytes.Length; i++)
                        partPlainBytes[i] = plainBytes[i + index];

                    cipherBytes = algorithm.Encrypt(partPlainBytes, false);
                    totLength += cipherBytes.Length;
                    index += 80;

                    cipherArray.Add(cipherBytes);
                }
            }

            //// Convert to byte array.
            byte[] cipheredPlaintText = new byte[totLength];
            index = 0;
            foreach (byte[] item in cipherArray)
            {
                for (int i = 0; i < item.Length; i++)
                {
                    cipheredPlaintText[i + index] = item[i];
                }

                index += item.Length;
            }
            return Convert.ToBase64String(cipheredPlaintText);

        }

        /// <summary>
        /// Decrypts the specified algorithm.
        /// </summary>
        /// <param name="algorithm">The algorithm.</param>
        /// <param name="base64Text">The base64 text.</param>
        /// <returns></returns>
        public static string Decrypt(RSACryptoServiceProvider algorithm, string base64Text)
        {
            byte[] cipherBytes = Convert.FromBase64String(base64Text);
            List<byte[]> plainArray = new List<byte[]>();
            string privateKey = string.Empty;

            //// Read the private key.
            using (StreamReader streamReader = new StreamReader("PublicPrivateKey.xml"))
            {
                privateKey = streamReader.ReadToEnd();
            }

            algorithm.FromXmlString(privateKey);

            int index = 0;
            int totLength = 0;
            byte[] partPlainText = null;
            byte[] plainBytes;
            int length = cipherBytes.Length / 2;
            //int j = 0;
            //// Decrypt the ciphered text through private key.
            while (cipherBytes.Length - index > 0)
            {
                partPlainText = cipherBytes.Length - index > 128 ? new byte[128] : new byte[cipherBytes.Length - index];

                for (int i = 0; i < 128 && (i + index) < cipherBytes.Length; i++)
                    partPlainText[i] = cipherBytes[i + index];

                plainBytes = algorithm.Decrypt(partPlainText, false);

                totLength += plainBytes.Length;
                index += 128;
                plainArray.Add(plainBytes);

            }

            byte[] recoveredPlaintext = new byte[length];
            index = 0;
            for (int i = 0; i < plainArray.Count; i++)
            {
                for (int j = 0; j < plainArray[i].Length; j++)
                {
                    recoveredPlaintext[index + j] = plainArray[i][j];
                }
                index += plainArray[i].Length;
            }

            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream stream = new MemoryStream())
            {
                stream.Write(recoveredPlaintext, 0, recoveredPlaintext.Length);
                stream.Position = 0;
                string msgobj = (string)bf.Deserialize(stream);
                return msgobj;
            }

        }
    }
}
