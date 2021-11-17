namespace Server.Services.Repositories.Abstractions;

public abstract class BaseRepository
{
    protected readonly ILogger<BaseRepository>? Logger;


    #region Ctors
    protected BaseRepository
    (
        ILogger<BaseRepository>? logger = null
    )
    {
        Logger = logger;
    }
    #endregion
}