using System.Security.Cryptography;
using System.Text;

namespace WinOptimizer.Core.Licensing;

public static class CryptoHelper
{
    private static readonly byte[] Salt = Encoding.UTF8.GetBytes("WinOpt!m1z3r_S@lt_2024");
    private const int KeySize = 256;
    private const int BlockSize = 128;
    private const int Iterations = 50000;

    public static string Encrypt(string plainText, string passphrase)
    {
        using var deriveBytes = new Rfc2898DeriveBytes(passphrase, Salt, Iterations, HashAlgorithmName.SHA256);
        var key = deriveBytes.GetBytes(KeySize / 8);
        var iv = deriveBytes.GetBytes(BlockSize / 8);

        using var aes = Aes.Create();
        aes.Key = key;
        aes.IV = iv;
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;

        using var encryptor = aes.CreateEncryptor();
        var plainBytes = Encoding.UTF8.GetBytes(plainText);
        var cipherBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);
        return Convert.ToBase64String(cipherBytes);
    }

    public static string Decrypt(string cipherText, string passphrase)
    {
        using var deriveBytes = new Rfc2898DeriveBytes(passphrase, Salt, Iterations, HashAlgorithmName.SHA256);
        var key = deriveBytes.GetBytes(KeySize / 8);
        var iv = deriveBytes.GetBytes(BlockSize / 8);

        using var aes = Aes.Create();
        aes.Key = key;
        aes.IV = iv;
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;

        using var decryptor = aes.CreateDecryptor();
        var cipherBytes = Convert.FromBase64String(cipherText);
        var plainBytes = decryptor.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);
        return Encoding.UTF8.GetString(plainBytes);
    }

    public static string ComputeHash(string input)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(input));
        return Convert.ToHexString(bytes);
    }
}
