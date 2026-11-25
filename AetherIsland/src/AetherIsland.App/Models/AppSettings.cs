namespace AetherIsland.Models;

public sealed class AppSettings
{
    public int SchemaVersion { get; set; } = 1;
    public string Theme { get; set; } = "Aether";
    public int FocusDurationMinutes { get; set; } = 25;
    public bool StartWithWindows { get; set; }
}
