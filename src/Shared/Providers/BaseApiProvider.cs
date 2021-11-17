using System.Net.Http.Json;
using System.Net.Mime;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;

using Localization;

using Microsoft.Extensions.Logging;

using OneOf;

using Shared.Providers.Abstractions;
using Shared.Utils.TypeExtensions.JsonSerializerExtensions;
using Shared.ViewModels.Results;


namespace Shared.Providers;

public abstract class BaseApiProvider : IBaseProvider<HttpClient, HttpResponseMessage>
{
    #region Fields
    protected readonly HttpClient Http;
    protected readonly ILocalization L;
    protected readonly ILogger<BaseApiProvider>? Logger;
    #endregion

        
    #region Ctors
    protected BaseApiProvider
    (
        HttpClient httpClient,
        ILocalization localization,
        ILogger<BaseApiProvider>? logger = null
    )
    {
        Http = httpClient;
        L = localization;
        Logger = logger;
            
        Logger?.LogInformation("Initialized");
    }
    #endregion


    #region Methods
    public async Task<OneOf<TSuccess, ErrorResult>> HandleRequestAsync<TSuccess>
    (
        Func<HttpClient, Task<HttpResponseMessage>> request,
        [CallerMemberName] string memberName = ""
    ) where TSuccess : notnull, new()
    {
        if (request is null)
            throw new ArgumentNullException(nameof(request));

        try
        {
            var response = await request.Invoke(Http);

            return await HandleResponseAsync<TSuccess>(response, memberName);
        }
        catch (HttpRequestException exc)
        {
            Logger?.LogWarning(exc, $"Lost connection when executing {memberName}");

            return new ErrorResult(L.ErrorLostConnection);
        }
        catch (Exception exc)
        {
            Logger?.LogError(exc, $"An unexpected error occurred while processing the request {memberName}");

            return new ErrorResult(L.ErrorInternal);
        }
    }
        
        
    public async Task<OneOf<TSuccess, ErrorResult>> HandleRequestAsync<TModel, TSuccess>
    (
        Func<HttpClient, Func<TModel, HttpContent>, Task<HttpResponseMessage>> request,
        [CallerMemberName] string memberName = ""
    ) where TSuccess : notnull
    {
        if (request is null)
            throw new ArgumentNullException(nameof(request));

        try
        {
            var response =
                await request.Invoke
                (
                    Http,
                    model => new StringContent(model.Serialize(), Encoding.UTF8, MediaTypeNames.Application.Json)
                );

            return await HandleResponseAsync<TSuccess>(response, memberName);
        }
        catch (HttpRequestException exc)
        {
            Logger?.LogWarning(exc, $"Lost connection when executing {memberName}");

            return new ErrorResult(L.ErrorLostConnection);
        }
        catch (Exception exc)
        {
            Logger?.LogError(exc, $"An unexpected error occurred while processing the request {memberName}");

            return new ErrorResult(L.ErrorInternal);
        }
    }
    

    protected async Task<OneOf<TSuccess, ErrorResult>> HandleResponseAsync<TSuccess>
    (
        HttpResponseMessage response,
        string memberName
    ) where TSuccess : notnull
    {
        if (response is null)
            throw new ArgumentNullException(nameof(response));
        
        
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<TSuccess>(GeoIpJsonSerializerOptions.Instance) ??
                   throw new SerializationException($"An attempt to deserialize the query result type: {typeof(TSuccess).Name} resulted with 'null'");
        }

        var error = await response.Content.ReadFromJsonAsync<ErrorResult>(GeoIpJsonSerializerOptions.Instance) ??
                    throw new SerializationException($"An attempt to deserialize the error result type: {typeof(TSuccess).Name} of query resulted with 'null'");

        Logger?.LogWarning($"{memberName} return code {Enum.GetName(response.StatusCode)} with errors: {error}");

        return error;
    }
    #endregion _Methods
}