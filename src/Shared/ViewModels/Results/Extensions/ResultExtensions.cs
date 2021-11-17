using Shared.ViewModels.Results.Abstractions;


namespace Shared.ViewModels.Results.Extensions;

public static class ResultExtensions
{
    #region IBaseResult reductions
    public static IBaseResult Content<TContent>(TContent content) =>
        new ContentResult<TContent> { Success = content };
        
        
    public static IBaseResult Error(params string[] errors) =>
        new ErrorResult(errors);
        
        
    public static IBaseResult Error(IEnumerable<string> errors) =>
        new ErrorResult(errors);
        
        
    public static IBaseResult None() =>
        NoneResult.Default;
    #endregion _IBaseResult reductions
}