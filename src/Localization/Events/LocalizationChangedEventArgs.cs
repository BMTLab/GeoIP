using System.Globalization;


namespace Localization.Events;

public sealed class LocalizationChangedEventArgs : EventArgs
{
    public LocalizationChangedEventArgs(CultureInfo culture) =>
        Culture = culture;

    public CultureInfo Culture { get; }
}