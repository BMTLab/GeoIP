namespace Shared.Providers.Models;

public class DataChangedEventArgs<TEntity> : EventArgs where TEntity: class
{
    #region Properties
    public TEntity NewEntity { get; init; } = default!;
        
    public DataChangeType ChangeType { get; init; }

    public IEnumerable<string>? Properties { get; init; } = Enumerable.Empty<string>();
    #endregion _Properties
}