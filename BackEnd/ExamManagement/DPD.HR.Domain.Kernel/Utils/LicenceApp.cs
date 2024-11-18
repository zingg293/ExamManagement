namespace DPD.HR.Kernel.Utils;

public class LicenceApp
{
    private static bool SetExpireDate()
    {
        var startDate = new DateTime(2023, 7, 20, 8, 00, 00);
        var expireDate = startDate.AddYears(1);

        if (DateTime.Now >= expireDate)
        {
            return false;
        }

        return true;
    }
}