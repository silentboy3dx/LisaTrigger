using System.Reflection;

namespace LisaTrigger.Core;

public class ProjectInfo
{
    private static ProjectInfo? instance = null;
    
    public string? ProductName { get; set; }
    public string? ProductVersion { get; set; }
    public string Author { get; set; } = "SilentBoy";
    
    public static ProjectInfo Instance
    {
        get { return instance ??= new ProjectInfo(); }
    }
    
    private ProjectInfo()
    {
        var assembly = Assembly.GetExecutingAssembly();

        ProductName = assembly?.GetName().Name;
        ProductVersion =  assembly?.GetName().Version.ToString();
    }
}