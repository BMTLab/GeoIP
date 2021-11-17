using Shared.ViewModels.Results.Abstractions;


namespace Shared.ViewModels.Results;

public record NoneResult : BaseResult
{
    public static readonly NoneResult Default = new();
        
    
    #region Overrides of BaseResult
    public override bool IsSuccessful => true;
    #endregion
}