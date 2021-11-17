using Shared.ViewModels.Results.Abstractions;


namespace Shared.ViewModels.Results;

public record ContentResult<TSuccess> : BaseResult
{
    #region Ctors
    public ContentResult() {}
        
    public ContentResult(TSuccess success) =>
        Success = success;
    #endregion _Ctors


    public TSuccess Success { get; init; } = default!;


    #region Overrides of BaseResult
    public override bool IsSuccessful => true;
    #endregion
}