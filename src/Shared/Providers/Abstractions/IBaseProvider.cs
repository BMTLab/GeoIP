using System.Runtime.CompilerServices;

using OneOf;

using Shared.ViewModels.Results;


namespace Shared.Providers.Abstractions;

public interface IBaseProvider<out TClient, TContent>
{
    Task<OneOf<TSuccess, ErrorResult>> HandleRequestAsync<TSuccess>
    (
        Func<TClient, Task<TContent>> request,
        [CallerMemberName] string memberName = ""
    ) where TSuccess : notnull, new();
}