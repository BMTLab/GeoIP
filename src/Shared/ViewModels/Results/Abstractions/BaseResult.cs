namespace Shared.ViewModels.Results.Abstractions;

public abstract record BaseResult : IBaseResult
{
    #region Implementation of IBaseResult
    public abstract bool IsSuccessful { get; }
    public string? TypeName => GetType().FullName;
    #endregion
}