using System.Globalization;

using JetBrains.Annotations;

using Localization.Events;
using Localization.Helpers;


namespace Localization;

[PublicAPI]
public class LocalizationBuilder : ILocalizationBuilder
{
    #region Fields
    private static Dictionary<string, ILocalization> _cached = new(2);
        
    private ILocalization? _localization;
    private CultureInfo? _currentCulture;
    #endregion


    #region Properties
    public static LocalizationOptions Options { get; [UsedImplicitly] set; } = new();

        
    [PublicAPI]
    public CultureInfo Culture
    {
        get
        {
            var culture = _currentCulture ?? GetCultureFromSource() ?? CultureInfo.CurrentCulture;
                
            /*CultureInfo.CurrentCulture = culture;
            CultureInfo.CurrentUICulture = culture;
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;*/

            return culture;
        }
        set
        {
            if (value is null)
                throw new ArgumentNullException(nameof(value), "Culture must have not null value");
                
            if (value.Equals(Culture))
                return;
                
            _currentCulture = SetCultureToSource(value);

            CultureInfo.CurrentCulture = value;
            CultureInfo.CurrentUICulture = value;
            Thread.CurrentThread.CurrentCulture = value;
            Thread.CurrentThread.CurrentUICulture = value;
            Thread.CurrentThread.CurrentCulture = value;
            Thread.CurrentThread.CurrentUICulture = value;

            OnChanged();
        }
    }

    [PublicAPI]
    public ILocalization Localization
    {
        get => _localization ?? GetOrBuild(Culture);
        set => _localization = value;
    }
    #endregion


    #region Methods
    //[RequiresUnreferencedCode("Calls Build")]
    internal static ILocalization GetOrBuild(CultureInfo culture)
    {
        if (culture is null)
            throw new ArgumentNullException(nameof(culture));

        var lang = culture.TwoLetterISOLanguageName;

        ILocalization localization;
        if (_cached.TryGetValue(lang, out var cached))
        {
            localization = cached;
        }
        else
        {
            localization = Build(lang);
            _cached.TryAdd(lang, localization);
        }
            
        return localization;
    }


    //[RequiresUnreferencedCode("Calls GetLocalizedType")]
    internal static ILocalization Build(in string language)
    {
        if (language is null || string.IsNullOrWhiteSpace(language))
            throw new ArgumentNullException(nameof(language));

        var loc = ReflectionHelpers.GetLocalizedType(language) as ILocalization ??
                  ReflectionHelpers.GetLocalizedType(Options.DefaultLanguage) as ILocalization ??
                  throw new ArgumentException($"Failed to create localization type with {language} lang");

        return loc;
    }


    protected virtual CultureInfo? GetCultureFromSource() =>
        null;


    protected virtual CultureInfo SetCultureToSource(CultureInfo culture) =>
        culture;
    #endregion _Methods


    #region Events & Invocators
    [PublicAPI]
    public event EventHandler<LocalizationChangedEventArgs>? Changed;

    //[RequiresUnreferencedCode("")]
    protected virtual void OnChanged()
    {
        Localization = GetOrBuild(Culture);

        if (Options.RaiseLocalizationChangedEvent)
            Changed?.Invoke(this, new LocalizationChangedEventArgs(Culture));
    }
    #endregion
}