using System.ComponentModel;

using JetBrains.Annotations;


namespace DataLoader.Options;


[PublicAPI]
public class GeoLiteOptions
{
    [DefaultValue("")]
    public string Api { get; set; } = string.Empty;

    [DefaultValue(@"geolitedb.zip")]
    public string ArchiveName { get; set; } = @"geolitedb.zip";

    [DefaultValue(@"ArchivesUpdate")]
    public string ArchiveDirectoryName { get; set; } = @"ArchivesUpdate";

    [DefaultValue(24)]
    public float UpdatePeriodHours { get; set; } = 24;

    [DefaultValue(true)]
    public bool IsEnabled { get; set; } = true;
}