
using System.Globalization;

using JetBrains.Annotations;

using Localization;

using Microsoft.Net.Http.Headers;

using Server.Models.Constants;

using Shared.Constants;




namespace Server.Middlewares;

public sealed class LocalizationMiddleware
{
    #region Fields
    private readonly RequestDelegate _next;
    private readonly ILocalizationBuilder _localizationBuilder;
    #endregion


    #region Ctors
    public LocalizationMiddleware
    (
        RequestDelegate next,
        ILocalizationBuilder localizationBuilder
    )
    {
        _localizationBuilder = localizationBuilder;
        _next = next;
    }
    #endregion


    #region Methods
    [UsedImplicitly]
    public async Task InvokeAsync(HttpContext context)
    {
        #pragma warning disable S3358
        var lang =
            ((context.Request.Cookies.TryGetValue(CookieNames.Culture, out var cookieLang)
                 ? cookieLang

                 : context.Request.Headers.TryGetValue(HeaderNames.AcceptLanguage, out var headerLangs)
                     ? headerLangs[0].Split(',')[0]
                     : CultureNames.Default) ??

             CultureNames.Default)
               .ToUpperInvariant()
                switch
                {
                    "EN" => CultureNames.En, 
                    "EN-US" => CultureNames.En, 
                    "RU" => CultureNames.Ru, 
                    "RU-RU" => CultureNames.Ru, 
                    var _ => CultureNames.Default
                };
        #pragma warning restore S3358
        
        var culture = CultureInfo.GetCultureInfo(lang);
        
        _localizationBuilder.Culture = culture;

        await _next(context);
    }
    #endregion
}