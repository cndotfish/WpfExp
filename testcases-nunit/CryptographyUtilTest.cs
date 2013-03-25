using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonUtils
{
    using NUnit.Framework;
    using System.Security.Cryptography;

    [TestFixture]
    public class CryptographyUtilTest
    {
        [Test]
        public void testHashEncrypt()
        {
            string dataToHash = "62586235214578";
            HashAlgorithm hashAlg = MD5.Create();
            string strEncrypt = CryptographyUtil.Encrypt(hashAlg, dataToHash);
            bool isMatch = CryptographyUtil.IsHashMatch(hashAlg, strEncrypt, dataToHash);

            Assert.AreEqual(isMatch, true);
        }

        [Test]
        public void testSymetricEncrypt()
        {
            string dataToHash = "12322364654564";
            SymmetricAlgorithm rijd = RijndaelManaged.Create();
            //必须32字节
            byte[] key = Encoding.ASCII.GetBytes("1234567890asdfghjklzxcvbnmqwerty"); 
            //byte[] key2 = Encoding.ASCII.GetBytes("1234567890asdfghjklzxcvbnmqwer7y");
            //必须16字节
            byte[] iv = Encoding.ASCII.GetBytes("1xxxxxxxxx7xxx2x");
            byte[] iv2 = Encoding.ASCII.GetBytes("7777777777777777");
            string strEncrypted = CryptographyUtil.Encrypt(rijd, dataToHash, key, iv, CipherMode.ECB, PaddingMode.ANSIX923);
            string strDecrypted = CryptographyUtil.Decrypt(rijd, strEncrypted, key, iv2, CipherMode.ECB, PaddingMode.Zeros);
            Assert.AreEqual(dataToHash, strDecrypted);
        }

        [Test]
        public void testBytesToBit()
        {
            string str = "1234567890asdfghjklzxcvbnmqwerty";
            Assert.AreEqual(32,Encoding.ASCII.GetBytes(str).Length);
        }

        [Test]
        public void testAsymetricEncrypt()
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            CryptographyUtil.GenerateRSAKey(rsa);

            string dataToHash = "1234123121";
            string strEncryped = CryptographyUtil.Encrypt(rsa, dataToHash);
            string strDecryped = CryptographyUtil.Decrypt(rsa, strEncryped);
            Assert.AreEqual(dataToHash, strDecryped);
        }
    }
}
