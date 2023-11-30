using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace MLunarCoffee.Comon.Crypt
{
    public static class Encrypt
    {

        public static string AESEncryptString(string plainText ,string key)
        {
            byte[] iv = new byte[16];
            byte[] array;

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key ,aes.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream ,encryptor ,CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                        {
                            streamWriter.Write(plainText);
                        }

                        array = memoryStream.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(array);
        }

        public static string AESDecryptString(string cipherText ,string key)
        {
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(cipherText);

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key ,aes.IV);

                using (MemoryStream memoryStream = new MemoryStream(buffer))
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream ,decryptor ,CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }
        public static string EncryptString(string text ,string keyString)
        {
            byte[] clearBytes = Encoding.Unicode.GetBytes(text);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(keyString ,new byte[] { 0x49 ,0x76 ,0x61 ,0x6e ,0x20 ,0x4d ,0x65 ,0x64 ,0x76 ,0x65 ,0x64 ,0x65 ,0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms ,encryptor.CreateEncryptor() ,CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes ,0 ,clearBytes.Length);
                        cs.Close();
                    }
                    text = Convert.ToBase64String(ms.ToArray());
                }
            }
            return text;
        }

        public static string DecryptString(string cipherText ,string keyString)
        {
            try
            {
                cipherText = cipherText != null ? cipherText.Replace(" " ,"+") : "";
                byte[] cipherBytes = Convert.FromBase64String(cipherText);
                using (Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(keyString ,new byte[] { 0x49 ,0x76 ,0x61 ,0x6e ,0x20 ,0x4d ,0x65 ,0x64 ,0x76 ,0x65 ,0x64 ,0x65 ,0x76 });
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms ,encryptor.CreateDecryptor() ,CryptoStreamMode.Write))
                        {
                            cs.Write(cipherBytes ,0 ,cipherBytes.Length);
                            cs.Close();
                        }
                        cipherText = Encoding.Unicode.GetString(ms.ToArray());
                    }
                }
                return cipherText;
            }
            catch (Exception e)
            {
                return "";
            }
        }

        public static string encryptApiDocPara(string id ,string keycode)
        {
            try
            {
                string idEncrypt = Encrypt.EncryptString(id.ToString() ,keycode);
                int lengthConvertByte = 0;
                Regex rx = new Regex(@"[^\w=]");
                foreach (Match match in rx.Matches(idEncrypt))
                {
                    int index = match.Index + lengthConvertByte;
                    byte[] bytes = Encoding.ASCII.GetBytes(match.Value);
                    string convertByte = "Vt" + bytes[0] + "ch";
                    lengthConvertByte += convertByte.Length - 1;
                    idEncrypt = idEncrypt.Remove(index ,match.Value.Length).Insert(index ,convertByte);
                }
                return idEncrypt;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public static string decryptApiDocPara(string idEncrypt ,string keycode)
        {
            try
            {
                Regex rx = new Regex(@"(Vt\d+ch)");

                int lengthConvertByte = 0;
                foreach (Match match in rx.Matches(idEncrypt))
                {
                    int index = match.Index - lengthConvertByte;
                    int byteReverse = Convert.ToInt32(Regex.Replace(match.Value ,@"[A-Za-z]+" ,""));
                    byte[] bytes = new byte[1];
                    bytes[0] = Convert.ToByte(byteReverse);
                    lengthConvertByte += match.Value.Length - 1;
                    string strReverse = Encoding.ASCII.GetString(bytes);
                    idEncrypt = idEncrypt.Remove(index ,match.Value.Length).Insert(index ,strReverse);
                }

                string idDecrypt = Encrypt.DecryptString(idEncrypt ,keycode);
                return idDecrypt;
            }
            catch (Exception ex)
            {
                return "";
            }
        }
    }

}
