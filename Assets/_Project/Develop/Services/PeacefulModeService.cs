public static class PeacefulModeService
{
    public static bool IsEnabled { get; private set; }

    public static void SetEnabled(bool isEnabled)
    {
        IsEnabled = isEnabled;
    }
}
