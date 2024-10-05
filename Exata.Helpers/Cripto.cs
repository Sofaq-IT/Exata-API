using System.Security.Cryptography;
using System.Text;
using Exata.Helpers.Interfaces;

namespace Exata.Helpers;

public class Cripto : ICripto
{
    //Chave convertida para arreay de bytes. A chave deve ter 16 bytes (128 bit)
    //ou 32 bytes (256 bit). No exemplo usamos 16 bytes (128 bit)
    private readonly byte[] chave;
    //Vetor de inicializaçao. Deve ter exatamente 16 bytes.
    private readonly byte[] Vi;

    public Cripto(string Chave = "VaLoRPaDRao1WaPP", string vi = "vAlOrpAdrAO2wApp")
    {
        chave = ASCIIEncoding.ASCII.GetBytes(Chave);
        Vi = ASCIIEncoding.ASCII.GetBytes(vi);
    }

    public string Criptografar(string strValor)
    {
        ValidaChaves();
        byte[] encrypted = encryptStringToBytes_AES(strValor, chave, Vi);
        return Convert.ToBase64String(encrypted);
    }
    private byte[] encryptStringToBytes_AES(string plainText, byte[] Key, byte[] IV)
    {
        if (plainText == null || plainText.Length <= 0)
            throw new ArgumentNullException("plainText");
        if (Key == null || Key.Length <= 0)
            throw new ArgumentNullException("Key");
        if (IV == null || IV.Length <= 0)
            throw new ArgumentNullException("IV");
        byte[] encrypted;

        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = Key;
            aesAlg.IV = IV;

            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(plainText);
                    }
                    encrypted = msEncrypt.ToArray();
                }
            }
        }

        return encrypted;
    }

    public string Descriptografar(string strValor)
    {
        ValidaChaves();
        return decryptStringFromBytes_AES(Convert.FromBase64String(strValor), chave, Vi);
    }

    private string decryptStringFromBytes_AES(byte[] cipherText, byte[] Key, byte[] IV)
    {
        if (cipherText == null || cipherText.Length <= 0)
            throw new ArgumentNullException("cipherText");
        if (Key == null || Key.Length <= 0)
            throw new ArgumentNullException("Key");
        if (IV == null || IV.Length <= 0)
            throw new ArgumentNullException("IV");

        string plaintext;

        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = Key;
            aesAlg.IV = IV;

            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            using (MemoryStream msDecrypt = new MemoryStream(cipherText))
            {
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {
                        plaintext = srDecrypt.ReadToEnd();
                    }
                }
            }
        }

        return plaintext;
    }

    private bool ValidaChaves()
    {
        if (chave.Length != 16)
        {
            throw new ArgumentException("Chave de Criptografia Informada Errada!");
        }

        if (Vi.Length != 16)
        {
            throw new ArgumentException("Vetor de Criptografia Informada Errada!");
        }

        return true;
    }
}