using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.IO;

public class Utils
{
    public static int Add(int x, int y)
    {
        return x + y;
    }

    public static byte[] Encrypt(string content)
    {
        using var aes = Aes.Create();
		
		// Create an encryptor to perform the stream transform.
		ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

        // Create the streams used for encryption.
        using MemoryStream msEncrypt = new();
        using CryptoStream csEncrypt = new(msEncrypt, encryptor, CryptoStreamMode.Write);
        using StreamWriter swEncrypt = new(csEncrypt);

        //Write all data to the stream.
        swEncrypt.Write(content);

        return msEncrypt.ToArray().Concat(aes.Key).Concat(aes.IV).ToArray();
    }

    public static string Decrypt(byte[] payload)
    {
        using Aes aes = Aes.Create();
        var keyLength = aes.Key.Length;
        var ivLength = aes.IV.Length;

        var key = payload[(payload.Length - (keyLength + ivLength)) .. (payload.Length - ivLength)].ToArray();
		var iv = payload[(payload.Length - ivLength) .. payload.Length].ToArray();
		var content = payload[0 .. (payload.Length - (keyLength + ivLength))].ToArray();

        // Create an Aes object
        // with the specified key and IV.
        
        aes.Key = key;
        aes.IV = iv;

        // Create a decryptor to perform the stream transform.
        ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

        // Create the streams used for decryption.
        using MemoryStream msDecrypt = new(content);
        using CryptoStream csDecrypt = new(msDecrypt, decryptor, CryptoStreamMode.Read);
        using StreamReader srDecrypt = new(csDecrypt);

        // Read the decrypted bytes from the decrypting stream
        // and place them in a string.
        return srDecrypt.ReadToEnd();
    }
}
