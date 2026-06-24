using WinOptimizer.Core;
using WinOptimizer.Core.Licensing;

namespace WinOptimizer.Licensing;

public class LicenseValidator
{
    public bool IsLicensed { get; private set; }
    public string? LicenseHolder { get; private set; }
    public DateTime? ExpiresAt { get; private set; }

    public bool Validate()
    {
        try
        {
            if (!File.Exists(Constants.LicenseFilePath))
                return false;

            var json = File.ReadAllText(Constants.LicenseFilePath);
            var licenseFile = LicenseFile.Deserialize(json);
            if (licenseFile == null) return false;

            var currentHwid = HwidHelper.GetHardwareId();
            if (licenseFile.HwidHash != CryptoHelper.ComputeHash(currentHwid))
                return false;

            var expectedSig = CryptoHelper.ComputeHash(
                licenseFile.EncryptedPayload + licenseFile.HwidHash + Constants.SignatureSecret);
            if (licenseFile.Signature != expectedSig)
                return false;

            var decrypted = CryptoHelper.Decrypt(licenseFile.EncryptedPayload, Constants.LicensePassphrase);
            var key = LicenseKey.Deserialize(decrypted);
            if (key == null) return false;

            if (key.HardwareId != currentHwid)
                return false;

            if (!key.IsValid)
                return false;

            IsLicensed = true;
            LicenseHolder = key.IssuedTo;
            ExpiresAt = key.ExpiresAt;
            return true;
        }
        catch
        {
            return false;
        }
    }

    public bool ActivateWithKey(string keyCode)
    {
        try
        {
            var decrypted = CryptoHelper.Decrypt(keyCode, Constants.LicensePassphrase);
            var key = LicenseKey.Deserialize(decrypted);
            if (key == null) return false;

            var currentHwid = HwidHelper.GetHardwareId();
            if (key.HardwareId != currentHwid)
                return false;

            if (!key.IsValid)
                return false;

            var licenseFile = new LicenseFile
            {
                EncryptedPayload = keyCode,
                HwidHash = CryptoHelper.ComputeHash(currentHwid)
            };
            licenseFile.Signature = CryptoHelper.ComputeHash(
                licenseFile.EncryptedPayload + licenseFile.HwidHash + Constants.SignatureSecret);

            Directory.CreateDirectory(Constants.AppDataPath);
            File.WriteAllText(Constants.LicenseFilePath, licenseFile.Serialize());

            IsLicensed = true;
            LicenseHolder = key.IssuedTo;
            ExpiresAt = key.ExpiresAt;
            return true;
        }
        catch
        {
            return false;
        }
    }

    public void Deactivate()
    {
        try
        {
            if (File.Exists(Constants.LicenseFilePath))
                File.Delete(Constants.LicenseFilePath);
        }
        catch { }
        IsLicensed = false;
        LicenseHolder = null;
        ExpiresAt = null;
    }
}
