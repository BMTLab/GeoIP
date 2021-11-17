using Shared.ViewModels.Results.Abstractions;


namespace Shared.ViewModels.Results;

public record ErrorResult : BaseResult
{
    #region Ctors
    public ErrorResult()
    {
    }
        
    public ErrorResult(string error)
    {
        Errors = new[] { error };
    }
        
    public ErrorResult(params string[] errors)
    {
        Errors = errors;
    }
        
        
    public ErrorResult(IEnumerable<string> errors)
    {
        Errors = errors;
    }
    #endregion
        
        
    public IEnumerable<string> Errors { get; set; } = Enumerable.Empty<string>();


    #region Overrides of BaseResult
    public override bool IsSuccessful => false;
    #endregion


    #region Overrides of Object
    public override string ToString() =>
        string.Join(", ", Errors);
    #endregion
}