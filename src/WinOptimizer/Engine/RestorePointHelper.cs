namespace WinOptimizer.Engine;

public static class RestorePointHelper
{
    public static async Task<bool> CreateRestorePointAsync(string description)
    {
        var script = $@"
            Enable-ComputerRestore -Drive 'C:\'
            Checkpoint-Computer -Description '{description.Replace("'", "''")}' -RestorePointType 'MODIFY_SETTINGS'
        ";

        var (success, _, _) = await PowerShellHelper.ExecuteAsync(script, 120000);
        return success;
    }
}
